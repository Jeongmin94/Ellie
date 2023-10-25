using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

using Assets.Scripts.Monsters.Others;

[System.Serializable]
public class SetData : ActionNode
{
    public NodeProperty<GameObject> player;
    public NodeProperty<float> playerDistance;
    public NodeProperty<Vector3> playerPosition;
    public NodeProperty<GameObject> detectChaseAI;

    protected override void OnStart() {
        playerDistance.Value = Vector3.Distance(player.Value.transform.position, context.transform.position);
        DistanceDetectedAI detectAI = detectChaseAI.Value.GetComponent<DistanceDetectedAI>();
        playerPosition.Value = player.Value.transform.position;
        if (detectAI.IsDetected)
            context.agent.stoppingDistance = 5.0f;
        else context.agent.stoppingDistance = 0.5f;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
