using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCharacter : MonoBehaviour {

	public string id;
	public float latestElapsedTime;

	public bool IsSimulated;

	public Vector3 simulatedEndPos;
	public Quaternion simulatedBodyEndRot;

	public Rigidbody Rb;

	Vector2 prevForward;
	Vector2 currForward;
	Vector2 currRight;

	Vector3 Velocity;

	public float positionSyncSpeed;
	public float rotationSyncSpeed;

	public float RotateAmount = 1f;

	// Use this for initialization
	void Start () {
		prevForward = new Vector2(transform.forward.x, transform.forward.z);
		currForward = new Vector2(transform.forward.x, transform.forward.z);
		currRight = new Vector2 (transform.right.x, transform.right.z);
	}
	
	// Update is called once per frame
	void Update () {

		if (IsSimulated) {
			Vector3 currPos = new Vector3(transform.position.x, 0f, transform.position.z);
			transform.position = Vector3.SmoothDamp (currPos, simulatedEndPos, ref Velocity, positionSyncSpeed * Time.deltaTime);
			transform.rotation = Quaternion.RotateTowards (transform.rotation, simulatedBodyEndRot, rotationSyncSpeed * Time.deltaTime);
		}


		// Animation of Car
		currForward.x = transform.forward.x;
		currForward.y = transform.forward.z;
		if (currForward != prevForward) {

			// rotation amount
			float dot = Vector2.Dot (currForward, prevForward);

			// figure out right or left
			currRight.x = transform.right.x;
			currRight.y = transform.right.z;
			float dot2 = Vector2.Dot (prevForward, currRight);

			/*
			if (dot2 > 0f)
				Debug.Log ("Left");
			else
				Debug.Log ("Right");
			Debug.Log (dot);*/

			Vector3 newBodyRotation = transform.rotation.eulerAngles + new Vector3 (0f, 0f, dot2 * dot * RotateAmount);
			transform.rotation = Quaternion.Euler (newBodyRotation);

			prevForward = currForward;
		}
	}
}
