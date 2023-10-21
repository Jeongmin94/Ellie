using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using Assets.Scripts.Monsters.Others;

[System.Serializable]
public class MonsterAction : ActionNode
{
    public NodeProperty<GameObject> detectPlayerAI;
    public NodeProperty<Vector3> spawnPosition;

    public NodeProperty<float> detectPlayerDistance;
    public NodeProperty<float> overtavelDistance;
    public NodeProperty<bool> isReturning;

    private DistanceDetectedAI detectPlayer;

    
    protected override void OnStart() {
        detectPlayer = detectPlayerAI.Value.GetComponent<DistanceDetectedAI>();
        detectPlayer.SetDetectDistance(detectPlayerDistance.Value);
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        //Animator
        if(context.agent.velocity.magnitude<0.01f)
        {
            context.animator.SetTrigger("SkeletonIdle");
        }
        else if(context.agent.velocity.magnitude>0.01f)
        {
            context.animator.SetTrigger("SkeletonWalk");
        }

        if(isReturning.Value)
        {
            if (Vector3.Distance(context.transform.position, spawnPosition.Value) < 1.0f)
            {
                isReturning.Value = false;
                return State.Failure;
            }

            return State.Running;
        }
        if (Vector3.Distance(context.transform.position, spawnPosition.Value)>overtavelDistance.Value)
        {
            context.agent.destination = spawnPosition.Value;
            isReturning.Value = true;
            return State.Failure;
        }
        if (detectPlayer.IsDetected)
        {
            //context.animator.SetTrigger("SkeletonWalk");
            return State.Success;
        }

        //context.animator.SetTrigger("SkeletonIdle");
        return State.Failure;

    }
}
