using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class PatrolToPoint : ActionNode
{
    public NodeProperty<GameObject> patrolPoints;
    public NodeProperty<bool> isOnSpawnPosition;

    private int count = 0;
    private Vector3[] patrolPointList;
    private bool isPointSet = false;
    protected override void OnStart()
    {
        isOnSpawnPosition.Value = false;
        if (!isPointSet)
        {
            patrolPointList = patrolPoints.Value.GetComponent<PatrolPoints>().GetPatrolPointst();
            isPointSet = true;
        }
        context.agent.stoppingDistance = 0.0f;
        context.agent.destination = patrolPointList[count];

    }

    protected override void OnStop() {

    }

    protected override State OnUpdate() {
        if (Vector3.Distance(context.transform.position, patrolPointList[count]) < 0.1f)
        {
            count++;
            if (count >= patrolPointList.Length)
            {
                count = 0;
            }
            context.agent.stoppingDistance = context.controller.monsterData.stopDistance;
            return State.Success;
        }
        else return State.Running;
        
    }
}
