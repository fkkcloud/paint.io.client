using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningNuts : MonoBehaviour {

    public float SpawnDist;

    List<SourceNut> Nuts = new List<SourceNut>();

    public int MaxNutCount = 1;

    public GameObject[] Prefabs;

    public float DelaySecond = 1f;

    public bool InitialLoading = true;

	// Use this for initialization
	void Start () {
        InvokeRepeating("CheckNutCount", 2f, 2f);

        if (InitialLoading) {
            for (int i = 0; i < MaxNutCount; i++) {
                SpawnNut();
            }
        }
	}

    void CheckNutCount() {
        if (Nuts.Count >= MaxNutCount)
            return;

        Invoke("SpawnNut", DelaySecond);
    }

    void SpawnNut() {
        float angle = Random.Range(0f, 360f);
        float x = Mathf.Cos(angle) * Random.Range(0f,SpawnDist);
        float z = Mathf.Sin(angle) * Random.Range(0f, SpawnDist);
        Vector3 pos = new Vector3(x, 0f, z) + transform.position;

        int i = Random.Range(0, 2);
        SourceNut nut = Instantiate(Prefabs[i], pos, Random.rotation).GetComponent<SourceNut>();

        if (!nut)
            Debug.LogError("Instantiated SourceNut is wrong type.");

        nut.OwnerList = Nuts;
        Nuts.Add(nut);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, SpawnDist);
    }
}
