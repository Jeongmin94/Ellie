using UnityEngine;
using TheKiwiCoder;
using Channels.Boss;

[System.Serializable]
public class SetBossEvent : ActionNode
{
    public NodeProperty<bool> isInit;
    public NodeProperty<IBaseEventPayload> bossPayload;

    public NodeProperty<int> intValue;
    public NodeProperty<float> floatValue;
    public NodeProperty<Vector3> vector3Value;
    public NodeProperty<Transform> transformValue1;
    public NodeProperty<Transform> transformValue2;
    public NodeProperty<TerrapupaAttackType> attackTypeValue;

    protected override void OnStart() {
        if(isInit.Value)
        {
            bossPayload.Value = new BossEventPayload();
        }
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        BossEventPayload payload = bossPayload.Value as BossEventPayload;

        if(payload == null)
        {
            return State.Failure;
        }

        if(isInit.Value)
        {
            // �ʱ�ȭ O
            payload.IntValue = intValue.Value;
            payload.FloatValue = floatValue.Value;
            payload.Vector3Value = vector3Value.Value;
            payload.TransformValue1 = transformValue1.Value;
            payload.TransformValue2 = transformValue2.Value;
            payload.AttackTypeValue = attackTypeValue.Value;
        }
        else
        {
            // ���� �� ����
            if (intValue.Value != 0) payload.IntValue = intValue.Value;
            if (floatValue.Value != 0.0f) payload.FloatValue = floatValue.Value;
            if (vector3Value.Value != Vector3.zero) payload.Vector3Value = vector3Value.Value;
            if (transformValue1.Value != null) payload.TransformValue1 = transformValue1.Value;
            if (transformValue2.Value != null) payload.TransformValue2 = transformValue2.Value;
            if (attackTypeValue.Value != TerrapupaAttackType.None) payload.AttackTypeValue = attackTypeValue.Value;
        }

        payload.Sender = context.transform;
        bossPayload.Value = payload;

        return State.Success;
    }
}