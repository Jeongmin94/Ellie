using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

using Assets.Scripts.Monsters.Others;

[System.Serializable]
public class ChasePlayerDetect : ActionNode
{
    public NodeProperty<GameObject> player;

    public NodeProperty<GameObject> detectPlayerAI;
    public NodeProperty<float> detectChaseDistance;

    private DistanceDetectedAI detectPlayer;

    protected override void OnStart() {
        detectPlayer = detectPlayerAI.Value.GetComponent<DistanceDetectedAI>();
        detectPlayer.SetDetectDistance(detectChaseDistance.Value);
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(detectPlayer.IsDetected)
        {
            context.agent.destination = player.Value.transform.position;
            return State.Success;
        }

        return State.Failure;
    }
}
