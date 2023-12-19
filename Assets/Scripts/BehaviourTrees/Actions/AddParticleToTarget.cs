using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using Assets.Scripts.Particle;

[System.Serializable]
public class AddParticleToTarget : ActionNode
{
    public NodeProperty<GameObject> effectPrefab;
    public NodeProperty<Transform> origin;
    public NodeProperty<Vector3> scale;
    public NodeProperty<Vector3> offset;

    public NodeProperty<bool> isLoop;
    public NodeProperty<bool> isFollowOrigin;
    public NodeProperty<int> loopCount;

    protected override void OnStart()
    {
        if (origin.Value == null)
            origin.Value = context.transform;

        if (scale.Value == Vector3.zero)
            scale.Value = Vector3.one;

        if (loopCount.Value <= 0)
            loopCount.Value = 1;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (effectPrefab == null)
            return State.Failure;

        ParticleManager.Instance.GetParticle(effectPrefab.Value, new ParticlePayload
        {
            Origin = origin.Value,
            IsLoop = isLoop.Value,
            IsFollowOrigin = isFollowOrigin.Value,
            Position = origin.Value.transform.position,
            Rotation = origin.Value.transform.rotation,
            Offset = offset.Value,
            Scale = scale.Value,
            LoopCount = loopCount.Value,
        });

        return State.Success;
    }
}
