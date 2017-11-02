using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : Controller {

	public Joystick UserInputControl;

    [HideInInspector]
    public float BoostValue = 1.0f;

    float PacketTimer = 0f;
	float PacketLimit = 35.0f / 1000f;

    float CarCollisionBounceDist = 0.6f;

	public float RotationSensitivity = 200f;

    Vector3 refRot;

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
        Vector3 targetPosition = newPosition;
        float smoothTime = 0.3f;
        if (Bumping)
        {
            smoothTime = 0.038f;
            
            CharacterObject.transform.rotation = Quaternion.Euler( Vector3.SmoothDamp(CharacterObject.transform.rotation.eulerAngles, CharacterObject.transform.rotation.eulerAngles + new Vector3(0f, Random.Range(-15f, 15f), 0f), ref refRot, 0.02f) );
            targetPosition = CharacterObject.transform.position + -CharacterObject.transform.forward * CarCollisionBounceDist * BumpRate;
        }
        else {
            targetPosition = CharacterObject.transform.position + CharacterObject.transform.forward * MoveSpeed * BoostValue;
        }

        CharacterObject.transform.position = Vector3.SmoothDamp(CharacterObject.transform.position, targetPosition, ref CurrentVelocity, smoothTime);
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
