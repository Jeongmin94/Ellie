using System;
using TheKiwiCoder;

[Serializable]
public class CheckBoolTrueAfterFalse : ActionNode
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
        if (checkBool.Value)
        {
            checkBool.Value = false;
            return State.Success;
        }

        return State.Failure;
    }
}