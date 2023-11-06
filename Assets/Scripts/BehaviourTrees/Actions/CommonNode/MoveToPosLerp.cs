using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class MoveToPosLerp : ActionNode
{
    public NodeProperty<Vector3> targetPosition;
    public NodeProperty<Transform> targetTransform;
    public NodeProperty<float> moveSpeed;
    public NodeProperty<float> moveDistance;

    private Vector3 startPosition;
    private float t = 0.0f;

    protected override void OnStart()
    {
        startPosition = context.transform.position;

        t = 0.0f;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        Vector3 targetPos = targetTransform.Value == null ? targetPosition.Value : targetTransform.Value.position;
        Vector3 toTarget = targetPos - context.transform.position;

        if (toTarget.sqrMagnitude < 0.001f)
        {
            return State.Success;
        }

        t += Time.deltaTime * moveSpeed.Value / Vector3.Distance(startPosition, targetPos);
        t = Mathf.Clamp(t, 0f, 1f);
        Vector3 nextPosition = Vector3.Lerp(startPosition, targetPos, t);

        if ((nextPosition - startPosition).sqrMagnitude >= moveDistance.Value * moveDistance.Value)
        {
            return State.Success;
        }

        context.transform.position = nextPosition;

        return State.Success;
    }

}
