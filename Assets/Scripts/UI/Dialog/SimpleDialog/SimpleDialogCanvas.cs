using System;
using System.Collections;
using Assets.Scripts.Data.UI.Dialog;
using Assets.Scripts.Data.UI.Transform;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Inventory;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Dialog;
using Channels.Type;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Dialog
{
    public class SimpleDialogCanvas : UIPopup
    {
        private enum GameObjects
        {
            SimpleDialogPanel,
            SimpleDialogNextPanel,
        }

        private enum Texts
        {
            SimpleDialogNextText,
        }

        [SerializeField] private DialogTypographyData dialogContextData;
        [SerializeField] private UITransformData nextPanelTransform;

        private GameObject simpleDialogPanel;
        private GameObject nextPanel;

        private RectTransform simpleDialogPanelRect;
        private RectTransform nextPanelRect;
        private RectTransform nextTextRect;

        private TextMeshProUGUI nextText;

        private DialogText dialogText;

        private TicketMachine ticketMachine;

        private readonly TransformController nextPanelController = new TransformController();

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            simpleDialogPanel.gameObject.SetActive(false);
            nextPanel.gameObject.SetActive(false);
            
            dialogText.SubscribeIsPlayingAction(SendPayloadToClientEvent);
        }

        private void LateUpdate()
        {
#if UNITY_EDITOR
            nextPanelController.CheckQueue(nextPanelRect);
#endif
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
            Bind<TextMeshProUGUI>(typeof(Texts));

            simpleDialogPanel = GetGameObject((int)GameObjects.SimpleDialogPanel);
            simpleDialogPanelRect = simpleDialogPanel.GetComponent<RectTransform>();

            nextPanel = GetGameObject((int)GameObjects.SimpleDialogNextPanel);
            nextPanelRect = nextPanel.GetComponent<RectTransform>();

            nextText = GetText((int)Texts.SimpleDialogNextText);
            nextTextRect = nextText.GetComponent<RectTransform>();

            dialogText = simpleDialogPanel.GetOrAddComponent<DialogText>();
        }

        private void InitObjects()
        {
            AnchorPresets.SetAnchorPreset(simpleDialogPanelRect, AnchorPresets.MiddleCenter);
            simpleDialogPanelRect.sizeDelta = DialogConst.SimpleDialogPanelRect.GetSize();
            simpleDialogPanelRect.localPosition = DialogConst.SimpleDialogPanelRect.ToCanvasPos();

            AnchorPresets.SetAnchorPreset(nextPanelRect, AnchorPresets.MiddleCenter);
            nextPanelRect.sizeDelta = nextPanelTransform.actionRect.Value.GetSize();
            nextPanelRect.localPosition = nextPanelTransform.actionRect.Value.ToCanvasPos();
            nextPanelRect.localScale = nextPanelTransform.actionScale.Value;

#if UNITY_EDITOR
            nextPanelTransform.actionRect.Subscribe(nextPanelController.OnRectChange);
            nextPanelTransform.actionScale.Subscribe(nextPanelController.OnScaleChange);
#endif
            dialogText.InitDialogText();
            dialogText.InitTypography(dialogContextData);

            AnchorPresets.SetAnchorPreset(nextTextRect, AnchorPresets.StretchAll);
            nextTextRect.sizeDelta = Vector2.zero;
            nextTextRect.localPosition = Vector3.zero;
            
            DialogTypographyData.SetDialogTypography(nextText, dialogContextData);
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
                        //isPlaying�� �� �ٽ� Play ��û�� ������ OnNext�ϵ���
                        dialogText.Next();
                        break;
                    }

                    simpleDialogPanel.SetActive(true);
                    if(dialogPayload.canvasType == DialogCanvasType.SimpleRemaining)
                        nextPanelRect.gameObject.SetActive(true);
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
                simpleDialogPanel.SetActive(false);
                nextPanel.SetActive(false);
            }
        }

        private IEnumerator PlaySimpleDialog(DialogPayload payload)
        {
            yield return dialogText.Play(payload.text, payload.interval, payload.dialogDuration);

            if (payload.canvasType == DialogCanvasType.Simple)
            {
                SendPayloadEndingDialog();
                Stop();
            }
        }

        private void SendPayloadToClientEvent(bool _isPlaying)
        {
            ticketMachine.SendMessage(ChannelType.Dialog, new DialogPayload
            {
                dialogType = DialogType.NotifyToClient,
                isPlaying = _isPlaying
            });
        }
        private void SendPayloadEndingDialog()
        {
            ticketMachine.SendMessage(ChannelType.Dialog, new DialogPayload
            {
                dialogType = DialogType.NotifyToClient,
                isPlaying = false,
                isEnd = true,
            });
        }
    }
}