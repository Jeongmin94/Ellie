using System;
using TheKiwiCoder;
using UnityEngine;

[Serializable]
public class MonsterLookPlayer : ActionNode
{
    public NodeProperty<Vector3> playerPos;
    private float accumTime;

    private Quaternion targetRotation;

    protected override void OnStart()
    {
        accumTime = 0.0f;
        var directionVector = playerPos.Value - context.transform.position;
        directionVector.y = 0;
        directionVector.Normalize();
        targetRotation = Quaternion.LookRotation(directionVector, Vector3.up);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (accumTime < 0.5f)
        {
            context.transform.rotation = Quaternion.Slerp(context.transform.rotation, targetRotation, Time.deltaTime * 90.0f);
            accumTime += Time.deltaTime;
            return State.Running;
        }

        return State.Success;
    }
}