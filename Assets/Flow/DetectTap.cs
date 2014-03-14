using TouchScript.Gestures;
using TouchScript.Gestures.Simple;
using UnityEngine;
namespace ca.HenrySoftware.CoverFlow
{
	public class DetectTap : MonoBehaviour
	{
		public void OnEnable()
		{
			GetComponent<TapGesture>().StateChanged += HandleTapStateChanged;
		}
		public void OnDisable()
		{
			GetComponent<TapGesture>().StateChanged -= HandleTapStateChanged;
		}
		private void HandleTapStateChanged(object s, TouchScript.Events.GestureStateChangeEventArgs e)
		{
			if (e.State == Gesture.GestureState.Recognized)
			{
				FlowView.Instance.Flow(gameObject);
			}
		}
	}
}
