using System;
using TheKiwiCoder;

[Serializable]
public class ShowRenderer : ActionNode
{
    public NodeProperty<bool> isVisible;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (isVisible.Value)
        {
            context.controller.renderer.enabled = true;
        }
        else
        {
            context.controller.renderer.enabled = false;
        }

        return State.Success;
    }
}