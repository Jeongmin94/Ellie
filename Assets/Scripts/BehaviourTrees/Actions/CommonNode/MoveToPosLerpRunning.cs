using System;
using TheKiwiCoder;
using UnityEngine;

[Serializable]
public class MoveToPosLerpRunning : ActionNode
{
    public NodeProperty<Vector3> targetPosition;
    public NodeProperty<float> moveSpeed;
    public NodeProperty<float> moveDistance;

    private Vector3 startPosition;
    private float t;

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
        var directionToTarget = targetPosition.Value - context.transform.position;
        var closeEnoughSqr = 0.001f;

        if (directionToTarget.sqrMagnitude < closeEnoughSqr)
        {
            return State.Success;
        }

        t += Time.deltaTime * moveSpeed.Value / Vector3.Distance(startPosition, targetPosition.Value);
        t = Mathf.Clamp(t, 0.0f, 1.0f);
        var nextPosition = Vector3.Lerp(startPosition, targetPosition.Value, t);

        if (Vector3.Distance(startPosition, nextPosition) >= moveDistance.Value)
        {
            return State.Success;
        }

        context.transform.position = nextPosition;

        return State.Running;
    }
}