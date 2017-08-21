using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class GameState : IOBehavior {

	string CurrentPlayerID;
	string ChannelName;

	public GameObject CarPrefab;

	public List<CarCharacter> cars = new List<CarCharacter>();

	// Use this for initialization
	void Start () {
		Application.runInBackground = true;
		Application.targetFrameRate = 30;
		SetupNetworks ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SetupNetworks(){

		SocketIOComp.On ("CLIENT:CONNECTED", OnServerConnected);

		SocketIOComp.On ("CLIENT:JOINED", OnUserJoined);
		SocketIOComp.On ("CLIENT:OTHER_JOINED", OnOtherUserJoined);

		SocketIOComp.On ("CLIENT:MOVE", OnMove);
	}

	private void OnServerConnected(SocketIOEvent evt){

		Debug.Log ("SERVER CONNECTED");
		JoinGame ();

	}

	void JoinGame(){
		SocketIOComp.Emit ("SERVER:JOIN");
	}

	private void OnUserJoined(SocketIOEvent evt){

		Debug.Log ("Game joined");
		// update room name
		ChannelName = Utility.JsonToString(evt.data.GetField("room").ToString(), "\"");
		CurrentPlayerID = Utility.JsonToString(evt.data.GetField("id").ToString(), "\"");

	}

	private void OnOtherUserJoined(SocketIOEvent evt){
		bool isSimulated = true;

		Debug.Log ("Create other car");

		CarCharacter car = CreateCar (evt, isSimulated, CarPrefab);

		//car.Rb.isKinematic = true;
		//car.Rb.useGravity = false;
		cars.Add(car);
	}

	private CarCharacter CreateCar(SocketIOEvent evt, bool isSimulated, GameObject CarPrefab){
		GameObject go = Instantiate (CarPrefab);
		CarCharacter car = go.GetComponent<CarCharacter> ();

		string id = "";
		JSONObject obj = evt.data.GetField ("id");
		if (obj) {
			id = Utility.JsonToString (obj.ToString (), "\"");
		}

		car.id = id;
		car.IsSimulated = isSimulated;
		return car;
	}

	private void OnMove(SocketIOEvent evt){

		float elapsedTime = Utility.JsonToFloat (evt.data.GetField ("elapsedTime").ToString (), "\"");
		string id = Utility.JsonToString(evt.data.GetField("id").ToString(), "\"");

		CarCharacter car = FindCarByID (id);
		if (elapsedTime < car.latestElapsedTime)
			return;

		Vector3 transform = Utility.StringToVecter3( Utility.JsonToString(evt.data.GetField("transform").ToString(), "\"") );

		car.latestElapsedTime = elapsedTime;

		car.simulatedEndPos = new Vector3(transform.x, 0f, transform.y);
		car.simulatedBodyEndRot = Quaternion.Euler(new Vector3(0f, transform.z, 0f)); // body only yaw
	}

	private CarCharacter FindCarByID(string id){
		foreach (CarCharacter car in cars){
			if (car.id == id)
				return car;
		}

		return null;
	}
}
