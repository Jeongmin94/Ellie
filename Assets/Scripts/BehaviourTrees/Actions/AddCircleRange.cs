using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class AddCircleRange : ActionNode
{
    public NodeProperty<BaseRange> targetRange;
    public NodeProperty<bool> isShowRange;

    public NodeProperty<Vector3> startPosition;
    public NodeProperty<Quaternion> startRotation;

    public NodeProperty<float> remainTime;

    public NodeProperty<float> radius;


    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        targetRange.Value = RangeManager.Instance.CreateRange(new RangePayload
        {
            Type = RangeType.Circle,
            IsShowRange = isShowRange.Value,
            StartPosition = startPosition.Value,
            StartRotation = startRotation.Value,
            RemainTime = remainTime.Value,
            Radius = radius.Value,
        }).GetComponent<BaseRange>();
        return State.Success;
    }
}
