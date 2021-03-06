﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCharacter : MonoBehaviour {

    public CarUpgradeData[] UpgradeData;

	public string id;
	public float latestElapsedTime;

    public ProceduralMeshgen Painter;

    int CurrentCarLevel = 0;
    int ConsumedNut = 0;

	public bool IsSimulated;

	public Vector3 simulatedEndPos;
	public Quaternion simulatedBodyEndRot;

    public Controller CharacterController;

	public Rigidbody Rb;

    private float UpgradeDuration = 1.0f;

    [HideInInspector]
    public bool IsUpgrading = false;

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

        ApplyCarData();

    }

    void ConsumeNut() {

        /* if car is upgrading or can't upgrade no more - dont! */
        if (IsUpgrading || CurrentCarLevel >= UpgradeData.Length - 1)
            return;

        IsUpgrading = true;
        ConsumedNut++;
        if (ConsumedNut >= UpgradeData[CurrentCarLevel].RequireNutCount)
            UpgradeCar();

        Invoke("UpgradeCarDone", UpgradeDuration);
    }

    private void UpgradeCarDone() {
        IsUpgrading = false;
    }

    public void DowngradeCar() {

        if (CurrentCarLevel == 0)
            return;

        CurrentCarLevel--;

        ApplyCarData();
    }

    void UpgradeCar() {
        CurrentCarLevel++;

#if UNITY_EDITOR
        if (CurrentCarLevel > UpgradeData.Length - 1)
            return;
        // only for cheating purposes / editor use only
        if (ConsumedNut < UpgradeData[CurrentCarLevel].RequireNutCount)
            ConsumedNut = UpgradeData[CurrentCarLevel].RequireNutCount;
#endif

        ApplyCarData();
    }

    void ApplyCarData() {
        Painter.DetachMesh();

        float newScale = UpgradeData[CurrentCarLevel].CarSize;
        transform.localScale = new Vector3(newScale, newScale, newScale);
        Painter.Width = UpgradeData[CurrentCarLevel].PaintWidth;

        CharacterController.MoveSpeed = UpgradeData[CurrentCarLevel].CarSpeed;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (IsUpgrading)
            return;

        SourceNut nut = collision.gameObject.GetComponent<SourceNut>();
        if (nut)
        {
            ConsumeNut();
            nut.Consume();
        }
    }

    // Update is called once per frame
    void Update () {

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.U))
            UpgradeCar();
#endif

        if (IsSimulated) {
            Vector3 currPos = transform.position; //new Vector3(transform.position.x, 0f, transform.position.z);
			transform.position = Vector3.SmoothDamp (currPos, simulatedEndPos, ref Velocity, positionSyncSpeed * Time.deltaTime);
			transform.rotation = Quaternion.RotateTowards (transform.rotation, simulatedBodyEndRot, rotationSyncSpeed * Time.deltaTime);
		}

        // Lock Y
        Vector3 pos = transform.position;
        //pos.y = 0f;
        transform.position = pos;
		
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

			Vector3 newBodyRotation = new Vector3 (0f, 0f + transform.rotation.eulerAngles.y, 0f /*dot2 * dot * RotateAmount + transform.rotation.eulerAngles.z*/);
			transform.rotation = Quaternion.Euler (newBodyRotation);

			prevForward = currForward;
		}
	}
}
