using System;
using TheKiwiCoder;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class RandomRotationVector : ActionNode
{
    public NodeProperty<Vector3> result;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        var randomY = Random.Range(0f, 360f);
        result.Value = new Vector3(0, randomY, 0);
        return State.Success;
    }
}