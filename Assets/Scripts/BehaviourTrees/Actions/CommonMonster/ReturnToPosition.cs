using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class ReturnToPosition : ActionNode
{
    public NodeProperty<Vector3> spawnPosition;
    public NodeProperty<bool> isOnSapwnPosition;
    public NodeProperty<bool> isReturning;

    protected override void OnStart() {
        context.agent.stoppingDistance = 0.0f;
        context.agent.destination = spawnPosition.Value;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        float distance = Vector3.SqrMagnitude(context.transform.position - spawnPosition.Value);
        if (distance > 0.5f)
        {
            return State.Running;
        }

        context.agent.stoppingDistance = context.controller.monsterData.stopDistance;
        isReturning.Value = false;
        return State.Success;
    }
}
