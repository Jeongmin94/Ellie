using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

using Assets.Scripts.Monsters.Others;

[System.Serializable]
public class WaitAndDetect : ActionNode
{
    public NodeProperty<float> duration;
    public NodeProperty<GameObject> chaseDetectAI;

    private float accumTime;
    private DistanceDetectedAI detectAI;
    protected override void OnStart() {
        accumTime = 0.0f;
        detectAI = chaseDetectAI.Value.GetComponent<DistanceDetectedAI>();
    }

    protected override void OnStop() {
        accumTime = 0.0f;
    }

    protected override State OnUpdate() {

        if (accumTime > duration.Value)
            return State.Success;
        if (detectAI.IsDetected)
            return State.Success;

        accumTime += Time.deltaTime;
        return State.Running;
    }
}
