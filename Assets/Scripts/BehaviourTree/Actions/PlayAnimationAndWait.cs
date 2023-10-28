using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class PlayAnimationAndWait : ActionNode
{
    public NodeProperty<string> animationTrigger;
    public NodeProperty<float> wait;

    private float accumTime;

    protected override void OnStart()
    {
        context.animator.SetTrigger(animationTrigger.Value);
        accumTime = 0.0f;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (accumTime < wait.Value)
        {
            accumTime += Time.deltaTime;
            return State.Running;
        }

        return State.Success;
    }
}
