using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CheckAnimationName : ActionNode
{
    public NodeProperty<string> animationName;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        string check = animationName.Value;

        if(context.animator.GetCurrentAnimatorStateInfo(0).IsName(check))
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
