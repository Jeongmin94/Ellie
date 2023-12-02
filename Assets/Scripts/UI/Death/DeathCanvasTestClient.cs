using System;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using UnityEngine;

namespace Assets.Scripts.UI.Death
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
                UIPayload payload = new UIPayload();
                payload.uiType = UIType.Notify;
                payload.actionType = ActionType.OpenDeathCanvas;

                ticketMachine.SendMessage(ChannelType.UI, payload);
            }
        }
    }
}