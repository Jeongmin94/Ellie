using System;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.PopupMenu;
using Assets.Scripts.Utils;
using Data.UI.Opening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Guide
{
    public class GuideButton : UIBase
    {
        public static readonly string Path = "Guide/GuideButton";
        public ButtonType GuideButtonType { get; set; }

        [SerializeField] private TextTypographyData colorData;

        private Image guidButtonImage;
        
        private Action<PopupPayload> guideButtonAction;

        public void Subscribe(Action<PopupPayload> listener)
        {
            guideButtonAction -= listener;
            guideButtonAction += listener;
        }

        public void InitButton()
        {
            Init();
        }

        protected override void Init()
        {
            Bind();
            InitObjects();
            BindEvents();
        }

        private void Bind()
        {
            guidButtonImage = gameObject.GetComponent<Image>();
        }

        private void InitObjects()
        {
            guidButtonImage.color = colorData.disabledColor;
        }

        private void BindEvents()
        {
            gameObject.BindEvent(OnPointerEnter, UIEvent.PointEnter);
            gameObject.BindEvent(OnPointerExit, UIEvent.PointExit);
            gameObject.BindEvent(OnPointerDown, UIEvent.Down);
            gameObject.BindEvent(OnPointerUp, UIEvent.Up);
            gameObject.BindEvent(OnPointerClick);
        }

        private void OnPointerEnter(PointerEventData data)
        {
            guidButtonImage.color = colorData.highlightedColor;
        }

        private void OnPointerExit(PointerEventData data)
        {
            guidButtonImage.color = colorData.disabledColor;
        }

        private void OnPointerDown(PointerEventData data)
        {
            guidButtonImage.color = colorData.pressedColor;
        }

        private void OnPointerUp(PointerEventData data)
        {
            guidButtonImage.color = colorData.disabledColor;
        }

        private void OnPointerClick(PointerEventData data)
        {
            guidButtonImage.color = colorData.highlightedColor;

            PopupPayload payload = new PopupPayload();
            payload.buttonType = GuideButtonType;
            guideButtonAction?.Invoke(payload);
        }
    }
}