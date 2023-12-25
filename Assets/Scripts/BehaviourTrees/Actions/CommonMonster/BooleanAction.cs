using System;
using TheKiwiCoder;

[Serializable]
public class BooleanAction : ActionNode
{
    public enum Boolean
    {
        TRUE,
        FALSE
    }

    public NodeProperty<bool> boolean;
    public NodeProperty<Boolean> action;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (boolean.Value)
        {
            if (action.Value == Boolean.TRUE)
            {
                return State.Success;
            }

            return State.Failure;
        }

        if (action.Value == Boolean.FALSE)
        {
            return State.Success;
        }

        return State.Failure;
    }
}