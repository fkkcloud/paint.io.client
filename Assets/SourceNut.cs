using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceNut : MonoBehaviour {

    public List<SourceNut> OwnerList;

    public void Consume() {
        OwnerList.Remove(this);
        Destroy(gameObject, 0.2f);
    }
}
