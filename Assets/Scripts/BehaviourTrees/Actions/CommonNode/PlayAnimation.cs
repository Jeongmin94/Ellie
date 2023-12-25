using System;
using TheKiwiCoder;

[Serializable]
public class PlayAnimation : ActionNode
{
    public NodeProperty<string> animationName;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        context.animator.Play(animationName.Value);

        return State.Success;
    }
}