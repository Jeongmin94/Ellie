using System;
using System.Collections.Generic;
using Channels.Type;
using UnityEngine;

namespace Channels.Components
{
    public class ChannelComponent : Component
    {
        public readonly List<ChannelType> NeededChannels = new List<ChannelType>();

        private readonly IDictionary<ChannelType, BaseEventChannel<IBaseEventPayload>> channels =
            new Dictionary<ChannelType, BaseEventChannel<IBaseEventPayload>>();

        public void SubscribeChannel(ChannelType type, Action<IBaseEventPayload> action)
        {
            if (channels.TryGetValue(type, out var channel))
                channel.Subscribe(action);
        }

        public void Publish(ChannelType type, IBaseEventPayload payload)
        {
            if (channels.TryGetValue(type, out var channel))
                channel.Publish(payload);
        }
    }
}