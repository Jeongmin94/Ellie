using System;

namespace Channels.Components
{
    public class Ticket<T> where T: IBaseEventPayload
    {
        public Action<T> SendMessageAction;

        public void Subscribe(Action<T> listener)
        {
            SendMessageAction -= listener;
            SendMessageAction += listener;
        }

        public void SendMessage(T payload)
        {
            SendMessageAction?.Invoke(payload);
        }
    }
}