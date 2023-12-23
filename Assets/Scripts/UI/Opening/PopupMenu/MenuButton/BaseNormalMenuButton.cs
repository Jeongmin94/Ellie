using System;
using System.Collections;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Opening;
using Assets.Scripts.Utils;
using Data.UI.Opening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.PopupMenu
{
    public enum ButtonType
    {
        Yes,
        No
    }

    public class BaseNormalMenuButton : OpeningText
    {
        public new static readonly string Path = "Opening/NormalMenuText";

        [SerializeField] private Sprite hoverSprite;
        [SerializeField] private float blinkInterval = 0.5f;
        [SerializeField] private TextTypographyData hoverTypography;
        private IEnumerator blinkEnumerator;

        private Image hoverImage;
        private RectTransform hoverImageRect;

        private MenuButton menuButton;
        private Color originHoverImageColor;

        public void Subscribe(Action<PopupPayload> listener)
        {
            menuButton.Subscribe(listener);
        }

        public void InitMenuButton(ButtonType buttonType)
        {
            Bind<Image>(typeof(Images));
            hoverImage = GetImage((int)Images.HoverImage);
            hoverImageRect = hoverImage.GetComponent<RectTransform>();
            hoverImage.sprite = hoverSprite;
            originHoverImageColor = hoverImage.color;

            AnchorPresets.SetAnchorPreset(hoverImageRect, AnchorPresets.StretchAll);
            hoverImageRect.sizeDelta = Vector2.zero;
            hoverImageRect.localPosition = Vector3.zero;


            blinkEnumerator = BlinkImage();
            switch (buttonType)
            {
                case ButtonType.Yes:
                    menuButton = gameObject.GetOrAddComponent<YesMenuButton>();
                    break;
                case ButtonType.No:
                    menuButton = gameObject.GetOrAddComponent<NoMenuButton>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(buttonType), buttonType, null);
            }
        }

        protected override void BindEvents()
        {
            imagePanel.BindEvent(OnClickButton);
            imagePanel.BindEvent(OnPointerEnter, UIEvent.PointEnter);
            imagePanel.BindEvent(OnPointerDown, UIEvent.Down);
            imagePanel.BindEvent(OnPointerUp, UIEvent.Up);
            imagePanel.BindEvent(OnPointerExit, UIEvent.PointExit);
        }

        private void OnClickButton(PointerEventData data)
        {
            menuButton.Click();
        }

        private void OnPointerEnter(PointerEventData data)
        {
            StartCoroutine(blinkEnumerator);
        }

        private void OnPointerDown(PointerEventData data)
        {
            textMeshProUGUI.color = hoverTypography.pressedColor;
        }

        private void OnPointerUp(PointerEventData data)
        {
            textMeshProUGUI.color = hoverTypography.disabledColor;
        }

        private void OnPointerExit(PointerEventData data)
        {
            StopCoroutine(blinkEnumerator);
            hoverImage.color = originHoverImageColor;
        }

        private IEnumerator BlinkImage()
        {
            var isBlink = true;
            var color = originHoverImageColor;
            color.a = 1.0f;
            hoverImage.color = color;

            var toRight = true;
            var timeAcc = 0.0f;
            var wfef = new WaitForEndOfFrame();
            while (isBlink)
            {
                yield return wfef;

                var alpha = Mathf.Lerp(0.0f, 1.0f, timeAcc / blinkInterval);
                color.a = alpha;
                hoverImage.color = color;

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
        }

        private enum Images
        {
            HoverImage
        }
    }
}