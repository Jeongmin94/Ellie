using System;
using TheKiwiCoder;

[Serializable]
public class CompareBlackboardValue : ActionNode
{
    public BlackboardKeyValuePair target;
    public BlackboardKeyValuePair node;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        var destination = target.key;
        var source = node.key;

        if (source != null && destination != null)
        {
            if (destination.Equals(source))
            {
                return State.Success;
            }
        }

        return State.Failure;
    }
}