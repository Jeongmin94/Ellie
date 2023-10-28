using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class LookAtPosition : ActionNode
{
    public NodeProperty<Vector3> targetPosition;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        context.transform.LookAt(targetPosition.Value);
        
        return State.Success;
    }
}
