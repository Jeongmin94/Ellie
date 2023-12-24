using System.Collections.Generic;
using Assets.Scripts.Managers;
using Channels.Components;
using Channels.Dialog;
using Channels.Type;
using Data.GoogleSheet._8200GuideDialog;
using UnityEngine;
using Utils;

namespace UI.Dialog.GuideDialog
{
    public class GuideDialogCanvasTestClient : MonoBehaviour
    {
        [SerializeField] private GuideDialogParsingInfo parsingInfo;

        // test index
        // 8200, 8201, 8202
        private readonly List<int> testIndex = new() { 8200, 8201, 8202 };

        private TicketMachine ticketMachine;

        private void Awake()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTicket(ChannelType.Dialog);

#if UNITY_EDITOR
            TicketManager.Instance.Ticket(ticketMachine);
#endif
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                var data = parsingInfo.GetIndexData<GuideDialogData>(testIndex[0]);
                var payload = new DialogPayload();
                payload.dialogType = DialogType.Notify;
                payload.canvasType = DialogCanvasType.GuideDialog;

                payload.speaker = data.speaker;
                payload.text = data.message;
                payload.imageName = data.imageName;

                ticketMachine.SendMessage(ChannelType.Dialog, payload);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                var data = parsingInfo.GetIndexData<GuideDialogData>(testIndex[1]);
                var payload = new DialogPayload();
                payload.dialogType = DialogType.Notify;
                payload.canvasType = DialogCanvasType.GuideDialog;

                payload.speaker = data.speaker;
                payload.text = data.message;
                payload.imageName = data.imageName;

                ticketMachine.SendMessage(ChannelType.Dialog, payload);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                var data = parsingInfo.GetIndexData<GuideDialogData>(testIndex[2]);
                var payload = new DialogPayload();
                payload.dialogType = DialogType.Notify;
                payload.canvasType = DialogCanvasType.GuideDialog;

                payload.speaker = data.speaker;
                payload.text = data.message;
                payload.imageName = data.imageName;

                ticketMachine.SendMessage(ChannelType.Dialog, payload);
            }
        }
    }
}