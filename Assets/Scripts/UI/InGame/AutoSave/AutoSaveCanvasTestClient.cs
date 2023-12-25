using Assets.Scripts.Managers;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using Managers.Ticket;
using UnityEngine;
using Utils;

namespace UI.InGame.AutoSave
{
    public class AutoSaveCanvasTestClient : MonoBehaviour
    {
        private TicketMachine ticketMachine;

        private void Awake()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTicket(ChannelType.UI);

            TicketManager.Instance.Ticket(ticketMachine);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                var payload = new UIPayload();
                payload.uiType = UIType.Notify;
                payload.actionType = ActionType.OpenAutoSave;

                ticketMachine.SendMessage(ChannelType.UI, payload);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                var payload = new UIPayload();
                payload.uiType = UIType.Notify;
                payload.actionType = ActionType.CloseAutoSave;

                ticketMachine.SendMessage(ChannelType.UI, payload);
            }
        }
    }
}