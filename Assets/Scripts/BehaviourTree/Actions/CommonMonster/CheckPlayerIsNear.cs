using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CheckPlayerIsNear : ActionNode
{
    public NodeProperty<float> playerDistance;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        //if(playerDistance.Value<context.agent.stoppingDistance)
        if (playerDistance.Value < context.agent.stoppingDistance * context.agent.stoppingDistance)
        {
            return State.Success;
        }

        return State.Failure;
    }
}
