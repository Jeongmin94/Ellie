using System;
using Channels.Components;

namespace Channels.UI
{
    public class UITicket<T> : Ticket<T> where T : IBaseEventPayload
    {
        public override void Publish(T payload)
        {
            base.Publish(payload);
        }

        public override void Subscribe(Action<T> listener)
        {
            base.Subscribe(listener);
        }
    }
}