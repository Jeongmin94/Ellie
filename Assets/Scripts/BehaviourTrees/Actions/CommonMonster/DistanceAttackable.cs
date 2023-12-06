using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class DistanceAttackable : ActionNode
{
    public NodeProperty<float> playerDistance;
    public NodeProperty<float> minimumAttackableDistance;
    public NodeProperty<float> maximumAttackableDistance;
    public NodeProperty<float> attackInterval;

    private float usedTime;
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (playerDistance.Value < minimumAttackableDistance.Value * minimumAttackableDistance.Value)
        {
            return State.Failure;
        }
        if (maximumAttackableDistance.Value == 0)
        {
            maximumAttackableDistance.Value = context.controller.monsterData.chasePlayerDistance;
        }
        if (playerDistance.Value > maximumAttackableDistance.Value * maximumAttackableDistance.Value)
        {
            return State.Failure;
        }

        if (Time.time - usedTime < attackInterval.Value)
        {
            return State.Failure;
        }
        usedTime = Time.time;

        return State.Success;
    }
}

