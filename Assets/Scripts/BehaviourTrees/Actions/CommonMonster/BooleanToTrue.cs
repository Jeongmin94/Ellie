using System;
using TheKiwiCoder;

[Serializable]
public class BooleanToTrue : ActionNode
{
    public NodeProperty<bool> boolean;

    protected override void OnStart()
    {
        boolean.Value = true;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}