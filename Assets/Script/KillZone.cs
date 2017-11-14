using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : IOBehavior {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (!GlobalGameState.IsPlayerDead && other.gameObject.transform.parent.gameObject.GetComponent<CarController>())
        {
            GlobalGameState.IsPlayerDead = true;

            Destroy(other.gameObject.transform.parent.gameObject);

            GlobalGameState.CreateCarLocalGameCar(Vector3.zero); // 0,0,0 re spawn pos is default temp
        }
        
    }
}
