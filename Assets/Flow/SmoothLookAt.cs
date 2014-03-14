using System.Collections;
using UnityEngine;
public class SmoothLookAt : MonoBehaviour
{
	public Transform Target;
	public float Damping = 6.0f;
	public bool Smooth = true;
	private void Start()
	{
		if (rigidbody)
			rigidbody.freezeRotation = true;
	}
	protected void LateUpdate()
	{
		if (Target)
		{
			if (Smooth)
			{
				var rotation = Quaternion.LookRotation(Target.position - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * Damping);
			}
			else
			{
				transform.LookAt(Target);
			}
		}
	}
}
