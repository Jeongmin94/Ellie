using System;
using Channels.Combat;
using Channels.Type;
using Channels.UI;

namespace Channels.Utils
{
    public static class ChannelUtil
    {
        public static BaseEventChannel<IBaseEventPayload> MakeChannel(ChannelType type)
        {
            BaseEventChannel<IBaseEventPayload> channel = null;
            switch (type)
            {
                case ChannelType.Combat:
                    channel = new CombatChannel<IBaseEventPayload>();
                    break;
                case ChannelType.UI:
                    channel = new UIChannel<IBaseEventPayload>();
                    break;
            }

            return channel;
        }
    }
}