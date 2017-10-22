using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BoostBtn : MonoBehaviour {

    public float AdditionalBoostDelay = 3f;
    public float BoostDuration = 1f;
    public float BoostSpeed = 2f;
    public CarController CarChar;

    public Button btn;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        /*
         * Debug purposes
         * */
        if (btn.interactable && Input.GetKeyDown(KeyCode.M))
            Boost();


    }

    void EnableBoost() {
        btn.interactable = true;
    }

    public void Boost() {
        btn.interactable = false;

        LeanTween.value(gameObject, (value) =>
        {
            CarChar.BoostValue = value;
        }, 1f, BoostSpeed, BoostDuration).setLoopPingPong(1).setOnComplete(() =>
        {
            Invoke("EnableBoost", AdditionalBoostDelay);
        }).setEaseOutExpo();

    }

}
