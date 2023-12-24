using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using Data.UI.Opening;
using Data.UI.Transform;
using TMPro;
using UI.Framework.Popup;
using UI.Framework.Presets;
using UI.Inventory;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Interactive
{
    public class InteractiveCanvas : UIPopup
    {
        [SerializeField] private float interval;

        [SerializeField] private InteractiveType[] interactiveTypes;
        [SerializeField] private string[] interactiveNames;

        [SerializeField] private UITransformData[] panelTransforms;
        [SerializeField] private TextTypographyData[] typographyData;

        private readonly IDictionary<InteractiveType, string> interactiveNameMap =
            new Dictionary<InteractiveType, string>();

        private readonly List<GameObject> panels = new();
        private readonly List<RectTransform> rectTransforms = new();
        private readonly List<TextMeshProUGUI> texts = new();
        private IEnumerator closeEnumerator;
        private Color imageColor;
        private Image interactiveImage;

        private bool onClosing;
        private Color textColor;

        private TicketMachine ticketMachine;

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        protected override void Init()
        {
            base.Init();

            Bind();
            InitObjects();
            InitTicketMachine();

            onClosing = false;
        }

        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));
            Bind<TextMeshProUGUI>(typeof(Texts));

            var gos = Enum.GetValues(typeof(GameObjects));
            for (var i = 0; i < gos.Length; i++)
            {
                var go = GetGameObject(i);
                panels.Add(go);
                rectTransforms.Add(go.GetComponent<RectTransform>());
            }

            var interactiveTexts = Enum.GetValues(typeof(Texts));
            for (var i = 0; i < interactiveTexts.Length; i++)
            {
                var t = GetText(i);
                texts.Add(t);
                textColor = t.color;
            }

            interactiveImage = panels[(int)GameObjects.ImagePanel].GetComponent<Image>();
            imageColor = interactiveImage.color;

            closeEnumerator = CloseCanvas();
        }

        private void InitObjects()
        {
            for (var i = 0; i < panelTransforms.Length; i++)
            {
                AnchorPresets.SetAnchorPreset(rectTransforms[i], AnchorPresets.MiddleCenter);
                rectTransforms[i].sizeDelta = panelTransforms[i].actionRect.Value.GetSize();
                rectTransforms[i].localPosition = panelTransforms[i].actionRect.Value.ToCanvasPos();
                rectTransforms[i].localScale = panelTransforms[i].actionScale.Value;
            }

            for (var i = 0; i < typographyData.Length; i++)
            {
                SetTypography(texts[i], typographyData[i]);
            }

            for (var i = 0; i < interactiveTypes.Length; i++)
            {
                interactiveNameMap.TryAdd(interactiveTypes[i], interactiveNames[i]);
            }
        }

        private void InitTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTicket(ChannelType.UI);
            ticketMachine.RegisterObserver(ChannelType.UI, OnNotify);

            TicketManager.Instance.Ticket(ticketMachine);
        }

        private void OnNotify(IBaseEventPayload payload)
        {
            if (payload is not UIPayload uiPayload)
            {
                return;
            }

            switch (uiPayload.actionType)
            {
                case ActionType.PopupInteractive:
                {
                    interactiveImage.color = imageColor;
                    texts[(int)Texts.InteractiveText].color = textColor;

                    texts[(int)Texts.InteractiveText].text = interactiveNameMap[uiPayload.interactiveType];
                    gameObject.SetActive(true);
                }
                    break;

                case ActionType.CloseInteractive:
                {
                    if (gameObject.activeSelf && !onClosing)
                    {
                        closeEnumerator = CloseCanvas();
                        StartCoroutine(closeEnumerator);
                    }
                }
                    break;
            }
        }

        private void SetTypography(TextMeshProUGUI tmp, TextTypographyData data)
        {
            tmp.font = data.fontAsset;
            tmp.fontSize = data.fontSize;
            tmp.alignment = data.alignmentOptions;
            tmp.lineSpacing = data.lineSpacing;
            tmp.color = data.color;
        }

        private IEnumerator CloseCanvas()
        {
            onClosing = true;
            var timeAcc = 0.0f;
            var wfef = new WaitForEndOfFrame();

            var targetImageColor = imageColor;
            targetImageColor.a = 0.0f;
            var targetTextColor = textColor;
            targetTextColor.a = 0.0f;

            while (timeAcc <= interval)
            {
                var ratio = timeAcc / interval;
                interactiveImage.color = Color.Lerp(imageColor, targetImageColor, ratio);
                texts[(int)Texts.InteractiveText].color = Color.Lerp(textColor, targetTextColor, ratio);

                yield return wfef;
                timeAcc += Time.deltaTime;
            }

            interactiveImage.color = targetImageColor;
            texts[(int)Texts.InteractiveText].color = targetTextColor;

            onClosing = false;
            gameObject.SetActive(false);
        }

        private enum GameObjects
        {
            TextPanel,
            ImagePanel
        }

        private enum Texts
        {
            InteractiveText
        }
    }
}