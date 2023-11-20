using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Dialog;
using Channels.Type;
using UnityEngine;

namespace UI.Dialog.Test
{
    public class DialogTestClient : MonoBehaviour
    {
        public string text = "Hello World Hello World Hello World Hello World";

        private TicketMachine ticketMachine;

        private void Awake()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTicket(ChannelType.Dialog);

            TicketManager.Instance.Ticket(ticketMachine);
        }

        private void OnGUI()
        {
            text = GUI.TextField(new Rect(10, 10, 500, 50), text, 100);

            if (GUI.Button(new Rect(520, 10, 100, 50), "Play Dialog"))
            {
                ticketMachine.SendMessage(ChannelType.Dialog, DialogPayload.Play(text, 0.1f));
            }

            if (GUI.Button(new Rect(625, 10, 100, 50), "Pause"))
            {
                ticketMachine.SendMessage(ChannelType.Dialog, DialogPayload.Pause());
            }

            if (GUI.Button(new Rect(730, 10, 100, 50), "Resume"))
            {
                ticketMachine.SendMessage(ChannelType.Dialog, DialogPayload.Resume());
            }

            if (GUI.Button(new Rect(520, 65, 100, 50), "Stop"))
            {
                ticketMachine.SendMessage(ChannelType.Dialog, DialogPayload.Stop());
            }

            if (GUI.Button(new Rect(625, 65, 100, 50), "OnNext"))
            {
                ticketMachine.SendMessage(ChannelType.Dialog, DialogPayload.OnNext());
            }
        }
    }
}