using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class ConvertTransformToPosition : ActionNode
{
    public NodeProperty<Transform> startTransform;
    public NodeProperty<Vector3> resultPosition;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        Debug.Log(startTransform.Value);
        resultPosition.Value = startTransform.Value.position;
        Debug.Log(resultPosition.Value);

        return State.Success;
    }
}
