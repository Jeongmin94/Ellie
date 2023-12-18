using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using Assets.Scripts.Particle;

[System.Serializable]
public class AddParticle : ActionNode
{
    public NodeProperty<GameObject> effectPrefab;
    public NodeProperty<Vector3> pos;
    public NodeProperty<Quaternion> rot;
    public NodeProperty<Vector3> scale;
    public NodeProperty<Vector3> offset;

    public NodeProperty<bool> isLoop;
    public NodeProperty<int> loopCount;

    protected override void OnStart()
    {
        if(scale.Value == Vector3.zero)
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
            Position = pos.Value,
            Rotation = rot.Value,
            IsLoop = isLoop.Value,
            Scale = scale.Value,
            Offset = offset.Value,
            LoopCount = loopCount.Value,
        });

        return State.Success;
    }
}
