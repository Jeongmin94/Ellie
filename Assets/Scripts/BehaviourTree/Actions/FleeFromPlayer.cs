using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEngine.AI;

[System.Serializable]
public class FleeFromPlayer : ActionNode
{
    public NodeProperty<float> fleeSpeed;
    public NodeProperty<float> fleeDistance;
    public NodeProperty<Vector3> playerPosition;


    private Vector3 directionVector;
    private Vector3 runAwayVector;
    private Vector3 fleeVector;
    private int attemptFlee;

    protected override void OnStart() {
        context.agent.speed = fleeSpeed.Value;
        attemptFlee = 0;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (attemptFlee < 5)
        {
            directionVector = playerPosition.Value - context.transform.position;
            directionVector.y = 0.0f;
            runAwayVector = directionVector.normalized * -fleeDistance.Value;
            fleeVector = context.transform.position + runAwayVector;

            //monster look at player
            Quaternion targetRotation = Quaternion.LookRotation(directionVector);
            context.transform.rotation = Quaternion.Slerp(context.transform.rotation, targetRotation, 180.0f * Time.deltaTime);

            //check if there are any wall
            RaycastHit hit;
            if (Physics.Raycast
                (context.transform.position, runAwayVector.normalized, out hit, fleeDistance.Value))
            {
                if (hit.collider.tag == "Wall")
                {
                    Vector3 newDirection = Vector3.Cross(Vector3.up, runAwayVector);
                    runAwayVector = newDirection * fleeDistance.Value;
                    fleeVector = context.transform.position + runAwayVector;
                }
            }

            //check if flee vector is on navmesh
            NavMeshHit navMeshHit;
            if (NavMesh.SamplePosition(fleeVector, out navMeshHit, 1.0f, NavMesh.AllAreas))
            {
                context.agent.destination = navMeshHit.position;
                return State.Success;
            }

            attemptFlee++;
            return State.Running;
        }
        else
            return State.Failure;
    }
}
