using System.Collections.Generic;
using System.Linq;
using Channels.Type;
using UnityEngine;

namespace Channels.Components
{
    public class TicketMachine<T> : Component where T : IBaseEventPayload
    {
        private readonly IDictionary<ChannelType, Ticket<T>> tickets =
            new Dictionary<ChannelType, Ticket<T>>();

        public void SetTicketType(params ChannelType[] types)
        {
            foreach (ChannelType type in types)
            {
                if (tickets.ContainsKey(type))
                {
                    Debug.LogWarning($"{name}'s tickets already has {type} ticket");
                }
                else
                {
                    tickets[type] = new Ticket<T>();
                }
            }
        }

        public Ticket<T> GetTicket(ChannelType type)
        {
            return tickets[type];
        }

        public void SendMessage(ChannelType type, T payload)
        {
            if (tickets.TryGetValue(type, out var ticket))
                ticket.SendMessageAction?.Invoke(payload);
        }

        public void Ticket(IDictionary<ChannelType, BaseEventChannel> channels)
        {
            if (tickets.Any())
            {
                foreach (var key in tickets.Keys)
                {
                    tickets[key].SendMessageAction -= channels[key].ReceiveMessage;
                    tickets[key].SendMessageAction += channels[key].ReceiveMessage;
                }
            }
            else
            {
                Debug.LogWarning($"{name}'s ticketTypes is Empty");
            }
        }
    }
}