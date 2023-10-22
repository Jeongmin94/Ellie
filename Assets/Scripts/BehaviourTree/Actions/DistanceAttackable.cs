using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class DistanceAttackable : ActionNode
{
    public NodeProperty<GameObject> player;
    public NodeProperty<float> playerDistance;
    public NodeProperty<float> minimumAttackableDistance;
    public NodeProperty<float> maximumAttackableDistance;
    public NodeProperty<float> attackInterval;

    private float usedTime;
    protected override void OnStart()
    {
        playerDistance.Value = Vector3.Distance(context.transform.position, player.Value.transform.position);
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        if (playerDistance.Value < minimumAttackableDistance.Value)
        {
            return State.Failure;
        }
        if (playerDistance.Value > maximumAttackableDistance.Value)
        {
            return State.Failure;
        }
        if(Time.time-usedTime>=attackInterval.Value)
        {
            usedTime = Time.time;
            return State.Success;
        }

        return State.Failure;
    }
}
