using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using Channels.Boss;

[System.Serializable]
public class SetBossInteractionEvent : ActionNode
{
    public NodeProperty<bool> isInit;
    public NodeProperty<IBaseEventPayload> bossInteractionPayload;

    public NodeProperty<float> cooldown;
    public NodeProperty<Transform> sender;
    public NodeProperty<Transform> receiver;
    public NodeProperty<BossInteractionEvent> eventType;
    public NodeProperty<TerrapupaAttackType> terrapupaBannedAttackType;

    protected override void OnStart()
    {
        if (isInit.Value)
        {
            bossInteractionPayload.Value = new BossInteractionPayload();
        }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        BossInteractionPayload payload = bossInteractionPayload.Value as BossInteractionPayload;
        if (payload == null)
        {
            return State.Failure;
        }

        if (isInit.Value)
        {
            // 초기화
            payload.Cooldown = cooldown.Value;
            payload.Sender = sender.Value;
            payload.Receiver = receiver.Value;
            payload.Type = eventType.Value;
            payload.TerrapupaBannedAttackType = terrapupaBannedAttackType.Value;
        }
        else
        {
            // 기존 값 유지
            if (cooldown.Value != 0.0f) payload.Cooldown = cooldown.Value;
            if (sender.Value != null) payload.Sender = sender.Value;
            if (receiver.Value != null) payload.Receiver = receiver.Value;
            if (eventType.Value != BossInteractionEvent.None) payload.Type = eventType.Value;
            if (terrapupaBannedAttackType.Value != TerrapupaAttackType.None) payload.TerrapupaBannedAttackType = terrapupaBannedAttackType.Value;
        }

        bossInteractionPayload.Value = payload;

        return State.Success;
    }
}
