using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : IOBehavior
{


    protected Vector3 CurrentVelocity;
    public CarCharacter CharacterObject;

    [HideInInspector]
    public float MoveSpeed;

    [HideInInspector]
    public bool Bumping = false;

}
