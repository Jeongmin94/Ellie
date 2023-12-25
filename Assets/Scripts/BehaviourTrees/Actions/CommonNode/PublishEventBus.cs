using System;
using Channels.Boss;
using Managers.Event;
using TheKiwiCoder;

[Serializable]
public class PublishEventBus : ActionNode
{
    public NodeProperty<EventBusEvents> eventBusEvent;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        EventBus.Instance.Publish(eventBusEvent.Value, new BossEventPayload());
        return State.Success;
    }
}