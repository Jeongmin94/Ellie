using System;
using TheKiwiCoder;

[Serializable]
public class CheckBoolean : ActionNode
{
    public NodeProperty<bool> checkBoolean;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (checkBoolean.Value)
        {
            return State.Success;
        }

        return State.Failure;
    }
}