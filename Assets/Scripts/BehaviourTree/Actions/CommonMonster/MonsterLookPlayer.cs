using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class MonsterLookPlayer : ActionNode
{
    public NodeProperty<GameObject> player;
    public NodeProperty<float> duration;
    public NodeProperty<float> rotationSpeed;

    private Vector3 directionVector;
    private float accumTime;
    protected override void OnStart() {
        accumTime = 0.0f;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(accumTime<duration.Value)
        {
            directionVector = player.Value.transform.position - context.transform.position;
            directionVector.y = 0.0f;
            Quaternion targetRotation = Quaternion.LookRotation(directionVector);
            context.transform.rotation = Quaternion.Slerp(context.transform.rotation, targetRotation, rotationSpeed.Value * Time.deltaTime);

            accumTime += Time.deltaTime;
            return State.Running;
        }
        return State.Success;
    }
}
