using TouchScript.Gestures;
using TouchScript.Gestures.Simple;
using UnityEngine;
namespace ca.HenrySoftware.CoverFlow
{
	public class DetectSimplePan : MonoBehaviour
	{
		public void OnEnable()
		{
			GetComponent<SimplePanGesture>().StateChanged += HandleSimplePanStateChanged;
		}
		public void OnDisable()
		{
			GetComponent<SimplePanGesture>().StateChanged -= HandleSimplePanStateChanged;
		}
		private void HandleSimplePanStateChanged(object s, TouchScript.Events.GestureStateChangeEventArgs e)
		{
			SimplePanGesture target = s as SimplePanGesture;
			switch (e.State)
			{
				case Gesture.GestureState.Began:
				case Gesture.GestureState.Changed:
					if (target.LocalDeltaPosition != Vector3.zero)
					{
						FlowView.Instance.Flow(target.LocalDeltaPosition.x);
					}
					break;
				case Gesture.GestureState.Ended:
					FlowView.Instance.Inertia(target.LocalTransformCenter.x - target.PreviousLocalTransformCenter.x);
					break;
			}
		}
	}
}
