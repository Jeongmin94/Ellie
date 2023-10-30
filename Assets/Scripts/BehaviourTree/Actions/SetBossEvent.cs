using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using Assets.Scripts.Boss;
using Assets.Scripts.Boss.Terrapupa;

[System.Serializable]
public class SetBossEvent : ActionNode
{
    public NodeProperty<IBaseEventPayload> bossPayload;

    public NodeProperty<int> intValue;
    public NodeProperty<Vector3> vector3Value;
    public NodeProperty<Transform> transformValue1;
    public NodeProperty<Transform> transformValue2;
    public NodeProperty<TerrapupaAttackType> attackTypeValue;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        BossEventPayload payload = bossPayload.Value as BossEventPayload;
        if(payload == null)
        {
            return State.Failure;
        }

        payload.IntValue = intValue.Value;
        payload.Vector3Value = vector3Value.Value;
        payload.TransformValue1 = transformValue1.Value;
        payload.TransformValue2 = transformValue2.Value;
        payload.AttackTypeValue = attackTypeValue.Value;

        bossPayload.Value = payload;

        return State.Success;
    }
}
