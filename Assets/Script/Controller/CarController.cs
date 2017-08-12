using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

	public UserControl UserInputControl;

	public CarCharacter CharacterObject;

	float PacketTimer = 0f;
	float PacketLimit = 35.0f / 1000f;

	public float Sensitivity = 1f;
	public float MoveSpeed = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		UpdateInput ();
	}

	void UpdateInput(){

		if (UserInputControl && CharacterObject) 
		{
			bool IsTransformDirty = false;

			// rotation //------------------------------------------------------------
			// default when there is no position input from player

			Vector3 newBodyRotation = CharacterObject.transform.rotation.eulerAngles; 
			float yaw = UserInputControl.Horizontal ();
			if (yaw != 0f) {


				// reminder - Euler(pitch , yaw , roll)
				newBodyRotation = CharacterObject.transform.rotation.eulerAngles + new Vector3 (0f, yaw * Sensitivity * Time.deltaTime, 0f);

				CharacterObject.transform.rotation = Quaternion.Euler (newBodyRotation);

				IsTransformDirty = true;
			}

			Vector3 newPosition = CharacterObject.transform.position;
			CharacterObject.Rb.MovePosition (CharacterObject.Rb.position +
				CharacterObject.transform.forward * MoveSpeed * Time.deltaTime);

			newPosition = CharacterObject.transform.position;

			IsTransformDirty = true;

			// position //------------------------------------------------------------
			/*
			Vector3 newPosition = CharacterObject.transform.position;
			float moveValue = UserInputControl.Vertical ();
			moveValue = moveValue * 0.725f; // resist too much running!
			// TODO: have to get walking animation turn off for local character when they are stuck in the wall and not moving
			if (moveValue != 0f) {

				CharacterObject.Rb.MovePosition (CharacterObject.Rb.position +
					CharacterObject.transform.forward * MoveSpeedB * moveValue * Time.deltaTime);

				newPosition = CharacterObject.transform.position;

				IsTransformDirty = true;

				if (Mathf.Abs(moveValue) > 0.075f) {
					CharacterObject.Anim.SetFloat (anim_speed_f, Mathf.Abs(moveValue));
				} else {
					CharacterObject.Anim.SetFloat (anim_speed_f, 0f);
				}
				CharacterObject.Anim.SetFloat (anim_direction, moveValue > 0f ? 1f : -1f);

				if (moveValue > 0.7f)
					CharacterObject.IsRunning = true;
				else
					CharacterObject.IsRunning = false;
				CharacterObject.IsMoving = true;
			} else {
				CharacterObject.Anim.SetFloat (anim_speed_f, 0f);
				CharacterObject.IsMoving = false;
				CharacterObject.IsRunning = false;
				//CharacterObject.Anim.SetFloat ("Direction", moveValue > 0f ? 1f : -1f);
			}*/

			// network //------------------------------------------------------------

			/*
			PacketTimer += Time.deltaTime;
			if (PacketTimer > PacketLimit && IsTransformDirty) {
				Dictionary<string, string> data = new Dictionary<string, string> ();
				data ["transform"] = newPosition.x + "," + newPosition.z + "," + newBodyRotation.y;
				data ["ctrl_val"] = moveValue.ToString();
				data ["elapsedTime"] = Time.timeSinceLevelLoad.ToString();
				SocketIOComp.Emit("SERVER:MOVE", new JSONObject(data));

				PacketTimer = 0f;
			}*/
		}
	}
}
