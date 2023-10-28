using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class RunToPlayer : ActionNode
{
    public NodeProperty<GameObject> player;
    public NodeProperty<float> playerDistance;

    private float accumTime;
    protected override void OnStart() {
        context.agent.speed = context.controller.runToPlayerData.movementSpeed;
        accumTime = 0.0f;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        context.agent.destination = player.Value.transform.position;
        if(accumTime<context.controller.runToPlayerData.attackDuration)
        {
            if(Vector3.Distance(context.transform.position,context.agent.destination)<context.agent.stoppingDistance)
            {
                Debug.Log("NearPlayer");
                return State.Success;
            }
            accumTime += Time.deltaTime;
            return State.Running;
        }

        context.agent.speed = context.controller.monsterData.movementSpeed;
        return State.Success;
    }
}
