using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using Assets.Scripts.Monsters.Others;

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
