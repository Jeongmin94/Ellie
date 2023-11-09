using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SetLayerWeight : ActionNode
{
    public NodeProperty<int> layerIndex;
    public NodeProperty<float> layerWeight;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        context.animator.SetLayerWeight(layerIndex.Value, layerWeight.Value);
        return State.Success;
    }
}
