using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using Channels.Boss;

[System.Serializable]
public class SetTerrapupaEvent : ActionNode
{
    public NodeProperty<bool> isInit;
    public NodeProperty<IBaseEventPayload> terrapupaPayload;

    public NodeProperty<int> attackValue;
    public NodeProperty<float> cooldown;
    public NodeProperty<Transform> sender;
    public NodeProperty<Transform> receiver;
    public NodeProperty<TerrapupaEvent> eventType;

    protected override void OnStart()
    {
        if (isInit.Value)
        {
            terrapupaPayload.Value = new TerrapupaPayload();
        }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        TerrapupaPayload payload = terrapupaPayload.Value as TerrapupaPayload;
        if (payload == null)
        {
            return State.Failure;
        }

        if (isInit.Value)
        {
            // 초기화
            payload.AttackValue = attackValue.Value;
            payload.Cooldown = cooldown.Value;
            payload.Sender = sender.Value;
            payload.Receiver = receiver.Value;
            payload.Type = eventType.Value;
        }
        else
        {
            // 기존 값 유지
            if (attackValue.Value != 0) payload.AttackValue = attackValue.Value;
            if (cooldown.Value != 0.0f) payload.Cooldown = cooldown.Value;
            if (sender.Value != null) payload.Sender = sender.Value;
            if (receiver.Value != null) payload.Receiver = receiver.Value;
            if (eventType.Value != TerrapupaEvent.None) payload.Type = eventType.Value;
        }

        terrapupaPayload.Value = payload;

        return State.Success;
    }
}
