using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using Assets.Scripts.Managers;

[System.Serializable]
public class PlaySound : ActionNode
{
    public NodeProperty<string> soundName;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, soundName.Value, context.transform.position);

        return State.Success;
    }
}
