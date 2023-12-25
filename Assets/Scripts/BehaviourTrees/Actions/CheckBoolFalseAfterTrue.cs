using System;
using TheKiwiCoder;

[Serializable]
public class CheckBoolFalseAfterTrue : ActionNode
{
    public NodeProperty<bool> checkBool;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (checkBool.Value == false)
        {
            checkBool.Value = true;
            return State.Success;
        }

        return State.Failure;
    }
}