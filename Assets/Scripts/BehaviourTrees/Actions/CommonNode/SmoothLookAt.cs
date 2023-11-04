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

        if (direction.sqrMagnitude < 0.001f)
        {
            return State.Success;
        }

        Quaternion rotation = Quaternion.LookRotation(direction);

        context.transform.rotation = Quaternion.Slerp(context.transform.rotation, rotation, rotationSpeed.Value * Time.deltaTime);

        return State.Success;
    }
}
