using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollision : MonoBehaviour {

    public float CollisionVelocity = 3f;

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

        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
            Debug.Log("Contact Point:" + Vector3.Dot(contact.normal, transform.forward));
        }

        Debug.Log("Collision Velocity:" + collision.relativeVelocity.magnitude);
        if (collision.relativeVelocity.magnitude > CollisionVelocity)
        {
            Debug.Log("Collide!!!!");
        }
    }
}
