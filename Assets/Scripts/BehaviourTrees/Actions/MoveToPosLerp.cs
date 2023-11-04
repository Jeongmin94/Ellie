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
        Vector2 targetPos;
        if(targetTransform.Value == null)
        {
            targetPos = targetPosition.Value;
        }
        else
        {
            targetPos = targetTransform.Value.position;
        }


        if (Vector3.Distance(context.transform.position, targetPos) < 0.001f)
        {
            return State.Success;
        }

        t += Time.deltaTime * moveSpeed.Value / Vector3.Distance(startPosition, targetPos);
        t = Mathf.Clamp(t, 0.0f, 1.0f);
        Vector3 nextPosition = Vector3.Lerp(startPosition, targetPos, t);

        if (Vector3.Distance(startPosition, nextPosition) >= moveDistance.Value)
        {
            return State.Success;
        }

        context.transform.position = nextPosition;

        return State.Success;
    }
}
