using System;
using TheKiwiCoder;

[Serializable]
public class SetBlackboardValue : ActionNode
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
            destination.CopyFrom(source);
            {
                return State.Success;
            }
        }

        return State.Failure;
    }
}