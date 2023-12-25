using System;
using TheKiwiCoder;

[Serializable]
public class CheckAnimationTransitionEnd : ActionNode
{
    private bool wasInTransition;

    protected override void OnStart()
    {
        wasInTransition = false;
    }

    protected override void OnStop()
    {
        wasInTransition = false;
    }

    protected override State OnUpdate()
    {
        var isInTransition = context.animator.IsInTransition(0);

        if (!isInTransition && wasInTransition)
        {
            return State.Success;
        }

        wasInTransition = isInTransition;
        return State.Running;
    }
}