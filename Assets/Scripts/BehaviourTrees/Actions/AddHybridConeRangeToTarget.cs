using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class AddHybridConeRangeToTarget : ActionNode
{
    public NodeProperty<BaseRange> targetRange;
    public NodeProperty<bool> isShowRange;

    public NodeProperty<Transform> origin;
    public NodeProperty<bool> isFollowOrigin;

    public NodeProperty<float> remainTime;

    public NodeProperty<float> radius;
    public NodeProperty<float> angle;
    public NodeProperty<float> upperBase;

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
            Type = RangeType.HybridCone,
            IsShowRange = isShowRange.Value,
            Original = origin.Value,
            IsFollowOrigin = isFollowOrigin.Value,
            RemainTime = remainTime.Value,
            Radius = radius.Value,
            Angle = angle.Value,
            UpperBase = upperBase.Value,
        }).GetComponent<BaseRange>();
        return State.Success;
    }
}
