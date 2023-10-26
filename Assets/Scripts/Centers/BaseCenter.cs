using System.Collections.Generic;
using Assets.Scripts.Data.Channels;
using Channels;
using Channels.Components;
using Channels.Type;
using Channels.Utils;
using UnityEngine;

namespace Centers
{
    public class BaseCenter : MonoBehaviour
    {
        [SerializeField] private ChannelTypesSo channelTypesSo;

        private readonly IDictionary<ChannelType, BaseEventChannel> channels =
            new Dictionary<ChannelType, BaseEventChannel>();

        protected virtual void Init()
        {
            InitChannels();
        }

        private void InitChannels()
        {
            int length = channelTypesSo.channelTypes.Length;
            for (int i = 0; i < length; i++)
            {
                ChannelType type = channelTypesSo.channelTypes[i];
                channels[type] = ChannelUtil.MakeChannel(type);
            }
        }

        protected void CheckTicket(GameObject go)
        {
            var ticketMachine = go.GetComponent<TicketMachine<IBaseEventPayload>>();
            if (ticketMachine == null)
                return;

            ticketMachine.Ticket(channels);
        }
    }
}