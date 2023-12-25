using System;
using TheKiwiCoder;

[Serializable]
public class CheckAnimationName : ActionNode
{
    public NodeProperty<int> layerIndex;
    public NodeProperty<string> animationName;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        var check = animationName.Value;

        if (context.animator.GetCurrentAnimatorStateInfo(layerIndex.Value).IsName(check))
        {
            return State.Success;
        }

        return State.Failure;
    }
}