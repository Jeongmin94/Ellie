using Assets.Scripts.Managers;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using Managers.Ticket;
using UnityEngine;
using Utils;

namespace UI.Video
{
    public class VideoCanvasTestClient : MonoBehaviour
    {
        private TicketMachine ticketMachine;

        private void Awake()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.UI);

            TicketManager.Instance.Ticket(ticketMachine);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                var payload = UIPayload.Notify();
                payload.actionType = ActionType.PlayVideo;
                ticketMachine.SendMessage(ChannelType.UI, payload);
            }
        }
    }
}