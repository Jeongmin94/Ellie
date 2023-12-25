using System;
using TheKiwiCoder;

[Serializable]
public class AddFloatAcceleration : ActionNode
{
    public NodeProperty<float> floatValue;
    public NodeProperty<float> accelerationValue;
    public NodeProperty<float> goalValue;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        floatValue.Value += accelerationValue.Value;

        if (floatValue.Value > goalValue.Value)
        {
            floatValue.Value = goalValue.Value;
            return State.Success;
        }

        return State.Running;
    }
}