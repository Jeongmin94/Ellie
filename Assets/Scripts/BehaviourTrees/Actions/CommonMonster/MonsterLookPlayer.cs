using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class MonsterLookPlayer : ActionNode
{
    public NodeProperty<Vector3> playerPos;

    private Quaternion targetRotation;
    private float accumTime;
    protected override void OnStart() {
        accumTime = 0.0f;
        Vector3 directionVector = playerPos.Value - context.transform.position;
        directionVector.y = 0;
        directionVector.Normalize();
        targetRotation = Quaternion.LookRotation(directionVector,Vector3.up);
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (accumTime < 0.5f)
        {
            context.transform.rotation = Quaternion.Slerp(context.transform.rotation, targetRotation, Time.deltaTime * 90.0f);
            accumTime += Time.deltaTime;
            return State.Running;
        }
        else return State.Success;
    }
}
