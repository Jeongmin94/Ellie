using System;
using Assets.Scripts.Managers;
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
        private static readonly string soundButton = "inven1";

        public ButtonType GuideButtonType { get; set; }
        public Image GuideButtonImage { get; set; }
        public bool IsActivated { get; set; } = false;

        [SerializeField] private TextTypographyData colorData;

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
            GuideButtonImage = gameObject.GetComponent<Image>();
        }

        private void InitObjects()
        {
            GuideButtonImage.color = colorData.disabledColor;
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
            GuideButtonImage.color = colorData.highlightedColor;
        }

        private void OnPointerExit(PointerEventData data)
        {
            GuideButtonImage.color = colorData.disabledColor;
        }

        private void OnPointerDown(PointerEventData data)
        {
            GuideButtonImage.color = colorData.pressedColor;
        }

        private void OnPointerUp(PointerEventData data)
        {
            GuideButtonImage.color = colorData.disabledColor;
        }

        private void OnPointerClick(PointerEventData data)
        {
            GuideButtonImage.color = colorData.highlightedColor;

            if (IsActivated)
                SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, soundButton, Vector3.zero);

            PopupPayload payload = new PopupPayload();
            payload.buttonType = GuideButtonType;
            guideButtonAction?.Invoke(payload);
        }
    }
}