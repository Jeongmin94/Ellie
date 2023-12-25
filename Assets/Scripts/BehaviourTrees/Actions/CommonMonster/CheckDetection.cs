using System;
using Monsters.Others;
using TheKiwiCoder;
using UnityEngine;

[Serializable]
public class CheckDetection : ActionNode
{
    public NodeProperty<GameObject> detectAI;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        var detect = detectAI.Value.GetComponent<DistanceDetectedAI>();
        if (detect.IsDetected)
        {
            return State.Success;
        }

        return State.Failure;
    }
}