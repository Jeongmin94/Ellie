using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class DashMove : ActionNode
{
    public NodeProperty<GameObject> player;
    public NodeProperty<float> dashTime;
    public NodeProperty<float> speedMultiplier;
    public NodeProperty<bool> isDashing;

    private float accumulatedTime;

    protected override void OnStart() {
        Debug.Log("Speed UP");
        context.agent.speed *= speedMultiplier.Value;
        isDashing.Value = true;
    }

    protected override void OnStop() {
        context.agent.speed /= speedMultiplier.Value;
        accumulatedTime = 0.0f;
        context.animator.SetTrigger("SkeletonIdle");
        isDashing.Value = false;
    }

    protected override State OnUpdate()
    {
        context.agent.destination = player.Value.transform.position;

        Debug.Log(accumulatedTime);

        if (accumulatedTime < dashTime.Value)
        {
            if (Vector3.Distance(player.Value.transform.position, context.transform.position) < 1.5f)
            {
                return State.Success;
            }
            accumulatedTime += Time.deltaTime;

            return State.Running;
        }
        else
        {
            return State.Success;
        }
    }
}
