using TheKiwiCoder;

[System.Serializable]
public class CheckAnimationTransitionEnd : ActionNode
{
    bool wasInTransition = false;
    protected override void OnStart() {
        wasInTransition = false;
    }

    protected override void OnStop() {
        wasInTransition = false;
    }

    protected override State OnUpdate() {
        bool isInTransition = context.animator.IsInTransition(0);

        if (!isInTransition && wasInTransition)
        {
            return State.Success;
        }

        wasInTransition = isInTransition;
        return State.Running;
    }
}
