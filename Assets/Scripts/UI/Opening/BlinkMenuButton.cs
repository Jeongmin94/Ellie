using System;
using System.Collections;
using Assets.Scripts.Managers;
using Managers.Sound;
using UI.Framework;
using UI.Opening.PopupMenu.PopupCanvas;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace UI.Opening
{
    public class BlinkMenuButton : OpeningText
    {
        public new static readonly string Path = "Opening/MenuText";

        private static readonly string SoundHover = "click1";
        private static readonly string SoundClick = "click2";

        [SerializeField] private float blinkInterval = 1.0f;
        private Action<PopupPayload> blinkMenuAction;

        private bool isBlink;

        public PopupType PopupType { get; set; }

        private void OnDestroy()
        {
            blinkMenuAction = null;
        }

        public void Subscribe(Action<PopupPayload> listener)
        {
            blinkMenuAction -= listener;
            blinkMenuAction += listener;
        }

        protected override void Init()
        {
            base.Init();

            var color = OriginColor();
            color.a = 0.0f;
            SetOriginColor(color);
            imageColor.Value = color;
        }

        protected override void BindEvents()
        {
            imagePanel.BindEvent(OnClickButton);
            imagePanel.BindEvent(OnPointerEnter, UIEvent.PointEnter);
            imagePanel.BindEvent(OnPointerExit, UIEvent.PointExit);
        }

        protected virtual void OnClickButton(PointerEventData data)
        {
            var payload = new PopupPayload();
            payload.popupType = PopupType;
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, SoundClick, Vector3.zero);
            blinkMenuAction?.Invoke(payload);
        }

        protected virtual void OnPointerEnter(PointerEventData data)
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, SoundHover, Vector3.zero);
            StartCoroutine(BlinkPanelImage());
        }

        protected virtual void OnPointerExit(PointerEventData data)
        {
            isBlink = false;
        }

        private IEnumerator BlinkPanelImage()
        {
            isBlink = true;
            var color = OriginColor();
            color.a = 0.0f;
            imageColor.Value = color;

            var toRight = true;
            var timeAcc = 0.0f;
            var wfef = new WaitForEndOfFrame();
            while (isBlink)
            {
                yield return wfef;

                var alpha = Mathf.Lerp(0.0f, 1.0f, timeAcc / blinkInterval);
                color.a = alpha;
                imageColor.Value = color;

                if (toRight)
                {
                    timeAcc += Time.deltaTime;
                }
                else
                {
                    timeAcc -= Time.deltaTime;
                }

                if (timeAcc > blinkInterval)
                {
                    toRight = false;
                }
                else if (timeAcc <= 0.0f)
                {
                    toRight = true;
                }
            }

            ResetImageColor();
        }
    }
}