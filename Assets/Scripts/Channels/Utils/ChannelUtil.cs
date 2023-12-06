using System;
using Assets.Scripts.Channels.Item;
using Channels.Boss;
using Channels.Combat;
using Channels.Dialog;
using Channels.Type;
using Channels.UI;

namespace Channels.Utils
{
    public static class ChannelUtil
    {
        public static BaseEventChannel MakeChannel(ChannelType type)
        {
            BaseEventChannel channel = null;
            switch (type)
            {
                case ChannelType.Combat:
                    channel = new CombatChannel();
                    break;
                case ChannelType.UI:
                    channel = new UIChannel();
                    break;
                case ChannelType.Stone:
                    channel = new StoneChannel();
                    break;
                case ChannelType.BossInteraction:
                    channel = new BossInteractionChannel();
                    break;
                case ChannelType.Terrapupa:
                    channel = new TerrapupaChannel();
                    break;
                case ChannelType.Dialog:
                    channel = new DialogChannel();
                    break;
                case ChannelType.Monster:
                    channel = new MonsterChannel();
                    break;
                case ChannelType.Camera:
                    channel = new MonsterChannel();
                    break;
            }
            return channel;
        }
    }
}