using Channels.Components;
using Channels.Type;
using TheKiwiCoder;

[System.Serializable]
public class SendTicketMessage : ActionNode
{
    public NodeProperty<ChannelType> channelType;
    public NodeProperty<IBaseEventPayload> baseEventPayload;

    private TicketMachine ticketMachine;
    
    protected override void OnStart()
    {
        ticketMachine = context.ticketMachine;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        ticketMachine.SendMessage(channelType.Value, baseEventPayload.Value);
        return State.Success;
    }
}
