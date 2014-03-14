using UnityEngine;
namespace ca.HenrySoftware.CoverFlow
{
	public class FlowView : Singleton<FlowView>
	{
		public float Time = 0.333f;
		public int Offset = 1;
		public bool Clamp = true;
		public GameObject[] views;
		private int _clamp = 0;
		private int _current;
		protected void Start()
		{
			_clamp = views.Length * Offset + 1;
		}
		private int GetIndex(GameObject view)
		{
			int found = -1;
			for (int i = 0; i < views.Length; i++)
			{
				if (view == views[i])
				{
					found = i;
				}
			}
			return found;
		}
		public void Flow(GameObject target)
		{
			int found = GetIndex(target);
			if (found != -1)
			{
				Flow(found);
			}
		}
		public void Flow()
		{
			Flow(GetClosestIndex());
		}
		public int GetClosestIndex()
		{
			int closestIndex = -1;
			float closestDistance = float.MaxValue;
			for (int i = 0; i < views.Length; i++)
			{
				float distance = (Vector3.zero - views[i].transform.localPosition).sqrMagnitude;
				if (distance < closestDistance)
				{
					closestIndex = i;
					closestDistance = distance;
				}
			}
			return closestIndex;
		}
		public void Flow(int target)
		{
			for (int i = 0; i < views.Length; i++)
			{
				int delta = (target - i) * -1;
				Vector3 to = new Vector3(delta * Offset, 0.0f, Mathf.Abs(delta) * Offset);
				LeanTween.moveLocal(views[i], to, Time).setEase(LeanTweenType.easeSpring);
			}
			_current = target;
		}
		public void Flow(float offset)
		{
			for (int i = 0; i < views.Length; i++)
			{
				Vector3 p = views[i].transform.localPosition;
				float newX = p.x + offset;
				bool negative = newX < 0;
				Vector3 newP;
				if (Clamp)
				{
					float clampX = Mathf.Clamp(newX, ClampXMin(i, negative), ClampXMax(i, negative));
					float clampZ = Mathf.Clamp(Mathf.Abs(newX), 0.0f, ClampXMax(i, negative));
					newP = new Vector3(clampX, p.y, clampZ);
				}
				else
				{
					newP = new Vector3(newX, p.y, Mathf.Abs(newX));
				}
				views[i].transform.localPosition = newP;
			}
		}
		private float ClampXMin(int index, bool negative)
		{
			float newIndex = negative ? index : newIndex = views.Length - index - 1;
			return -(_clamp - (Offset * newIndex));
		}
		private float ClampXMax(int index, bool negative)
		{
			float newIndex = negative ? index : newIndex = views.Length - index - 1;
			return _clamp - (Offset * newIndex);
		}
		public void Inertia(float velocity)
		{
			LeanTween.value(gameObject, Flow, velocity, 0, 0.5f).setEase(LeanTweenType.easeInExpo).setOnComplete(Flow);
		}
		protected void OnGUI()
		{
			if (GUI.Button(new Rect(10.0f, 10.0f, 20.0f, 20.0f), "<"))
			{
				if (_current > 0)
				{
					Flow(_current - 1);
				}
			}
			if (GUI.Button(new Rect(10.0f, 30.0f, 20.0f, 20.0f), ">"))
			{
				if (_current < 5)
				{
					Flow(_current + 1);
				}
			}
		}
	}
}
