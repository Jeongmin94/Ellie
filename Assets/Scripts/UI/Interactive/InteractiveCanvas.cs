using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Data.UI.Transform;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Inventory;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using Data.UI.Opening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Interactive
{
    public class InteractiveCanvas : UIPopup
    {
        private enum GameObjects
        {
            TextPanel,
            ImagePanel,
        }

        private enum Texts
        {
            InteractiveText
        }

        [SerializeField] private float interval;

        [SerializeField] private InteractiveType[] interactiveTypes;
        [SerializeField] private string[] interactiveNames;

        [SerializeField] private UITransformData[] panelTransforms;
        [SerializeField] private TextTypographyData[] typographyData;

        private readonly List<GameObject> panels = new List<GameObject>();
        private readonly List<RectTransform> rectTransforms = new List<RectTransform>();
        private readonly List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
        private Image interactiveImage;

        private bool onClosing;
        private Color textColor;
        private Color imageColor;

        private readonly IDictionary<InteractiveType, string> interactiveNameMap = new Dictionary<InteractiveType, string>();

        private TicketMachine ticketMachine;
        private IEnumerator closeEnumerator;

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
            for (int i = 0; i < gos.Length; i++)
            {
                var go = GetGameObject(i);
                panels.Add(go);
                rectTransforms.Add(go.GetComponent<RectTransform>());
            }

            var interactiveTexts = Enum.GetValues(typeof(Texts));
            for (int i = 0; i < interactiveTexts.Length; i++)
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
            for (int i = 0; i < panelTransforms.Length; i++)
            {
                AnchorPresets.SetAnchorPreset(rectTransforms[i], AnchorPresets.MiddleCenter);
                rectTransforms[i].sizeDelta = panelTransforms[i].actionRect.Value.GetSize();
                rectTransforms[i].localPosition = panelTransforms[i].actionRect.Value.ToCanvasPos();
                rectTransforms[i].localScale = panelTransforms[i].actionScale.Value;
            }

            for (int i = 0; i < typographyData.Length; i++)
            {
                SetTypography(texts[i], typographyData[i]);
            }

            for (int i = 0; i < interactiveTypes.Length; i++)
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
                return;

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
            float timeAcc = 0.0f;
            WaitForEndOfFrame wfef = new WaitForEndOfFrame();

            Color targetImageColor = imageColor;
            targetImageColor.a = 0.0f;
            Color targetTextColor = textColor;
            targetTextColor.a = 0.0f;

            while (timeAcc <= interval)
            {
                float ratio = timeAcc / interval;
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
    }
}