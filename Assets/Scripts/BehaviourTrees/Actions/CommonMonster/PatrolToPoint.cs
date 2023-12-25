using System;
using Monsters.Others;
using TheKiwiCoder;
using UnityEngine;

[Serializable]
public class PatrolToPoint : ActionNode
{
    public NodeProperty<GameObject> patrolPoints;
    public NodeProperty<bool> isOnSpawnPosition;
    public NodeProperty<GameObject> chaseDetectAI;

    private int count;
    private bool isPointSet;
    private Vector3[] patrolPointList;
    private DistanceDetectedAI playerDetect;

    protected override void OnStart()
    {
        isOnSpawnPosition.Value = false;
        if (!isPointSet)
        {
            patrolPointList = patrolPoints.Value.GetComponent<PatrolPoints>().GetPatrolPointst();
            isPointSet = true;
            playerDetect = chaseDetectAI.Value.GetComponent<DistanceDetectedAI>();
        }

        context.agent.stoppingDistance = 0.0f;
        context.agent.destination = patrolPointList[count];
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        //if (Vector3.Distance(context.transform.position, patrolPointList[count]) < 0.1f)
        var distance = Vector3.SqrMagnitude(context.transform.position - patrolPointList[count]);

        if (distance < 0.1f)
        {
            count++;
            if (count >= patrolPointList.Length)
            {
                count = 0;
            }

            context.agent.stoppingDistance = context.controller.monsterData.stopDistance;
            return State.Success;
        }

        if (playerDetect.IsDetected)
        {
            return State.Success;
        }

        return State.Running;
    }
}