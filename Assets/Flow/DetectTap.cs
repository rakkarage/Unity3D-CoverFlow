using TouchScript.Gestures;
using TouchScript.Gestures.Simple;
using UnityEngine;
namespace ca.HenrySoftware.CoverFlow
{
	public class DetectTap : MonoBehaviour
	{
		public float Scale = 1.1f;
		public float Time = 0.333f;
		private Vector3 _origiginalScale;
		private int _tweenUp;
		private int _tweenDown;
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
		protected void Awake()
		{
			_origiginalScale = gameObject.transform.localScale;
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
				FlowView.Instance.StopInertia();
				PressGesture gesture = sender as PressGesture;
				ScaleUp(gesture.gameObject);
			}
		}
		private void ScaleUp(GameObject o)
		{
			LeanTween.cancel(o, _tweenUp);
			LeanTween.cancel(o, _tweenDown);
			Vector3 to = Vector3.Scale(_origiginalScale, new Vector3(Scale, Scale, 1.0f));
			_tweenUp = LeanTween.scale(o, to, Time).setEase(LeanTweenType.easeSpring).id;
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
			_tweenDown = LeanTween.scale(o, _origiginalScale, Time).setEase(LeanTweenType.easeSpring).id;
		}
	}
}
