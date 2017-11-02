using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : Controller {


    public float Sensitivity = 1f;

    public NavMeshAgent agent;

    Vector3 CurrentDestination;

    public float Radius;

    void Start() {
        agent.isStopped = false;

        Invoke("SetAIDestination", 0.1f);
    }

    void SetAIDestination() {
        Vector3 randomDirection = Random.insideUnitSphere * Radius + Vector3.zero;
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

        Vector3 newPosition = CharacterObject.transform.position;
        Vector3 targetPosition = newPosition;
        
        float smoothTime = 0.3f;
        if (Bumping)
        {
            agent.speed = 0f;
            smoothTime = .1f;
            targetPosition = CharacterObject.transform.position + -CharacterObject.transform.forward * BumpRate;
        }

        CharacterObject.transform.position = Vector3.SmoothDamp(CharacterObject.transform.position, targetPosition, ref CurrentVelocity, smoothTime);
        newPosition = CharacterObject.transform.position;

        if (Vector3.Distance(transform.position, CurrentDestination) < 0.5f)
        {
            CancelInvoke("SetAIDestination");
            SetAIDestination();
        }
    }

}
