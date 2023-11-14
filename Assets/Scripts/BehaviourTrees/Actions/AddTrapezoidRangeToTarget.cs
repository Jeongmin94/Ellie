using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class AddTrapezoidRangeToTarget : ActionNode
{
    public NodeProperty<BaseRange> targetRange;
    public NodeProperty<bool> isShowRange;

    public NodeProperty<Transform> origin;
    public NodeProperty<bool> isFollowOrigin;

    public NodeProperty<float> remainTime;

    public NodeProperty<float> height;
    public NodeProperty<float> upperBase;
    public NodeProperty<float> lowerBase;

    protected override void OnStart()
    {
        if (origin.Value == null)
            origin.Value = context.transform;
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
            Original = origin.Value,
            IsFollowOrigin = isFollowOrigin.Value,
            RemainTime = remainTime.Value,
            Height = height.Value,
            UpperBase = upperBase.Value,
            LowerBase = lowerBase.Value,
        }).GetComponent<BaseRange>();
        return State.Success;
    }
}
