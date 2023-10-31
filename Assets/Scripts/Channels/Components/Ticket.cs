using System;

namespace Channels.Components
{
    public class Ticket<T> where T : IBaseEventPayload
    {
        private Action<T> sendMessageAction;

        public virtual void Subscribe(Action<T> listener)
        {
            sendMessageAction -= listener;
            sendMessageAction += listener;
        }

        public virtual void Subscribe(BaseEventChannel channel)
        {
            sendMessageAction -= channel.ReceiveMessage;
            sendMessageAction += channel.ReceiveMessage;
        }

        public virtual void Publish(T payload)
        {
            sendMessageAction?.Invoke(payload);
        }
    }
}