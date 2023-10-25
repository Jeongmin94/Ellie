using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class ReturnToPosition : ActionNode
{
    public NodeProperty<Vector3> spawnPosition;

    protected override void OnStart() {
        context.agent.stoppingDistance = 0.0f;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (Vector3.Distance(context.transform.position, spawnPosition.Value) > 0.1f)
        {
            context.agent.destination = spawnPosition.Value;
            return State.Running;
        }
        context.agent.stoppingDistance = context.controller.monsterData.stopDistance;
            return State.Success;
    }
}
