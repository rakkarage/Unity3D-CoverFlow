using UnityEngine;
namespace ca.HenrySoftware.CoverFlow
{
	public class FlowView : Singleton<FlowView>
	{
		public float Time = 0.333f;
		public int Offset = 1;
		public bool Clamp = true;
		public GameObject[] Views;
		private int _clamp;
		private int _current;
		private int[] _tween;
		private int _tweenInertia;
		protected void Start()
		{
			_clamp = Views.Length * Offset + 1;
			_tween = new int[Views.Length];
		}
		public int GetClosestIndex()
		{
			int closestIndex = -1;
			float closestDistance = float.MaxValue;
			for (int i = 0; i < Views.Length; i++)
			{
				float distance = (Vector3.zero - Views[i].transform.localPosition).sqrMagnitude;
				if (distance < closestDistance)
				{
					closestIndex = i;
					closestDistance = distance;
				}
			}
			return closestIndex;
		}
		public void Flow()
		{
			Flow(GetClosestIndex());
		}
		private int GetIndex(GameObject view)
		{
			int found = -1;
			for (int i = 0; i < Views.Length; i++)
			{
				if (view == Views[i])
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
		public void Flow(int target)
		{
			for (int i = 0; i < Views.Length; i++)
			{
				int delta = (target - i) * -1;
				Vector3 to = new Vector3(delta * Offset, 0.0f, Mathf.Abs(delta) * Offset);
				LeanTween.cancel(Views[i], _tween[i]);
				_tween[i] = LeanTween.moveLocal(Views[i], to, Time).setEase(LeanTweenType.easeSpring).id;
			}
			_current = target;
		}
		public void Flow(float offset)
		{
			for (int i = 0; i < Views.Length; i++)
			{
				Vector3 p = Views[i].transform.localPosition;
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
				LeanTween.cancel(Views[i], _tween[i]);
				Views[i].transform.localPosition = newP;
			}
		}
		private float ClampXMin(int index, bool negative)
		{
			float newIndex = negative ? index : newIndex = Views.Length - index - 1;
			return -(_clamp - (Offset * newIndex));
		}
		private float ClampXMax(int index, bool negative)
		{
			float newIndex = negative ? index : newIndex = Views.Length - index - 1;
			return _clamp - (Offset * newIndex);
		}
		public void Inertia(float velocity)
		{
			StopInertia();
			_tweenInertia = LeanTween.value(gameObject, Flow, velocity, 0, 0.5f).setEase(LeanTweenType.easeInExpo).setOnComplete(Flow).id;
		}
		public void StopInertia()
		{
			LeanTween.cancel(gameObject, _tweenInertia);
		}
		protected void OnGUI()
		{
			if (GUI.Button(new Rect(10.0f, 10.0f, 64.0f, 64.0f), "<"))
			{
				if (_current > 0)
				{
					Flow(_current - 1);
				}
			}
			if (GUI.Button(new Rect(10.0f, 64.0f, 64.0f, 64.0f), ">"))
			{
				if (_current < Views.Length - 1)
				{
					Flow(_current + 1);
				}
			}
		}
	}
}
