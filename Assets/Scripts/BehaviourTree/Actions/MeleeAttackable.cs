using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class MeleeAttackable : ActionNode
{
    public NodeProperty<GameObject> player;
    public NodeProperty<float> playerDistance;

    private float usedTime;
    protected override void OnStart() {
        playerDistance.Value = Vector3.Distance(context.transform.position, player.Value.transform.position);
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(playerDistance.Value>context.controller.meleeAttackData.attackableDistance)
        {
            return State.Failure;
        }
        if(Time.time-usedTime<=context.controller.meleeAttackData.attackInterval)
        {
            return State.Failure;
        }

        return State.Success;
    }
}
