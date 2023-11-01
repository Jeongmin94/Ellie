using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class TriggerAnimation : ActionNode
{
    public NodeProperty<string> animationName;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        context.animator.SetTrigger(animationName.Value);

        return State.Success;
    }
}
