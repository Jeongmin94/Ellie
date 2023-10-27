using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using Codice.CM.Common;
using log4net.Util;

[System.Serializable]
public class MoveTowards : ActionNode
{
    public NodeProperty<Vector3> targetPosition;
    public NodeProperty<float> moveSpeed;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }
    protected override State OnUpdate() {
        if (Vector3.Distance(context.transform.position, targetPosition.Value) < 0.001f)
        {
            return State.Success;
        }

        context.transform.position = Vector3.MoveTowards(context.transform.position, targetPosition.Value, moveSpeed.Value * Time.deltaTime);
        return State.Success;
    }
}
