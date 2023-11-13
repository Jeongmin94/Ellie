using Assets.Scripts.UI.Framework;
using Centers;
using Channels.Type;
using Channels.UI;

namespace UI.Framework.Test
{
    public class UITestCenter : BaseCenter
    {
        public UITestClient client;

        private void Awake()
        {
            Init();
        }

        protected override void Start()
        {
            AddChannel(ChannelType.UI, new UIChannel());

            CheckTicket(client.gameObject);
        }
    }
}