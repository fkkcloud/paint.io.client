using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollision : MonoBehaviour {

    float collisionVelocity = 0.2f;
    float limitDotAngle = -0.1f;
    public Controller Control;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        
        if (!collision.gameObject.GetComponent<GameplayBlockCollision>())
            return;
            

        //Debug.Log("Contact Point:" + Vector3.Dot(collision.contacts[0].normal, transform.forward));
        //Debug.Log("Collision Velocity:" + collision.relativeVelocity.magnitude);
        if (collision.relativeVelocity.magnitude > collisionVelocity && 
            Vector3.Dot(collision.contacts[0].normal, transform.forward) < limitDotAngle)
        {
            //Debug.Log("Collide!!!!");
            Control.Bumping = true;
            Invoke("StopBumping", 0.1f);

            Control.CharacterObject.DowngradeCar();
        }
    }

    void StopBumping() {
        Control.Bumping = false;
    }
}
