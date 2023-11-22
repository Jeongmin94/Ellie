using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class UpdatePlayerPosition : ActionNode
{
    public NodeProperty<GameObject> player;
    public NodeProperty<Vector3> playerPos;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        playerPos.Value = player.Value.transform.position;
        return State.Success;
    }
}
