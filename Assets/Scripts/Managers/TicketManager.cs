using System;
using System.Collections.Generic;
using Channels;
using Channels.Components;
using Channels.Type;
using Channels.Utils;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class TicketManager : Singleton<TicketManager>
    {
        [SerializeField]
        private readonly IDictionary<ChannelType, BaseEventChannel> channels = new Dictionary<ChannelType, BaseEventChannel>();

        public override void Awake()
        {
            base.Awake();

            InitBaseChannels();
        }

        private void InitBaseChannels()
        {
            var types = Enum.GetValues(typeof(ChannelType));
            for (int i = 0; i < types.Length; i++)
            {
                var type = (ChannelType)types.GetValue(i);
                channels.TryAdd(type, ChannelUtil.MakeChannel(type));
            }
        }

        public void Ticket(TicketMachine machine)
        {
            machine.Ticket(channels);
        }

        public void CheckTicket(GameObject go)
        {
            var machines = go.GetComponentsInChildren<TicketMachine>();
            if (machines.Length == 0)
                return;

            foreach (var machine in machines)
            {
                machine.Ticket(channels);
            }
        }
    }
}