using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class PlayParticle : ActionNode
{
    public NodeProperty<MonsterParticleType> particleType;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (context.particleController.PlayParticle(particleType.Value))
            return State.Success;

        return State.Failure;
    }
}
