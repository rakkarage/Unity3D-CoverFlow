using TouchScript.Gestures;
using TouchScript.Gestures.Simple;
using UnityEngine;
namespace ca.HenrySoftware.CoverFlow
{
	public class DetectSimplePan : MonoBehaviour
	{
		public float Threshold = 0.1f;
		public void OnEnable()
		{
			GetComponent<SimplePanGesture>().StateChanged += HandleSimplePanStateChanged;
		}
		public void OnDisable()
		{
			GetComponent<SimplePanGesture>().StateChanged -= HandleSimplePanStateChanged;
		}
		private void HandleSimplePanStateChanged(object sender, TouchScript.Events.GestureStateChangeEventArgs e)
		{
			SimplePanGesture target = sender as SimplePanGesture;
			switch (e.State)
			{
				case Gesture.GestureState.Began:
					FlowView.Instance.StopInertia();
					goto case Gesture.GestureState.Changed;
				case Gesture.GestureState.Changed:
					if (target.LocalDeltaPosition != Vector3.zero)
						FlowView.Instance.Flow(target.LocalDeltaPosition.x);
					break;
				case Gesture.GestureState.Ended:
					float velocity = (target.LocalTransformCenter.x - target.PreviousLocalTransformCenter.x) * 0.5f;
					if (Mathf.Abs(velocity) > Threshold)
						FlowView.Instance.Inertia(velocity);
					else
						FlowView.Instance.Flow();
					break;
			}
		}
	}
}
