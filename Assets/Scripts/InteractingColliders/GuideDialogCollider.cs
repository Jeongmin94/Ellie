using System;
using System.Collections;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Dialog;
using Channels.Type;
using Data.GoogleSheet._8200GuideDialog;
using UnityEngine;

namespace InteractingColliders
{
    public class GuideDialogCollider : MonoBehaviour
    {
        [SerializeField] private int index;
        private GuideDialogData data;

        private TicketMachine ticketMachine;

        private void Awake()
        {
            InitTicketMachine();
            StartCoroutine(InitGuideDialogData());
        }

        private void InitTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.Dialog);
        }
        private IEnumerator InitGuideDialogData()
        {
            yield return DataManager.Instance.CheckIsParseDone();
            data = DataManager.Instance.GetIndexData<GuideDialogData, GuideDialogParsingInfo>(index);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            ticketMachine.SendMessage(ChannelType.Dialog, new DialogPayload()
            {
                dialogType = DialogType.Notify,
                canvasType = DialogCanvasType.GuideDialog,
                speaker = data.speaker,
                text = data.message,
                imageName = data.imageName
            });
            
            gameObject.SetActive(false);
        }
    }
}