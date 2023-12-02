using System;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using UnityEngine;

namespace Assets.Scripts.UI.Interactive
{
    public class InteractiveCanvasTestClient : MonoBehaviour
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
            // 1 대화하기
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ticketMachine.SendMessage(ChannelType.UI, Make(ActionType.PopupInteractive, InteractiveType.Chatting));
            }
            // 2 채광하기
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ticketMachine.SendMessage(ChannelType.UI, Make(ActionType.PopupInteractive, InteractiveType.Mining));
            }
            // 3 획득하기
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ticketMachine.SendMessage(ChannelType.UI, Make(ActionType.PopupInteractive, InteractiveType.Acquisition));
            }
            // 4 끄기
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ticketMachine.SendMessage(ChannelType.UI, Make(ActionType.CloseInteractive, InteractiveType.Acquisition));
            }
        }

        private UIPayload Make(ActionType actionType, InteractiveType interactiveType)
        {
            UIPayload payload = UIPayload.Notify();
            payload.actionType = actionType;
            payload.interactiveType = interactiveType;

            return payload;
        }
    }
}