using System;
using TheKiwiCoder;

[Serializable]
public class StopAnimation : ActionNode
{
    protected override void OnStart()
    {
        context.animator.speed = 0.0f;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}