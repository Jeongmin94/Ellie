using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using Assets.Scripts.Monsters.Others;

[System.Serializable]
public class Patrol : ActionNode
{
    public NodeProperty<float> detectPlayerDistance = new();

    public NodeProperty<float> patrolInterval = new() { defaultValue = 3.0f };
    public NodeProperty<float> patrolRadius = new() { defaultValue = 5.0f };
    public NodeProperty<bool> isReturning;
    public NodeProperty<Vector3> spawnPosition;

    public NodeProperty<GameObject> detectPlayerAI;
    public NodeProperty<GameObject> detectChaseAI;

    private float accumulateTime;
    private DistanceDetectedAI detectPlayer;
    private DistanceDetectedAI detectChase;

    protected override void OnStart()
    {
        Vector3 randomPosition = Random.insideUnitSphere*patrolRadius.Value+spawnPosition.Value;
        context.agent.destination = randomPosition;

        detectPlayer = detectPlayerAI.Value.GetComponent<DistanceDetectedAI>();
        detectChase = detectChaseAI.Value.GetComponent<DistanceDetectedAI>();

        accumulateTime = 0.0f;


    }

    protected override void OnStop() { 
    }

    protected override State OnUpdate() {
        if (isReturning.Value)
            return State.Failure;

        if (!detectPlayer.IsDetected)
        {
            context.agent.destination = spawnPosition.Value;
            return State.Failure;
        }
        else
        {
            if (detectChase.IsDetected)
            {
                return State.Success;
            }
            if(accumulateTime>patrolInterval.Value)
            {
                return State.Success;
            }
            accumulateTime += Time.deltaTime;
        }
        return State.Running;
    }
}
