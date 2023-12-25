using System;
using TheKiwiCoder;
using UnityEngine;

[Serializable]
public class MoveDown : ActionNode
{
    public NodeProperty<float> speed;
    public NodeProperty<float> length;
    public NodeProperty<float> duration;
    public NodeProperty<float> offsetValue;

    private float accumTime;

    protected override void OnStart()
    {
        offsetValue.Value = context.agent.baseOffset;
        accumTime = 0.0f;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        while (accumTime < duration.Value)
        {
            if (offsetValue.Value - context.agent.baseOffset < length.Value)
            {
                context.agent.baseOffset -= speed.Value * Time.deltaTime;
            }

            accumTime += Time.deltaTime;
            return State.Running;
        }

        return State.Success;
    }
}