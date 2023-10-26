using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Wait1Frame : ActionNode
{
    bool isBreak;

    protected override void OnStart() {
        isBreak = true;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(isBreak)
        {
            isBreak = false;
            return State.Running;
        }

        return State.Success;
    }
}
