using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : Controller {

	public Joystick UserInputControl;

	public CarCharacter CharacterObject;

    Vector3 CurrentVelocity;

    [HideInInspector]
    public float BoostValue = 1.0f;

    float PacketTimer = 0f;
	float PacketLimit = 35.0f / 1000f;

	public float RotationSensitivity = 200f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		UpdateInput ();
	}

	void UpdateInput(){

		// rotation //------------------------------------------------------------
		// default when there is no position input from player

		Vector3 newBodyRotation = CharacterObject.transform.rotation.eulerAngles; 
		float yaw = UserInputControl.Yaw ();

//#if UNITY_EDITOR
        // debug purpose
        yaw = Input.GetAxis("Horizontal");
//#endif

        if (yaw != 0f) {
			// reminder - Euler(pitch , yaw , roll)
			newBodyRotation = CharacterObject.transform.rotation.eulerAngles + new Vector3 (0f, yaw * RotationSensitivity * Time.deltaTime, 0f);
			CharacterObject.transform.rotation = Quaternion.Euler (newBodyRotation);
		}

		Vector3 newPosition = CharacterObject.transform.position;
        Vector3 targetPosition = CharacterObject.transform.position + CharacterObject.transform.forward * MoveSpeed * BoostValue;
        CharacterObject.transform.position = Vector3.SmoothDamp(CharacterObject.transform.position, targetPosition, ref CurrentVelocity, 0.3f);

        newPosition = CharacterObject.transform.position;

		// network //------------------------------------------------------------
		PacketTimer += Time.deltaTime;
		if (PacketTimer > PacketLimit) {
			Dictionary<string, string> data = new Dictionary<string, string> ();
			data ["transform"] = newPosition.x + "," + newPosition.z + "," + newBodyRotation.y;
			data ["elapsedTime"] = Time.timeSinceLevelLoad.ToString();
			SocketIOComp.Emit("SERVER:MOVE", new JSONObject(data));

			PacketTimer = 0f;
		}
	}
}
