using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Dialog.GuideDialog
{
    public class GuideDialogCanvas : UIPopup
    {
        private struct GuideDialogInfo
        {
            public string Speaker { get; }

            public string Message { get; }

            public string Image { get; }

            private GuideDialogInfo(string speaker, string message, string image)
            {
                Speaker = speaker;
                Message = message;
                Image = image;
            }

            public static GuideDialogInfo Of(string speaker, string message, string image)
            {
                return new GuideDialogInfo(speaker, message, image);
            }
        }

        // Speaker Image Base Path
        private static readonly string SpeakerImageBasePath = "UI/GuideDialog/SpeakerImage/";

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
        [SerializeField] private float showDuration = 5.0f;
        [SerializeField] private float fadeOutDuration = 1.0f;

        private readonly List<GameObject> panels = new List<GameObject>();
        private readonly List<RectTransform> panelRects = new List<RectTransform>();

        private readonly List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();

        private Image guideDialogImage;
        private Image guideDialogBackgroundImage;

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

        private void Start()
        {
            InactiveObjects();
            StartCoroutine(Execute());
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
            guideDialogBackgroundImage = panels[(int)GameObjects.GuideDialogBackgroundPanel].GetComponent<Image>();
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

            if (dialogPayload.dialogType != DialogType.Notify)
                return;
            if (dialogPayload.canvasType != DialogCanvasType.GuideDialog)
                return;

            var guidDialogInfo = GuideDialogInfo.Of(dialogPayload.speaker, dialogPayload.text, dialogPayload.imageName);
            dialogInfoQueue.Enqueue(guidDialogInfo);
        }

        private IEnumerator Execute()
        {
            WaitForEndOfFrame wfef = new WaitForEndOfFrame();
            while (true)
            {
                if (dialogInfoQueue.Any())
                {
                    var info = dialogInfoQueue.Dequeue();

                    yield return ShowGuideDialog(info);
                }

                yield return wfef;
            }
        }

        private IEnumerator ShowGuideDialog(GuideDialogInfo info)
        {
            string path = SpeakerImageBasePath + info.Image;
            var sprite = ResourceManager.Instance.LoadSprite(path);

            guideDialogImage.sprite = sprite;
            texts[(int)Texts.GuideDialogMessageText].text = info.Message;
            texts[(int)Texts.GuideDialogSpeakerText].text = info.Speaker;

            ActiveObjects();
            yield return new WaitForSeconds(showDuration);

            yield return FadeOutObjects();
        }


        private void ActiveObjects()
        {
            foreach (var panel in panels)
            {
                panel.SetActive(true);
            }

            foreach (var text in texts)
            {
                var color = text.color;
                color.a = 1.0f;
                text.color = color;
            }

            var imageColor = guideDialogImage.color;
            imageColor.a = 1.0f;
            guideDialogImage.color = imageColor;

            var backgroundColor = guideDialogBackgroundImage.color;
            backgroundColor.a = 1.0f;
            guideDialogBackgroundImage.color = backgroundColor;
        }

        private void InactiveObjects()
        {
            foreach (var panel in panels)
            {
                panel.SetActive(false);
            }
        }

        private IEnumerator FadeOutObjects()
        {
            WaitForEndOfFrame wfef = new WaitForEndOfFrame();
            float timeAcc = 0.0f;

            List<Color> textOriginColors = new List<Color>();
            texts.ForEach(t => textOriginColors.Add(t.color));

            Color imageColor = guideDialogImage.color;
            Color backgroundImageColor = guideDialogBackgroundImage.color;

            while (timeAcc <= fadeOutDuration)
            {
                yield return wfef;
                timeAcc += Time.deltaTime;

                float ratio = timeAcc / fadeOutDuration;

                for (int i = 0; i < texts.Count; i++)
                {
                    texts[i].color = FadeOutColor(textOriginColors[i], ratio);
                }

                guideDialogImage.color = FadeOutColor(imageColor, ratio);
                guideDialogBackgroundImage.color = FadeOutColor(backgroundImageColor, ratio);
            }

            InactiveObjects();
        }

        private Color FadeOutColor(Color origin, float ratio)
        {
            var st = origin;
            st.a = 1.0f;
            var en = origin;
            en.a = 0.0f;

            return Color.Lerp(st, en, ratio);
        }
    }
}