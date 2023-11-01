using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Item.PopupInven;
using Centers;
using Channels.Type;
using Channels.UI;

namespace UI.Framework.Test
{
    public class UITestCenter : BaseCenter
    {
        public UITestClient client;

        private UIPopupInvenCanvas popupInvenCanvas;

        private void Awake()
        {
            Init();
        }

        protected override void Start()
        {
            AddChannel(ChannelType.UI, new UIChannel());

            popupInvenCanvas = UIManager.Instance.MakePopup<UIPopupInvenCanvas>(UIManager.UIPopupInvenCanvas);

            CheckTicket(client.gameObject);
            CheckTicket(popupInvenCanvas.gameObject);
        }
    }
}