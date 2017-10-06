using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCameraController : IOBehavior {

	public GameObject CameraTarget;

	private Vector3 diffVector;
	private Vector3 targetDestination;
	//private Vector3 currentDestination;

	[HideInInspector]
	public Vector3 CamVelocity = Vector3.zero;

	//[HideInInspector]
	//public Vector3 CamRotVelocity = Vector3.zero;

	public float smoothTime = 0.15f;

	void Start(){
        diffVector = CameraTarget.transform.position - transform.position;
	}

	public void Setup(GameObject target)
	{
		//CameraTarget = target;

		//currentDestination = CameraTarget.transform.position + new Vector3 (0f, LookAtHeightOffset, 0f);
	}

	// Update is called once per frame
	void LateUpdate () {

		if (CameraTarget) {

            targetDestination = CameraTarget.transform.position - diffVector;
			transform.position = Vector3.SmoothDamp (transform.position, targetDestination, ref CamVelocity, smoothTime);

			//targetDestination = CameraTarget.transform.position + new Vector3 (0f, LookAtHeightOffset, 0f);
			//currentDestination = Vector3.SmoothDamp (currentDestination, targetDestination, ref CamRotVelocity, dampTime);

			//transform.LookAt (currentDestination);
		}
	}

	public void ShakeCamera(){
		StartCoroutine(Shake());
	}

	IEnumerator Shake() {

		float elapsed = 0.0f;
		float duration = 0.1f;
		float magnitude = 0.86f;

		Vector3 originalCamPos = transform.position;

		while (elapsed < duration) {

			elapsed += Time.deltaTime;          

			float percentComplete = elapsed / duration;         
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

			// map value to [-1, 1]
			float x = Random.value * 2.0f - 1.0f;
			float y = Random.value * 2.0f - 1.0f;
			x *= magnitude * damper;
			y *= magnitude * damper;

			transform.position = new Vector3(originalCamPos.x + x, originalCamPos.y + y, originalCamPos.z);

			yield return null;
		}

		transform.position = originalCamPos;
	}
}
