using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class MoveTowardsRunning : ActionNode
{
    public NodeProperty<Vector3> targetPosition;
    public NodeProperty<Transform> targetTransform;
    public NodeProperty<float> moveSpeed;
    public NodeProperty<float> moveDistance;

    private float travelledDistance;
    
    protected override void OnStart()
    {
        travelledDistance = 0.0f;
    }

    protected override void OnStop()
    {
    }
    protected override State OnUpdate()
    {
        // Vector3 사용
        if (targetTransform.Value == null)
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

            context.transform.position = nextPosition; 
        }
        // Transform 사용
        else
        {
            Vector3 currentPosition = context.transform.position;
            if (Vector3.Distance(currentPosition, targetTransform.Value.position) < 0.001f)
            {
                return State.Success;
            }

            Vector3 nextPosition = Vector3.MoveTowards(context.transform.position, targetTransform.Value.position, moveSpeed.Value * Time.deltaTime);
            travelledDistance += Vector3.Distance(currentPosition, nextPosition);

            if (travelledDistance >= moveDistance.Value)
            {
                return State.Success;
            }

            context.transform.position = nextPosition;
        }

        // 목표 안넘어가면 유지
        return State.Running;
    }
}
