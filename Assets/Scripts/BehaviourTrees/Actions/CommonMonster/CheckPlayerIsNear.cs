using System;
using TheKiwiCoder;

[Serializable]
public class CheckPlayerIsNear : ActionNode
{
    public NodeProperty<float> playerDistance;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (playerDistance.Value < 4.0f)
        {
            return State.Success;
        }

        return State.Failure;
    }
}