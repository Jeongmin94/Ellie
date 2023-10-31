using System;
using Channels.Components;

namespace Channels.UI
{
    public class UITicket<T> : Ticket<T> where T : IBaseEventPayload
    {
        public Action uiTicketAction;

        public override void Publish(T payload)
        {
            base.Publish(payload);
        }

        public override void Subscribe(BaseEventChannel channel)
        {
            base.Subscribe(channel);

            var uiChannel = channel as UIChannel;
            uiChannel.uiChannelAction -= OnUIChannelAction;
            uiChannel.uiChannelAction += OnUIChannelAction;
        }

        public override void Subscribe(Action<T> listener)
        {
            base.Subscribe(listener);
        }

        private void OnUIChannelAction()
        {
            uiTicketAction?.Invoke();
        }
    }
}