using System;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Opening;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

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

        private MenuButton menuButton;

        public void Subscribe(Action<PopupPayload> listener)
        {
            menuButton.Subscribe(listener);
        }

        public void InitMenuButton(ButtonType buttonType)
        {
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
            imagePanel.BindEvent(OnPointerExit, UIEvent.PointExit);
        }

        // 클릭하면 새로운 UI Popup
        private void OnClickButton(PointerEventData data)
        {
            Debug.Log($"{name} 버튼 클릭됨");
            menuButton.Click();
        }

        private void OnPointerEnter(PointerEventData data)
        {
            SetImageSprite(hoverSprite);
        }

        private void OnPointerExit(PointerEventData data)
        {
            ResetImageSprite();
        }
    }
}