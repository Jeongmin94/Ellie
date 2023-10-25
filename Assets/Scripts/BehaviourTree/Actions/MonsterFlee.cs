using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEngine.AI;

[System.Serializable]
public class MonsterFlee : ActionNode
{
    public NodeProperty<GameObject> player;
    public NodeProperty<float> fleeDistance;
    public NodeProperty<float> fleeSpeedMultiplier;

    private Vector3 directionVector;
    private Vector3 runAwayVector;
    private Vector3 fleeVector;

    private int attemptFlee;

    protected override void OnStart() {
        attemptFlee = 0;
        context.agent.speed *= fleeSpeedMultiplier.Value;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {

        //search monster flee vector
        if (attemptFlee < 5)
        {
            directionVector = player.Value.transform.position - context.transform.position;
            directionVector.y = 0.0f;
            runAwayVector = directionVector.normalized * -fleeDistance.Value;
            fleeVector = context.transform.position + runAwayVector;

            //monster look at player
            

            if (Vector3.Distance(player.Value.transform.position, context.transform.position) > 10.0f)
            {
                return State.Success;
            }

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
