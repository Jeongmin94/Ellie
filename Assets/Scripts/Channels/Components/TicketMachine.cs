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

        private readonly IDictionary<ChannelType, Ticket> tickets =
            new Dictionary<ChannelType, Ticket>();

        private void Subscribe(Action<TicketBox> listener)
        {
            addTicketAction -= listener;
            addTicketAction += listener;
        }

        public Ticket GetTicket(ChannelType type)
        {
            return tickets[type];
        }

        public void SendMessage(ChannelType type, IBaseEventPayload payload)
        {

            if (tickets.TryGetValue(type, out var ticket))
                ticket.Publish(payload);
        }

        public void Notify(ChannelType type, IBaseEventPayload payload)
        {
            if (tickets.TryGetValue(type, out var ticket))
                ticket.Notify(payload);
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
                    tickets[type] = new Ticket();
                }
            }
        }

        /// <summary>
        /// 각 Channel이 ChannelType에 맞는 Ticket의 이벤트를 구독합니다.
        /// GameCenter에서 호출됩니다.
        /// </summary>
        public void Ticket(BaseCenter center)
        {
            if (tickets.Any())
            {
                foreach (var channelType in tickets.Keys)
                {
                    Debug.Log(channelType + " 채널 구독 : " + gameObject.name);
                    tickets[channelType].Subscribe(center.GetChannel(channelType));
                    Subscribe(center.OnAddTicket);
                }
            }
            else
            {
                Debug.LogWarning($"{name}'s ticketTypes is Empty");
            }
        }

        public void Ticket(IDictionary<ChannelType, BaseEventChannel> channels)
        {
            if (tickets.Any())
            {
                foreach (var channelType in tickets.Keys)
                {
                    Debug.Log(channelType + " 채널 구독 : " + gameObject.name);
                    tickets[channelType].Subscribe(channels[channelType]);
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
        public void AddTicket(ChannelType type)
        {
            if (tickets.ContainsKey(type))
            {
                Debug.LogWarning($"{name}'s tickets already has {type} ticket");
            }
            else
            {
                tickets[type] = new Ticket();
                addTicketAction?.Invoke(TicketBox.Of(type, tickets[type]));
            }
        }

        public void AddTicket(TicketMachine machine)
        {
            if (addTicketAction == null)
            {
                Debug.LogWarning($"{name} is invalid ticket machine");
            }
            else
            {
                var keys = machine.tickets.Keys;
                foreach (ChannelType type in keys)
                {
                    addTicketAction?.Invoke(TicketBox.Of(type, machine.tickets[type]));
                }
            }
        }

        public void RegisterObserver(ChannelType type, Action<IBaseEventPayload> observer)
        {
            Components.Ticket.RegisterObserver(tickets[type], observer);
        }
    }
}