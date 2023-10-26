using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class ReturnToPosition : ActionNode
{
    public NodeProperty<Vector3> spawnPosition;
    public NodeProperty<bool> isOnSapwnPosition;

    protected override void OnStart() {
        context.agent.stoppingDistance = 0.0f;
        context.agent.destination = spawnPosition.Value;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        if (Vector3.Distance(context.transform.position, spawnPosition.Value) > 0.5f)
        {
            isOnSapwnPosition.Value = false;
            return State.Running;
        }
        context.agent.stoppingDistance = context.controller.monsterData.stopDistance;
        isOnSapwnPosition.Value = true;
        return State.Success;
    }
}
