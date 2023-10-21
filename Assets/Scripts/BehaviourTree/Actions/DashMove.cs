using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class DashMove : ActionNode
{
    public NodeProperty<GameObject> player;
    public NodeProperty<float> dashDistance;
    public NodeProperty<float> dashTime;
    public NodeProperty<float> speedMultiplier;

    private float dotProduct;
    private float accumulateTime;
    private bool startAnimation;

    protected override void OnStart() {
        context.agent.speed *= speedMultiplier.Value;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        if (accumulateTime < dashTime.Value)
        {            
            context.agent.destination = player.Value.transform.position;
            accumulateTime += Time.deltaTime;

            if (Vector3.Distance(player.Value.transform.position, context.transform.position) < 0.1f)
                return State.Success;

            return State.Running;
        }
        else
        {
            context.agent.speed /= speedMultiplier.Value;
            accumulateTime = 0.0f;
            return State.Success;
        }
    }
}
