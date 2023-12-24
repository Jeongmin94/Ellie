using System.Collections;
using System.Collections.Generic;
using Monsters.Others;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CheckDetection : ActionNode
{
    public NodeProperty<GameObject> detectAI;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        DistanceDetectedAI detect = detectAI.Value.GetComponent<DistanceDetectedAI>();
        if (detect.IsDetected)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
