using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class MonsterLookPlayer : ActionNode
{
    public NodeProperty<Vector3> playerPos;

    private float accumTime;
    protected override void OnStart() {
        accumTime = 0.0f;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (accumTime < 0.5f)
        {
            Vector3 directionToTarget = (playerPos.Value - context.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));
            context.transform.rotation = Quaternion.Slerp(context.transform.rotation, lookRotation, Time.deltaTime * 5.0f);
            accumTime += Time.deltaTime;
            return State.Running;
        }
        else return State.Success;
    }
}
