using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class AddRectangleRange : ActionNode
{
    public NodeProperty<BaseRange> targetRange;
    public NodeProperty<bool> isShowRange;

    public NodeProperty<Vector3> startPosition;
    public NodeProperty<Quaternion> startRotation;

    public NodeProperty<float> remainTime;

    public NodeProperty<float> height;
    public NodeProperty<float> width;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        targetRange.Value = RangeManager.Instance.CreateRange(new RangePayload
        {
            Type = RangeType.Rectangle,
            IsShowRange = isShowRange.Value,
            StartPosition = startPosition.Value,
            StartRotation = startRotation.Value,
            RemainTime = remainTime.Value,
            Height = height.Value,
            Width = width.Value,
        }).GetComponent<BaseRange>();
        return State.Success;
    }
}
