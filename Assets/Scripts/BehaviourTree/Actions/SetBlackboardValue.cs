using System;
using System.Diagnostics;
using TheKiwiCoder;
using UnityEngine;
using Debug = UnityEngine.Debug;

[System.Serializable]
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
        BlackboardKey destination = target.key;
        BlackboardKey source = node.key;

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
