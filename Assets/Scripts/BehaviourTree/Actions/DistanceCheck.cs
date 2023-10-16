using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class DistanceCheck : ActionNode
{
    public NodeProperty<Vector3> startPoint;
    public NodeProperty<Vector3> endPoint;

    public NodeProperty<GameObject> gameObject;

    public float checkDistance = 10.0f;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(Vector3.Distance(startPoint.Value, endPoint.Value) < checkDistance)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
