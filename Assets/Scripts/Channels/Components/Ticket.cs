using System;

namespace Channels.Components
{
    public class Ticket<T> where T : IBaseEventPayload
    {
        private Action<T> sendMessageAction;
        private Action<IBaseEventPayload> notifyAction;

        public virtual void Subscribe(Action<T> listener)
        {
            sendMessageAction -= listener;
            sendMessageAction += listener;
        }
        public virtual void Subscribe(BaseEventChannel channel)
        {
            channel.SubscribeNotifyAction(OnNotifyAction);
            Subscribe(channel.ReceiveMessage);
        }
        public virtual void Publish(T payload)
        {
            sendMessageAction?.Invoke(payload);
        }

        public virtual void OnNotifyAction(IBaseEventPayload payload)
        {
            notifyAction?.Invoke(payload);
        }

        public void SubscribeNotifyAction(Action<IBaseEventPayload> listener)
        {
            notifyAction -= listener;
            notifyAction += listener;
        }
    }
}