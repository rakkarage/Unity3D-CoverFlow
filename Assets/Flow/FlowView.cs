using UnityEngine;
namespace ca.HenrySoftware.Deko
{
	public class FlowView : Singleton<FlowView>
	{
		public float Time = 0.333f;
		public int Offset = 1;
		public GameObject[] views;
		private int _current;
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
		public int GetClosestIndex()
		{
			int closestIndex = -1;
			float closestDistance = float.MaxValue;
			for (int i = 0; i < views.Length; i++)
			{
				float distance = (Vector3.zero - views[i].transform.position).sqrMagnitude;
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
				LeanTween.move(views[i], to, Time).setEase(LeanTweenType.easeSpring);
			}
			_current = target;
		}
		public void Flow(float offset)
		{
			for (int i = 0; i < views.Length; i++)
			{
				Vector3 p = views[i].transform.position;
				float newx = p.x + offset;
				Vector3 newp = new Vector3(newx, p.y, Mathf.Abs(newx));
				views[i].transform.position = newp;
			}
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
