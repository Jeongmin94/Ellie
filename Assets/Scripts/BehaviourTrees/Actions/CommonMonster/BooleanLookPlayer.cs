using System;
using TheKiwiCoder;

[Serializable]
public class BooleanLookPlayer : ActionNode
{
    protected override void OnStart()
    {
        //delete
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}