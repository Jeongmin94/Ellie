using System.Collections.Generic;
using Assets.Scripts.Data.Channels;
using Channels;
using Channels.Components;
using Channels.Type;
using Channels.Utils;
using UnityEngine;

namespace Centers
{
    public readonly struct TicketBox
    {
        private readonly ChannelType type;
        private readonly Ticket ticket;

        private TicketBox(ChannelType type, Ticket ticket)
        {
            this.type = type;
            this.ticket = ticket;
        }

        public static TicketBox Of(ChannelType type, Ticket ticket)
        {
            return new TicketBox(type, ticket);
        }

        public void Ticket(IDictionary<ChannelType, BaseEventChannel> channels)
        {
            ticket.Subscribe(channels[type]);
        }
    }

    public class BaseCenter : MonoBehaviour
    {
        [SerializeField] private BaseChannelTypeSo channelTypeSo;

        public GameObject Canvases;
        public GameObject[] uiPrefabs;


        private readonly IDictionary<ChannelType, BaseEventChannel> channels =
            new Dictionary<ChannelType, BaseEventChannel>();

        protected virtual void Init()
        {
            InitChannels();
        }

        protected virtual void InitObjects()
        {
            if (Canvases != null)
            {
                foreach (Transform child in Canvases.transform)
                {
                    CheckTicket(child.gameObject);
                }
            }

            foreach (var ui in uiPrefabs)
            {
                Instantiate(ui, Canvases.transform);
            }
        }

        private void InitChannels()
        {
            if (channelTypeSo == null)
                return;

            int length = channelTypeSo.channelTypes.Length;
            for (int i = 0; i < length; i++)
            {
                ChannelType type = channelTypeSo.channelTypes[i];
                channels[type] = ChannelUtil.MakeChannel(type);
            }
        }

        protected virtual void Start()
        {
            // !TODO Start() 메서드에서 CheckTicket 메서드를 호출하여 GameObject의 티켓을 만들어야 합니다. 
        }

        protected void CheckTicket(GameObject go)
        {
            var machines = go.GetComponentsInChildren<TicketMachine>();
            if (machines.Length == 0)
                return;

            foreach (var machine in machines)
            {
                machine.Ticket(this);
            }
        }

        public void OnAddTicket(TicketBox box)
        {
            box.Ticket(channels);
        }

        protected void AddChannel(ChannelType type, BaseEventChannel channel)
        {
            if (channels.ContainsKey(type))
            {
                Debug.LogWarning($"{type}'s channel is already exist");
            }
            else
            {
                channels[type] = channel;
            }
        }

        public BaseEventChannel GetChannel(ChannelType type)
        {
            if (channels.TryGetValue(type, out var channel))
            {
                return channel;
            }

            return null;
        }
    }
}