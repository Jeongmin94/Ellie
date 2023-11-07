using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SetAgentToPlayer : ActionNode
{
    public NodeProperty<Vector3> player;

    protected override void OnStart() {
        context.agent.destination = player.Value;
        context.agent.stoppingDistance = context.controller.monsterData.stopDistance;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        return State.Success;
    }
}