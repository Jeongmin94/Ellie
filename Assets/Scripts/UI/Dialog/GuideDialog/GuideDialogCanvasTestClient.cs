using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Dialog;
using Channels.Type;
using Data.GoogleSheet._8200GuideDialog;
using UnityEngine;

namespace UI.Dialog.GuideDialog
{
    public class GuideDialogCanvasTestClient : MonoBehaviour
    {
        [SerializeField] private GuideDialogParsingInfo parsingInfo;

        private TicketMachine ticketMachine;

        private void Awake()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTicket(ChannelType.Dialog);

#if UNITY_EDITOR
            TicketManager.Instance.Ticket(ticketMachine);
#endif
        }

        // test index
        // 8200, 8201, 8202
        private readonly List<int> testIndex = new List<int>() { 8200, 8201, 8202 };

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                var data = parsingInfo.GetIndexData<GuideDialogData>(testIndex[0]);
                DialogPayload payload = new DialogPayload();
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
                DialogPayload payload = new DialogPayload();
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
                DialogPayload payload = new DialogPayload();
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