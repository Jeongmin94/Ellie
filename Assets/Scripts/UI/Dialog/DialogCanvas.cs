using System;
using Assets.Scripts.Data.UI.Dialog;
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
using UnityEngine.UI;

namespace Assets.Scripts.UI.Dialog
{
    public class DialogCanvas : UIPopup
    {
        public static readonly string Path = "Dialog/DialogCanvas";

        private enum GameObjects
        {
            DialogPanel,
            DialogNextPanel,
            DialogContextPanel,
        }

        private enum Images
        {
            DialogImage,
        }

        private enum Texts
        {
            DialogTitle,
            DialogNext,
        }

        [SerializeField] private DialogTypographyData dialogContextData;

        private GameObject dialogPanel;
        private RectTransform dialogPanelRect;

        // 다음 버튼 패널
        private GameObject dialogNextPanel;
        private RectTransform dialogNextPanelRect;

        // 대사 출력 패널
        private GameObject dialogContextPanel;
        private DialogText dialogContextText;

        private Image dialogImage;

        private TextMeshProUGUI dialogTitle;
        private TextMeshProUGUI dialogNext;

        private TicketMachine ticketMachine;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();

            Bind();
            InitObjects();
            InitTicketMachine();
        }
        private void Start()
        {
            dialogPanel.gameObject.SetActive(false);
            dialogContextText.SubscribeIsPlayingAction(SendPayloadToClientEvent);
            //dialogContextText.SubscribeEndingAction(SendPayloadEndingDialog);
        }
        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));
            Bind<Image>(typeof(Images));
            Bind<TextMeshProUGUI>(typeof(Texts));

            dialogPanel = GetGameObject((int)GameObjects.DialogPanel);
            dialogPanelRect = dialogPanel.GetComponent<RectTransform>();

            dialogNextPanel = GetGameObject((int)GameObjects.DialogNextPanel);
            dialogNextPanelRect = dialogNextPanel.GetComponent<RectTransform>();

            dialogContextPanel = GetGameObject((int)GameObjects.DialogContextPanel);

            dialogImage = GetImage((int)Images.DialogImage);

            dialogTitle = GetText((int)Texts.DialogTitle);
            dialogNext = GetText((int)Texts.DialogNext);

            //contextText의 정보를 페이로드통해 송신하기 위한 이벤트 구독
            
        }

        private void InitObjects()
        {
            AnchorPresets.SetAnchorPreset(dialogPanelRect, AnchorPresets.MiddleCenter);
            dialogPanelRect.sizeDelta = DialogConst.DialogPanelRect.GetSize();
            dialogPanelRect.localPosition = DialogConst.DialogPanelRect.ToCanvasPos();

            dialogNextPanelRect.SetParent(transform);
            {
                AnchorPresets.SetAnchorPreset(dialogNextPanelRect, AnchorPresets.MiddleCenter);
                dialogNextPanelRect.sizeDelta = DialogConst.DialogNextRect.GetSize();
                dialogNextPanelRect.localPosition = DialogConst.DialogNextRect.ToCanvasPos();
            }
            dialogNextPanelRect.SetParent(dialogPanelRect);

            dialogContextText = dialogContextPanel.gameObject.GetOrAddComponent<DialogText>();
            dialogContextText.InitDialogText();
            dialogContextText.InitTypography(dialogContextData);
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

            if (dialogPayload.dialogType != DialogType.Notify) return;

            if (dialogPayload.canvasType != DialogCanvasType.Default) return;

            switch (dialogPayload.dialogAction)
            {
                case DialogAction.Play:
                    {
                        if (dialogContextText.IsPlaying)
                        {
                            //isPlaying일 때 다시 Play 요청이 들어오면 OnNext하도록
                            dialogContextText.Next();

                            break;
                        }
                        dialogPanel.gameObject.SetActive(true);

                        dialogTitle.text = dialogPayload.speaker;
                        dialogContextText.Play(dialogPayload.text, dialogPayload.interval);
                    }
                    break;

                case DialogAction.Stop:
                    {
                        if (dialogContextText.Stop())
                        {
                            dialogPanel.gameObject.SetActive(false);
                        }
                    }
                    break;

                case DialogAction.Resume:
                    {
                        dialogContextText.SetPause(false);
                    }
                    break;

                case DialogAction.Pause:
                    {
                        dialogContextText.SetPause(true);
                    }
                    break;

                case DialogAction.OnNext:
                    {
                        dialogContextText.Next();
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
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
    }
}