using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using Data.UI.Opening;
using Data.UI.Transform;
using Managers.Save;
using Managers.SceneLoad;
using Managers.Ticket;
using TMPro;
using UI.AlphaController;
using UI.Framework.Popup;
using UI.Framework.Presets;
using UI.Inventory;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Utils;

namespace UI.Death
{
    public class DeathCanvas : UIPopup
    {
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

        private readonly List<GameObject> panels = new();
        private readonly List<RectTransform> rectTransforms = new();
        private Image backgroundImage;
        private ColorAdjustments colorAdjustments;

        // grayscale
        private Volume deathCanvasVolume;

        private TextMeshProUGUI deathText;

        // fade-out
        private Image fadeOutImage;
        private Color fadeOutOriginColor;
        private ImageAlphaController imageAlphaController;

        private bool onPlayingDeath;

        private TextAlphaController textAlphaController;

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
        }

        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));
            Bind<TextMeshProUGUI>(typeof(Texts));
            Bind<Image>(typeof(Images));

            var gos = Enum.GetValues(typeof(GameObjects));
            for (var i = 0; i < gos.Length; i++)
            {
                var go = GetGameObject(i);
                panels.Add(go);
                rectTransforms.Add(go.GetComponent<RectTransform>());
            }

            deathCanvasVolume = panels[(int)GameObjects.VolumeProfile].GetComponent<Volume>();

            if (!deathCanvasVolume.profile.TryGet(out colorAdjustments))
            {
                Debug.LogError($"{name}의 Volume Profile 설정 오류 - type: {typeof(ColorAdjustments)}");
            }

            deathText = GetText((int)Texts.DeathText);
            backgroundImage = panels[(int)GameObjects.BackgroundPanel].GetComponent<Image>();

            textAlphaController = deathText.gameObject.GetOrAddComponent<TextAlphaController>();
            imageAlphaController = panels[(int)GameObjects.BackgroundPanel].GetOrAddComponent<ImageAlphaController>();

            fadeOutImage = GetImage((int)Images.FadeOutImage);
            fadeOutOriginColor = fadeOutImage.color;
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
            {
                return;
            }

            if (uiPayload.actionType == ActionType.OpenDeathCanvas)
            {
                if (!onPlayingDeath)
                {
                    gameObject.SetActive(true);

                    StartCoroutine(OpenDeathCanvas());
                }
            }
        }

        private IEnumerator OpenDeathCanvas()
        {
            onPlayingDeath = true;

            yield return StartCoroutine(imageAlphaController.ChangeAlpha(backgroundStartColor, backgroundTargetColor,
                backgroundAppearTime));

            StartCoroutine(textAlphaController.ChangeAlpha(textStartColor, textTargetColor, textAppearTime));

            yield return StartCoroutine(ToGray());

            yield return StartCoroutine(textAlphaController.ChangeAlpha(textTargetColor, textStartColor,
                textDisappearTime));

            yield return StartCoroutine(imageAlphaController.ChangeAlpha(backgroundTargetColor, backgroundStartColor,
                backgroundDisAppearTime));

            yield return StartCoroutine(FadeOut());

            // !TODO Load Data
            SaveLoadManager.Instance.IsLoadData = true;
            SceneLoadManager.Instance.LoadScene(SceneName.InGame);
            onPlayingDeath = false;
            backgroundImage.color = backgroundStartColor;
            deathText.color = textStartColor;
            fadeOutImage.color = fadeOutOriginColor;
            colorAdjustments.saturation.value = 0.0f;

            gameObject.SetActive(false);
        }

        private IEnumerator ToGray()
        {
            var timeAcc = 0.0f;
            var wfef = new WaitForEndOfFrame();

            var origin = 0.0f;
            var min = -100.0f;

            while (timeAcc <= grayScaleTime)
            {
                colorAdjustments.saturation.value = Mathf.Lerp(origin, min, timeAcc / grayScaleTime);
                yield return wfef;
                timeAcc += Time.deltaTime;
            }

            colorAdjustments.saturation.value = min;
        }

        private IEnumerator FadeOut()
        {
            var timeAcc = 0.0f;
            var wfef = new WaitForEndOfFrame();

            fadeOutImage.color = fadeOutOriginColor;

            var target = fadeOutOriginColor;
            target.a = 1.0f;

            while (timeAcc <= fadeOutTime)
            {
                fadeOutImage.color = Color.Lerp(fadeOutOriginColor, target, timeAcc / fadeOutTime);
                yield return wfef;
                timeAcc += Time.deltaTime;
            }

            fadeOutImage.color = target;
        }

        private enum GameObjects
        {
            BackgroundPanel,
            TextPanel,
            VolumeProfile
        }

        private enum Texts
        {
            DeathText
        }

        private enum Images
        {
            FadeOutImage
        }
    }
}