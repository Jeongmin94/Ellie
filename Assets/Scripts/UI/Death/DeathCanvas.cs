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

namespace Assets.Scripts.UI.Death
{
    public class DeathCanvas : UIPopup
    {
        private enum GameObjects
        {
            BackgroundPanel,
            TextPanel,
        }

        private enum Texts
        {
            DeathText,
        }

        [SerializeField] private float grayScaleTime;
        [SerializeField] private float fadeOutTime;

        [SerializeField] private Color backgroundStartColor;
        [SerializeField] private Color backgroundTargetColor;
        [SerializeField] private Color textStartColor;
        [SerializeField] private Color textTargetColor;

        [SerializeField] private float backgroundAppearTime;
        [SerializeField] private float backgroundDisAppearTime;
        [SerializeField] private float textAppearTime;
        [SerializeField] private float textDisappearTime;

        [SerializeField] private UITransformData[] panelTransforms;
        [SerializeField] private TextTypographyData deathTextTypography;

        private readonly List<GameObject> panels = new List<GameObject>();
        private readonly List<RectTransform> rectTransforms = new List<RectTransform>();

        private TextMeshProUGUI deathText;
        private Image backgroundImage;

        private TicketMachine ticketMachine;

        private TextAlphaController textAlphaController;
        private ImageAlphaController imageAlphaController;

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
                panels.Add(go);
                rectTransforms.Add(go.GetComponent<RectTransform>());
            }

            deathText = GetText((int)Texts.DeathText);
            backgroundImage = panels[(int)GameObjects.BackgroundPanel].GetComponent<Image>();

            textAlphaController = deathText.gameObject.GetOrAddComponent<TextAlphaController>();
            imageAlphaController = panels[(int)GameObjects.BackgroundPanel].GetOrAddComponent<ImageAlphaController>();
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

            deathText.text = deathTextTypography.title;
            deathText.font = deathTextTypography.fontAsset;
            deathText.fontSize = deathTextTypography.fontSize;
            deathText.alignment = deathTextTypography.alignmentOptions;
            deathText.lineSpacing = deathTextTypography.lineSpacing;

            backgroundImage.color = backgroundStartColor;
            deathText.color = textStartColor;

            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTicket(ChannelType.UI);
            ticketMachine.RegisterObserver(ChannelType.UI, OnNotify);

            TicketManager.Instance.Ticket(ticketMachine);
        }

        private void OnNotify(IBaseEventPayload payload)
        {
            if (payload is not UIPayload uiPayload)
                return;

            if (uiPayload.actionType == ActionType.OpenDeathCanvas)
            {
                StartCoroutine(OpenDeathCanvas());
            }
        }

        private IEnumerator OpenDeathCanvas()
        {
            yield return StartCoroutine(imageAlphaController.ChangeAlpha(backgroundStartColor, backgroundTargetColor, backgroundAppearTime));

            yield return StartCoroutine(textAlphaController.ChangeAlpha(textStartColor, textTargetColor, textAppearTime));
        }
    }
}