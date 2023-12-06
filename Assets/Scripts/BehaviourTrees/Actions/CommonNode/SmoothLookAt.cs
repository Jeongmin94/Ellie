using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SmoothLookAt : ActionNode
{
    public NodeProperty<Transform> targetTransform;
    public NodeProperty<Vector3> targetPosition;
    public NodeProperty<float> rotationSpeed;

    public bool reverse;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        Vector3 direction;

        if (targetTransform.Value == null)
        {
            direction = targetPosition.Value - context.transform.position;
        }
        else
        {
            direction = targetTransform.Value.position - context.transform.position;
        }

        direction.y = 0;

        if (direction.sqrMagnitude < 0.01f)
        {
            return State.Success;
        }

        // 반대 방향 바라보기
        if(reverse)
        {
            direction *= -1;
        }

        Quaternion rotation = Quaternion.LookRotation(direction);

        context.transform.rotation = Quaternion.Slerp(context.transform.rotation, rotation, rotationSpeed.Value * Time.deltaTime);

        return State.Success;
    }
}
