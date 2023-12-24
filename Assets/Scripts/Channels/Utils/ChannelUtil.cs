using Channels.Boss;
using Channels.Camera;
using Channels.Combat;
using Channels.Dialog;
using Channels.Monsters;
using Channels.Portal;
using Channels.Stone;
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
                case ChannelType.BossBattle:
                    channel = new BossBattleChannel();
                    break;
                case ChannelType.Terrapupa:
                    channel = new BossChannel();
                    break;
                case ChannelType.Dialog:
                    channel = new DialogChannel();
                    break;
                case ChannelType.Monster:
                    channel = new MonsterChannel();
                    break;
                case ChannelType.Camera:
                    channel = new CameraChannel();
                    break;
                case ChannelType.Item:
                    break;
                case ChannelType.Portal:
                    channel = new PortalChannel();
                    break;
                case ChannelType.BossDialog:
                    channel = new BossDialogChannel();
                    break;
            }

            return channel;
        }
    }
}