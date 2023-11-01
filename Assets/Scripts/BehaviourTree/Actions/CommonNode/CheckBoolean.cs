using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CheckBoolean : ActionNode
{
    public NodeProperty<bool> checkBoolean;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(checkBoolean.Value)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
