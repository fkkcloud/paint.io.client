using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : Controller {

    public CarCharacter CharacterObject;

    public float Sensitivity = 1f;

    public NavMeshAgent agent;

    Vector3 CurrentDestination;

    public GameObject CenterAnchor;
    public float Radius;

    void Start() {
        agent.isStopped = false;

        Invoke("SetAIDestination", 0.1f);
    }

    void SetAIDestination() {
        Vector3 randomDirection = Random.insideUnitSphere * Radius + CenterAnchor.gameObject.transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, Radius, 1);
        CurrentDestination = hit.position;
        agent.SetDestination(CurrentDestination);

        float nextDelay = Random.Range(1f, 4f);
        Invoke("SetAIDestination", nextDelay);
    }

    // Update is called once per frame
    void Update()
    {
        agent.speed = MoveSpeed;

        if (Vector3.Distance(transform.position, CurrentDestination) < 1f)
        {
            CancelInvoke("SetAIDestination");
            SetAIDestination();
        }
    }

}
