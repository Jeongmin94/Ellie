using System;
using System.Collections.Generic;
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
using Data.UI.Opening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Dialog.GuideDialog
{
    // 특정 상황 최초 발생시 출력됨
    // 출력 후 n초 후에 사라짐
    // 동시에 여러 다이얼로그가 출력되면 이전 메시지 사라지고 출력됨
    // ex) 1번 , 2번 다이얼로그가 동시에 발생하면 1번 출력 -> 5초 표시 -> 1번 사라짐 -> 2번 출력
    public class GuideDialogCanvas : UIPopup
    {
        private struct GuideDialogInfo
        {
            private string speaker;
            private string message;
            private string image;

            private GuideDialogInfo(string speaker, string message, string image)
            {
                this.speaker = speaker;
                this.message = message;
                this.image = image;
            }

            public static GuideDialogInfo Of(string speaker, string message, string image)
            {
                return new GuideDialogInfo(speaker, message, image);
            }
        }

        // Speaker Image Base Path
        private static readonly string SpeakerImageBasePath = "UI/GuideDialog/SpeakerImage";

        private enum GameObjects
        {
            GuideDialogBackgroundPanel,
            GuideDialogImagePanel,
            GuideDialogSpeakerPanel,
            GuideDialogMessagePanel,
        }

        private enum Texts
        {
            GuideDialogSpeakerText,
            GuideDialogMessageText,
        }

        [SerializeField] private UITransformData[] transformData;
        [SerializeField] private TextTypographyData[] typographyData;

        private readonly List<GameObject> panels = new List<GameObject>();
        private readonly List<RectTransform> panelRects = new List<RectTransform>();

        private readonly List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
        private Image guideDialogImage;

        private TicketMachine ticketMachine;
        private readonly Queue<GuideDialogInfo> dialogInfoQueue = new Queue<GuideDialogInfo>();

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();

            Bind();
            InitObjects();
        }

        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));
            Bind<TextMeshProUGUI>(typeof(Texts));

            var gos = Enum.GetValues(typeof(GameObjects));
            for (int i = 0; i < gos.Length; i++)
            {
                var go = GetGameObject(i);
                panelRects.Add(go.GetComponent<RectTransform>());
                panels.Add(go);
            }

            var t = Enum.GetValues(typeof(Texts));
            for (int i = 0; i < t.Length; i++)
            {
                texts.Add(GetText(i));
            }

            guideDialogImage = panels[(int)GameObjects.GuideDialogImagePanel].GetComponent<Image>();
        }

        private void InitObjects()
        {
            for (int i = 0; i < transformData.Length; i++)
            {
                AnchorPresets.SetAnchorPreset(panelRects[i], AnchorPresets.MiddleCenter);
                panelRects[i].sizeDelta = transformData[i].actionRect.Value.GetSize();
                panelRects[i].localPosition = transformData[i].actionRect.Value.ToCanvasPos();
                panelRects[i].localScale = transformData[i].actionScale.Value;
            }

            for (int i = 0; i < typographyData.Length; i++)
            {
                TextTypographyData.SetTextTypographyData(texts[i], typographyData[i]);
            }

            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTicket(ChannelType.Dialog);
            ticketMachine.RegisterObserver(ChannelType.Dialog, OnNotify);

#if UNITY_EDITOR
            TicketManager.Instance.Ticket(ticketMachine);
#endif
        }

        private void OnNotify(IBaseEventPayload payload)
        {
            if (payload is not DialogPayload dialogPayload)
                return;
        }
    }
}