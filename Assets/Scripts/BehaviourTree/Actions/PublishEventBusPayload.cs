using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using Assets.Scripts.Boss;

[System.Serializable]
public class PublishEventBusPayload : ActionNode
{
    public NodeProperty<EventBusEvents> eventBusEvent;
    public NodeProperty<BaseEventPayload> baseEventPayload;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        EventBus.Instance.Publish(eventBusEvent.Value, baseEventPayload.Value);
        Debug.Log("Å×½ºÆ®");
        return State.Success;
    }
}
