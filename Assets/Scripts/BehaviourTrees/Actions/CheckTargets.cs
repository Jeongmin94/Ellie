using System;
using TheKiwiCoder;

[Serializable]
public class CheckTargets : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}