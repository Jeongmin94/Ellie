using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class AddTrapezoidRange : ActionNode
{
    public NodeProperty<BaseRange> targetRange;
    public NodeProperty<bool> isShowRange;

    public NodeProperty<Vector3> startPosition;
    public NodeProperty<Quaternion> startRotation;

    public NodeProperty<float> remainTime;

    public NodeProperty<float> height;
    public NodeProperty<float> upperBase;
    public NodeProperty<float> lowerBase;

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
            Type = RangeType.Trapezoid,
            IsShowRange = isShowRange.Value,
            StartPosition = startPosition.Value,
            StartRotation = startRotation.Value,
            RemainTime = remainTime.Value,
            Height = height.Value,
            UpperBase = upperBase.Value,
            LowerBase = lowerBase.Value,
        }).GetComponent<BaseRange>();
        return State.Success;
    }
}
