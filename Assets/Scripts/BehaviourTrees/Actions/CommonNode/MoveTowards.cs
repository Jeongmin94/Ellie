using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class MoveTowards : ActionNode
{
    public NodeProperty<Vector3> targetPosition;
    public NodeProperty<Transform> targetTransform;
    public NodeProperty<float> moveSpeed;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        // Vector3로 이동
        if (targetTransform.Value == null)
        {
            if (Vector3.Distance(context.transform.position, targetPosition.Value) < 0.001f)
            {
                return State.Success;
            }

            context.transform.position = Vector3.MoveTowards(context.transform.position, targetPosition.Value, moveSpeed.Value * Time.deltaTime); 
        }
        // Transform으로 이동
        else
        {
            if (Vector3.Distance(context.transform.position, targetTransform.Value.position) < 0.001f)
            {
                return State.Success;
            }

            context.transform.position = Vector3.MoveTowards(context.transform.position, targetTransform.Value.position, moveSpeed.Value * Time.deltaTime);
        }
        return State.Success;
    }
}
