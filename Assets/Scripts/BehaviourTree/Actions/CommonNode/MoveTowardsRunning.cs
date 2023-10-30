using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class MoveTowardsRunning : ActionNode
{
    public NodeProperty<Vector3> targetPosition;
    public NodeProperty<float> moveSpeed;
    public NodeProperty<float> moveDistance;

    private Vector3 startPosition;
    private float travelledDistance;
    
    protected override void OnStart()
    {
        startPosition = context.transform.position;
        travelledDistance = 0.0f;
    }

    protected override void OnStop()
    {
    }
    protected override State OnUpdate()
    {
        Vector3 currentPosition = context.transform.position;
        if (Vector3.Distance(currentPosition, targetPosition.Value) < 0.001f)
        {
            return State.Success;
        }

        Vector3 nextPosition = Vector3.MoveTowards(context.transform.position, targetPosition.Value, moveSpeed.Value * Time.deltaTime);
        travelledDistance += Vector3.Distance(currentPosition, nextPosition);

        if (travelledDistance >= moveDistance.Value)
        {
            return State.Success;
        }

        // 이동
        context.transform.position = nextPosition;

        // 목표 안넘어가면 유지
        return State.Running;
    }
}
