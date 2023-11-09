using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class RunToPlayer : ActionNode
{
    public NodeProperty<GameObject> player;

    private float accumTime;
    protected override void OnStart()
    {
        context.agent.speed = context.controller.runToPlayerData.movementSpeed;
        accumTime = 0.0f;
    }

    protected override void OnStop()
    {
        context.agent.speed = context.controller.monsterData.movementSpeed;
    }

    protected override State OnUpdate()
    {
        context.agent.destination = player.Value.transform.position;
        float remainingDistance = Vector3.SqrMagnitude(context.agent.destination - context.transform.position);
        if (accumTime < context.controller.runToPlayerData.attackDuration)
        {
            Debug.Log("destination : " + context.agent.destination);
            Debug.Log("Remain : "+remainingDistance);
            Debug.Log("Stop : " + context.agent.stoppingDistance * context.agent.stoppingDistance);
            if (remainingDistance < context.agent.stoppingDistance * context.agent.stoppingDistance)
            {
                return State.Success;
            }
            accumTime += Time.deltaTime;
            Debug.Log("Running");
            return State.Running;
        }
        Debug.Log("FinishRun");
        return State.Success;
    }
}
