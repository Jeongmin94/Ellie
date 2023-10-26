using System;
using System.Collections.Generic;
using System.Linq;
using Centers;
using Channels.Type;
using UnityEngine;

namespace Channels.Components
{
    public class TicketMachine : MonoBehaviour
    {
        private Action<TicketBox> addTicketAction;

        private readonly IDictionary<ChannelType, Ticket<IBaseEventPayload>> tickets =
            new Dictionary<ChannelType, Ticket<IBaseEventPayload>>();

        public void Subscribe(Action<TicketBox> listener)
        {
            addTicketAction -= listener;
            addTicketAction += listener;
        }

        public Ticket<IBaseEventPayload> GetTicket(ChannelType type)
        {
            return tickets[type];
        }

        public void SendMessage(ChannelType type, IBaseEventPayload payload)
        {
            if (tickets.TryGetValue(type, out var ticket))
                ticket.Publish(payload);
        }

        /// <summary>
        /// ChannelType[]에 맞는 티켓을 초기화합니다.
        /// 기본 Ticket만 생성합니다.
        /// </summary>
        /// <param name="types"></param>
        public void AddTickets(params ChannelType[] types)
        {
            foreach (ChannelType type in types)
            {
                if (tickets.ContainsKey(type))
                {
                    Debug.LogWarning($"{name}'s tickets already has {type} ticket");
                }
                else
                {
                    tickets[type] = new Ticket<IBaseEventPayload>();
                }
            }
        }

        /// <summary>
        /// 각 Channel이 ChannelType에 맞는 Ticket의 이벤트를 구독합니다.
        /// GameCenter에서 호출됩니다.
        /// </summary>
        /// <param name="channels"></param>
        public void Ticket(IDictionary<ChannelType, BaseEventChannel> channels)
        {
            if (tickets.Any())
            {
                foreach (var channelType in tickets.Keys)
                {
                    tickets[channelType].Subscribe(channels[channelType].ReceiveMessage);
                }
            }
            else
            {
                Debug.LogWarning($"{name}'s ticketTypes is Empty");
            }
        }

        /// <summary>
        /// 런타임에 Ticket을 추가합니다.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ticket"></param>
        public void AddTicket(ChannelType type, Ticket<IBaseEventPayload> ticket)
        {
            if (tickets.ContainsKey(type))
            {
                Debug.LogWarning($"{name}'s tickets already has {type} ticket");
            }
            else
            {
                tickets[type] = ticket;
                addTicketAction?.Invoke(TicketBox.Of(type, tickets[type]));
            }
        }
    }
}