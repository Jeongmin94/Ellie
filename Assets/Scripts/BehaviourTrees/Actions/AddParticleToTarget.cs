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

    public NodeProperty<bool> isLoop;
    public NodeProperty<bool> isFollowOrigin;

    protected override void OnStart()
    {
        if (origin.Value == null)
            origin.Value = context.transform;

        if (scale.Value == Vector3.zero)
            scale.Value = Vector3.one;
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
            Scale = scale.Value,
        });

        return State.Success;
    }
}
