using System;
using TheKiwiCoder;

[Serializable]
public class BooleanInverter : ActionNode
{
    public NodeProperty<bool> boolean;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        boolean.Value = !boolean.Value;

        return State.Success;
    }
}