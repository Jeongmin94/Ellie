using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SetAnimationBool : ActionNode
{
    public NodeProperty<bool> animationBool;
    public NodeProperty<string> animationName;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if(animationBool.Value == true)
        {
            context.animator.SetBool(animationName.Value, true);
        }
        else
        {
            context.animator.SetBool(animationName.Value, false);
        }

        return State.Success;
    }
}
