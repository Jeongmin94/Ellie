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

namespace Assets.Scripts.UI.InGame.AutoSave
{
    public class AutoSaveCanvas : UIPopup
    {
        public static readonly string Path = "AutoSaveCanvas";

        [SerializeField] private float rotateInterval;
        [SerializeField] private float fadeOut;

        private float minimumShowingDuration = 1.0f;
        private float currentShowingAcc = 0.0f;

        private enum GameObjects
        {
            IconPanel,
            TextPanel,
        }

        private enum Texts
        {
            AutoSaveText
        }

        [SerializeField] private UITransformData[] panelTransform;
        [SerializeField] private TextTypographyData typographyData;

        private readonly List<GameObject> panels = new List<GameObject>();
        private readonly List<RectTransform> rectTransforms = new List<RectTransform>();

        private Image autoSaveIcon;
        private RectTransform iconRect;
        private TextMeshProUGUI autoSaveText;

        private TicketMachine ticketMachine;

        private IEnumerator rotateEnumerator;
        private IEnumerator fadeOutEnumerator;

        private bool isRotating = false;

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
            for (int i = 0; i < gos.Length; i++)
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
            for (int i = 0; i < panelTransform.Length; i++)
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
                return;

            switch (uiPayload.actionType)
            {
                case ActionType.OpenAutoSave:
                {
                    if (isRotating)
                        return;

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
                yield return new WaitForSeconds(minimumShowingDuration - currentShowingAcc);

            float timeAcc = 0.0f;
            WaitForEndOfFrame wfef = new WaitForEndOfFrame();

            Color originIconColor = autoSaveIcon.color;
            Color originTextColor = autoSaveText.color;

            Color targetIconColor = autoSaveIcon.color;
            targetIconColor.a = 0.0f;
            Color targetTextColor = autoSaveText.color;
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
                yield break;

            isRotating = true;
            float rotationSpeed = 360.0f / rotateInterval;
            WaitForEndOfFrame wfef = new WaitForEndOfFrame();

            while (true)
            {
                rectTransforms[(int)GameObjects.IconPanel].Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
                yield return wfef;
                currentShowingAcc += Time.deltaTime;
            }
        }

        private void OnSaveAction()
        {
            UIPayload payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.OpenAutoSave;

            OnNotify(payload);
        }

        private void OnSaveDoneAction()
        {
            UIPayload payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.CloseAutoSave;

            OnNotify(payload);
        }
    }
}