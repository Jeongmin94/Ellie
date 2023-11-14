using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class DeleteRange : ActionNode
{
    public NodeProperty<BaseRange> targetRange;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (targetRange.Value == null)
            return State.Success;

        targetRange.Value.FadeOutAndDestroy();
        targetRange.Value = null;

        return State.Success;
    }
}
