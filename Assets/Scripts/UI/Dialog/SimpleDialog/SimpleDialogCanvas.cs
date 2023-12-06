using System;
using System.Collections;
using Assets.Scripts.Data.UI.Dialog;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Inventory;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Dialog;
using Channels.Type;
using UnityEngine;

namespace Assets.Scripts.UI.Dialog
{
    public class SimpleDialogCanvas : UIPopup
    {
        private enum GameObjects
        {
            SimpleDialogPanel,
        }

        [SerializeField] private DialogTypographyData dialogContextData;

        private GameObject simpleDialogPanel;
        private RectTransform simpleDialogPanelRect;

        private DialogText dialogText;

        private TicketMachine ticketMachine;

        private void Awake()
        {
            Init();
        }
        private void Start()
        {
            simpleDialogPanel.gameObject.SetActive(false);
            dialogText.SubscribeIsPlayingAction(SendPayloadToClientEvent);
        }

        protected override void Init()
        {
            base.Init();

            Bind();
            InitObjects();
            InitTicketMachine();
        }


        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));

            simpleDialogPanel = GetGameObject((int)GameObjects.SimpleDialogPanel);
            simpleDialogPanelRect = simpleDialogPanel.GetComponent<RectTransform>();

            dialogText = simpleDialogPanel.GetOrAddComponent<DialogText>();
        }

        private void InitObjects()
        {
            AnchorPresets.SetAnchorPreset(simpleDialogPanelRect, AnchorPresets.MiddleCenter);
            simpleDialogPanelRect.sizeDelta = DialogConst.SimpleDialogPanelRect.GetSize();
            simpleDialogPanelRect.localPosition = DialogConst.SimpleDialogPanelRect.ToCanvasPos();

            dialogText.InitDialogText();
            dialogText.InitTypography(dialogContextData);
        }

        private void InitTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();

            ticketMachine.AddTickets(ChannelType.Dialog);
            ticketMachine.RegisterObserver(ChannelType.Dialog, OnNotify);

            TicketManager.Instance.Ticket(ticketMachine);
        }

        private void OnNotify(IBaseEventPayload payload)
        {
            if (payload is not DialogPayload dialogPayload) return;

            if (dialogPayload.canvasType != DialogCanvasType.Simple &&
                dialogPayload.canvasType != DialogCanvasType.SimpleRemaining) return;

            switch (dialogPayload.dialogAction)
            {
                case DialogAction.Play:
                    {
                        if (dialogText.IsPlaying)
                        {
                            //isPlaying일 때 다시 Play 요청이 들어오면 OnNext하도록
                            dialogText.Next();
                            break;
                        }
                        dialogText.gameObject.SetActive(true);
                        Play(dialogPayload);
                    }
                    break;

                case DialogAction.Stop:
                    {
                        Stop();
                    }
                    break;

                case DialogAction.Resume:
                    {
                        dialogText.SetPause(false);
                    }
                    break;

                case DialogAction.Pause:
                    {
                        dialogText.SetPause(true);
                    }
                    break;

                case DialogAction.OnNext:
                    {
                        dialogText.Next();
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Play(DialogPayload payload)
        {
            if (payload.canvasType == DialogCanvasType.Simple && dialogText.IsPlaying)
                return;
            StartCoroutine(PlaySimpleDialog(payload));
        }

        private void Stop()
        {
            if (dialogText.Stop())
            {
                dialogText.gameObject.SetActive(false);
            }
        }

        private IEnumerator PlaySimpleDialog(DialogPayload payload)
        {
            yield return dialogText.Play(payload.text, payload.interval, 1.0f);

            if (payload.canvasType == DialogCanvasType.Simple)
                Stop();
        }

        private void SendPayloadToClientEvent(bool _isPlaying)
        {

            ticketMachine.SendMessage(ChannelType.Dialog, new DialogPayload
            {
                dialogType = DialogType.NotifyToClient,
                isPlaying = _isPlaying
            });
        }
    }
}