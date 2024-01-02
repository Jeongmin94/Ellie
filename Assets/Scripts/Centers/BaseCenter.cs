using System.Collections.Generic;
using Channels;
using Channels.Components;
using Channels.Type;
using Channels.Utils;
using Controller;
using Data.Channels;
using Sirenix.OdinInspector;
using Spawner;
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
        public GameObject[] controllerInstances;
        public GameObject[] spawnerInstances;

        [ShowInInspector] private readonly IDictionary<ChannelType, BaseEventChannel> channels =
            new Dictionary<ChannelType, BaseEventChannel>();

        protected virtual void Start()
        {
            // !TODO Start() 메서드에서 CheckTicket 메서드를 호출하여 GameObject의 티켓을 만들어야 합니다. 
        }

        protected virtual void Init()
        {
            InitChannels();
        }

        private void InitChannels()
        {
            if (channelTypeSo == null)
            {
                return;
            }

            var length = channelTypeSo.channelTypes.Length;
            for (var i = 0; i < length; i++)
            {
                var type = channelTypeSo.channelTypes[i];
                channels[type] = ChannelUtil.MakeChannel(type);
            }
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
                var canvas = Instantiate(ui, Canvases.transform);
                CheckTicket(canvas.gameObject);
            }

            // instance only
            if (controllerInstances != null)
            {
                foreach (var controller in controllerInstances)
                {
                    controller.GetComponent<BaseController>().InitController();
                }
            }

            if (spawnerInstances != null)
            {
                foreach (var spawner in spawnerInstances)
                {
                    spawner.GetComponent<BaseSpanwer>().InitSpawner();
                }
            }
        }

        protected void CheckTicket(GameObject go)
        {
            var machines = go.GetComponentsInChildren<TicketMachine>();
            if (machines.Length == 0)
            {
                return;
            }

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