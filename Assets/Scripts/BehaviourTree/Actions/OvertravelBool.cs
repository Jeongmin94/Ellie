using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class OvertravelBool : ActionNode
{
    public NodeProperty<Vector3> returnPosition;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(Vector3.Distance(returnPosition.Value, context.transform.position)
            >context.controller.monsterData.overtravelDistance)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
