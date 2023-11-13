using TheKiwiCoder;
using Channels.Type;

[System.Serializable]
public class SendMessageToChannel : ActionNode
{
    public NodeProperty<ChannelType> channelType;
    public NodeProperty<IBaseEventPayload> payload;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        context.ticketMachine.SendMessage(channelType.Value, payload.Value);

        return State.Success;
    }
}
