using System;
using TheKiwiCoder;

[System.Serializable]
public class PublishEventBusPayload : ActionNode
{
    public NodeProperty<EventBusEvents> eventBusEvent;
    public NodeProperty<IBaseEventPayload> baseEventPayload;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        EventBus.Instance.Publish(eventBusEvent.Value, baseEventPayload.Value);
        return State.Success;
    }
}
