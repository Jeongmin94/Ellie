using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CheckRangeTargets : ActionNode
{
    public NodeProperty<BaseRange> range;
    public NodeProperty<List<Transform>> targets;

    public NodeProperty<string> checkTag;
    public NodeProperty<LayerMask> checkLayer;

    protected override void OnStart() {
        if (checkLayer.Value == 0)
            checkLayer.Value = -1;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(range == null)
        {
            return State.Failure;
        }

        targets.Value = range.Value.CheckRange(checkTag.Value, checkLayer.Value);
        foreach (var item in targets.Value)
        {
            Debug.Log(item);
        }

        return State.Success;
    }
}
