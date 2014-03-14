using TouchScript.Gestures;
using TouchScript.Gestures.Simple;
using UnityEngine;
namespace ca.HenrySoftware.CoverFlow
{
	public class DetectTap : MonoBehaviour
	{
		private Vector3 _origiginalScale;
		public void OnEnable()
		{
			GetComponent<TapGesture>().StateChanged += HandleTap;
			GetComponent<PressGesture>().StateChanged += HandlePress;
			GetComponent<ReleaseGesture>().StateChanged += HandleRelease;
		}
		public void OnDisable()
		{
			GetComponent<TapGesture>().StateChanged -= HandleTap;
			GetComponent<PressGesture>().StateChanged -= HandlePress;
			GetComponent<ReleaseGesture>().StateChanged -= HandleRelease;
		}
		private void HandleTap(object sender, TouchScript.Events.GestureStateChangeEventArgs e)
		{
			if (e.State == Gesture.GestureState.Recognized)
			{
				FlowView.Instance.Flow(gameObject);
			}
		}
		private void HandlePress(object sender, TouchScript.Events.GestureStateChangeEventArgs e)
		{
			if (e.State == Gesture.GestureState.Recognized)
			{
				PressGesture gesture = sender as PressGesture;
				ScaleUp(gesture.gameObject);
			}
		}
		private void ScaleUp(GameObject o)
		{
			_origiginalScale = o.transform.localScale;
			Vector3 scaleTo = Vector3.Scale(o.transform.localScale, new Vector3(1.1f, 1.1f, 1.0f));
			LeanTween.scale(o, scaleTo, 0.333f).setEase(LeanTweenType.easeSpring);
		}
		private void HandleRelease(object sender, TouchScript.Events.GestureStateChangeEventArgs e)
		{
			if (e.State == Gesture.GestureState.Recognized)
			{
				ReleaseGesture gesture = sender as ReleaseGesture;
				ScaleDown(gesture.gameObject);
			}
		}
		private void ScaleDown(GameObject o)
		{
			LeanTween.scale(o, _origiginalScale, 0.333f).setEase(LeanTweenType.easeSpring);
		}
	}
}
