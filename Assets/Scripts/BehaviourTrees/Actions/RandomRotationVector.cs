using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class RandomRotationVector : ActionNode
{
    public NodeProperty<Vector3> result;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        float randomY = Random.Range(0f, 360f);
        result.Value = new Vector3(0, randomY, 0);
        return State.Success;
    }
}
