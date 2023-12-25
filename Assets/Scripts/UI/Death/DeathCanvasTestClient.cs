using Assets.Scripts.Managers;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using Managers.Ticket;
using UnityEngine;
using Utils;

namespace UI.Death
{
    public class DeathCanvasTestClient : MonoBehaviour
    {
        private TicketMachine ticketMachine;

        private void Awake()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTicket(ChannelType.UI);

            TicketManager.Instance.Ticket(ticketMachine);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                var payload = new UIPayload();
                payload.uiType = UIType.Notify;
                payload.actionType = ActionType.OpenDeathCanvas;

                ticketMachine.SendMessage(ChannelType.UI, payload);
            }
        }
    }
}