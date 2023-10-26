using Assets.Scripts.Managers;
using Assets.Scripts.UI.Monster;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Type;

namespace Centers.Test
{
    public class TestCenter : BaseCenter
    {
        public CenterTestClient TestClient;

        private void Awake()
        {
            CheckTicket(TestClient.gameObject);

            var ui = UIManager.Instance.MakeStatic<UIMonsterBillboard>(transform, UIManager.UIMonsterBillboard);

            var tm = ui.gameObject.GetOrAddComponent<TicketMachine<IBaseEventPayload>>();
            tm.SetTicketType(ChannelType.Combat, ChannelType.UI);
        }
    }
}