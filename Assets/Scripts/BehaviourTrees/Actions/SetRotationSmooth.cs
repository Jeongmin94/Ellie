using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SetRotationSmooth : ActionNode
{
    public NodeProperty<float> rotationSpeed;
    public NodeProperty<Vector3> rotationValue;

    private Quaternion targetRotation;

    protected override void OnStart()
    {
        targetRotation = Quaternion.Euler(0, rotationValue.Value.y, 0);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (Quaternion.Angle(context.transform.rotation, targetRotation) < 0.01f)
        {
            return State.Success;
        }

        targetRotation = Quaternion.Euler(0, rotationValue.Value.y, 0);
        context.transform.rotation = Quaternion.Slerp(context.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed.Value);

        return State.Success;
    }
}