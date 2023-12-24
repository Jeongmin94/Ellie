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

namespace UI.InGame.AutoSave
{
    public class AutoSaveCanvas : UIPopup
    {
        public static readonly string Path = "AutoSaveCanvas";

        [SerializeField] private float rotateInterval;
        [SerializeField] private float fadeOut;

        [SerializeField] private UITransformData[] panelTransform;
        [SerializeField] private TextTypographyData typographyData;

        private readonly List<GameObject> panels = new();
        private readonly List<RectTransform> rectTransforms = new();

        private Image autoSaveIcon;
        private TextMeshProUGUI autoSaveText;
        private float currentShowingAcc;
        private IEnumerator fadeOutEnumerator;
        private RectTransform iconRect;

        private bool isRotating;

        private readonly float minimumShowingDuration = 1.0f;

        private IEnumerator rotateEnumerator;

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

            SaveLoadManager.Instance.SubscribeSaveEvent(OnSaveAction);
            SaveLoadManager.Instance.saveDoneAction -= OnSaveDoneAction;
            SaveLoadManager.Instance.saveDoneAction += OnSaveDoneAction;

            gameObject.SetActive(false);
        }

        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));
            Bind<TextMeshProUGUI>(typeof(Texts));

            var gos = Enum.GetValues(typeof(GameObjects));
            for (var i = 0; i < gos.Length; i++)
            {
                var go = GetGameObject(i);
                rectTransforms.Add(go.GetComponent<RectTransform>());
                panels.Add(go);
            }

            autoSaveIcon = panels[(int)GameObjects.IconPanel].GetComponent<Image>();
            autoSaveText = GetText((int)Texts.AutoSaveText);
        }

        private void InitObjects()
        {
            for (var i = 0; i < panelTransform.Length; i++)
            {
                AnchorPresets.SetAnchorPreset(rectTransforms[i], AnchorPresets.MiddleCenter);
                rectTransforms[i].sizeDelta = panelTransform[i].actionRect.Value.GetSize();
                rectTransforms[i].localPosition = panelTransform[i].actionRect.Value.ToCanvasPos();
                rectTransforms[i].localScale = panelTransform[i].actionScale.Value;
            }

            autoSaveText.text = typographyData.title;
            autoSaveText.font = typographyData.fontAsset;
            autoSaveText.fontSize = typographyData.fontSize;
            autoSaveText.color = typographyData.color;
            autoSaveText.alignment = typographyData.alignmentOptions;
            autoSaveText.lineSpacing = typographyData.lineSpacing;

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
                case ActionType.OpenAutoSave:
                {
                    if (isRotating)
                    {
                        return;
                    }

                    gameObject.SetActive(true);
                    rotateEnumerator = RotateIcon();
                    StartCoroutine(rotateEnumerator);
                }
                    break;

                case ActionType.CloseAutoSave:
                {
                    StartCoroutine(FadeOut());
                }
                    break;
            }
        }

        private IEnumerator FadeOut()
        {
            if (currentShowingAcc < minimumShowingDuration)
            {
                yield return new WaitForSeconds(minimumShowingDuration - currentShowingAcc);
            }

            var timeAcc = 0.0f;
            var wfef = new WaitForEndOfFrame();

            var originIconColor = autoSaveIcon.color;
            var originTextColor = autoSaveText.color;

            var targetIconColor = autoSaveIcon.color;
            targetIconColor.a = 0.0f;
            var targetTextColor = autoSaveText.color;
            targetTextColor.a = 0.0f;

            while (timeAcc < fadeOut)
            {
                autoSaveIcon.color = Color.Lerp(originIconColor, targetIconColor, timeAcc / fadeOut);
                autoSaveText.color = Color.Lerp(originTextColor, targetTextColor, timeAcc / fadeOut);
                yield return wfef;
                timeAcc += Time.deltaTime;
            }

            StopCoroutine(rotateEnumerator);
            gameObject.SetActive(false);

            isRotating = false;
            autoSaveIcon.color = originIconColor;
            autoSaveText.color = originTextColor;
        }

        private IEnumerator RotateIcon()
        {
            currentShowingAcc = 0.0f;
            if (rotateInterval <= 0.0f)
            {
                yield break;
            }

            isRotating = true;
            var rotationSpeed = 360.0f / rotateInterval;
            var wfef = new WaitForEndOfFrame();

            while (true)
            {
                rectTransforms[(int)GameObjects.IconPanel].Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
                yield return wfef;
                currentShowingAcc += Time.deltaTime;
            }
        }

        private void OnSaveAction()
        {
            var payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.OpenAutoSave;

            OnNotify(payload);
        }

        private void OnSaveDoneAction()
        {
            var payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.CloseAutoSave;

            OnNotify(payload);
        }

        private enum GameObjects
        {
            IconPanel,
            TextPanel
        }

        private enum Texts
        {
            AutoSaveText
        }
    }
}