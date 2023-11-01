using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
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
        BlackboardKey destination = target.key;
        BlackboardKey source = node.key;

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
