using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Jump : ActionNode
{
    public NodeProperty<float> jumpPower;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        context.physics.AddForce(Vector3.up * jumpPower.Value, ForceMode.Impulse);
        return State.Success;
    }
}
