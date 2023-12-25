using System;
using TheKiwiCoder;

[Serializable]
public class IsDead : ActionNode
{
    //delete
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Failure;
    }
}