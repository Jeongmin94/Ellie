using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CheckBoolTrueAfterFalse : ActionNode
{
    public NodeProperty<bool> checkBool;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(checkBool.Value == true)
        {
            checkBool.Value = false;
            return State.Success;
        }
        return State.Failure;
    }
}
