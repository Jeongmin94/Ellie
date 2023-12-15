using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class FreezeEffect : ActionNode
{
    public NodeProperty<bool> isFreezed;

    protected override void OnStart() {
        context.animator.speed = 0.0f;
        context.controller.freezeEffect.SetActive(true);
        context.controller.characterMesh.AddFreezeRenderer();
    }

    protected override void OnStop() {
        context.animator.speed = 1.0f;
        context.controller.freezeEffect.SetActive(false);
        context.controller.characterMesh.DeleteFreezeRenderer();
    }

    protected override State OnUpdate() {
        if (isFreezed.Value) return State.Running;

        return State.Success;
    }
}
