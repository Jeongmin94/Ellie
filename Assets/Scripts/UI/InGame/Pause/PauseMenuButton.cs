using System;
using Assets.Scripts.Managers;
using Data.UI.Opening;
using Data.UI.Transform;
using Managers.Sound;
using UI.Framework;
using UI.Framework.Presets;
using UI.Inventory;
using UI.Opening;
using UI.Opening.PopupMenu.PopupCanvas;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace UI.InGame.Pause
{
    public class PauseMenuButton : OpeningText
    {
        public new static readonly string Path = "Pause/PauseMenuButton";

        private static readonly string SoundEnter = "pause2";
        private static readonly string SoundClick = "pause3";
        private static readonly string SoundEscape = "pause4";

        [SerializeField] private TextTypographyData hoverTypography;
        [SerializeField] private UITransformData hoverPanelTransform;
        [SerializeField] private Sprite hoverImage;

        private GameObject hoverPanel;
        private Image hoverPanelImage;
        private RectTransform hoverPanelRect;

        private Action<PopupPayload> pauseMenuButtonAction;

        private PopupType PauseButtonPopupType { get; set; }

        private void OnDestroy()
        {
            pauseMenuButtonAction = null;
        }

        public void InitPauseMenuButton(PopupType popupType, Action<PopupPayload> listener)
        {
            BindHoverPanel();
            InitHoverPanel();

            textMeshProUGUI.outlineColor = hoverTypography.outlineColor;
            textMeshProUGUI.outlineWidth = hoverTypography.outlineThickness;

            PauseButtonPopupType = popupType;

            pauseMenuButtonAction -= listener;
            pauseMenuButtonAction += listener;
        }

        private void BindHoverPanel()
        {
            Bind<GameObject>(typeof(HoverObject));

            hoverPanel = GetGameObject((int)HoverObject.HoverPanel);
            hoverPanelRect = hoverPanel.GetComponent<RectTransform>();
            hoverPanelImage = hoverPanel.GetComponent<Image>();
        }

        private void InitHoverPanel()
        {
            hoverPanelImage.sprite = hoverImage;

            AnchorPresets.SetAnchorPreset(hoverPanelRect, AnchorPresets.MiddleCenter);
            hoverPanelRect.sizeDelta = hoverPanelTransform.actionRect.Value.GetSize();
            hoverPanelRect.localScale = hoverPanelTransform.actionScale.Value;

            var rect = GetRect();
            var diffX = rect.width / 2.0f + hoverPanelTransform.actionRect.Value.width / 2.0f;
            var curPos = transform.localPosition;
            curPos.x -= diffX;
            hoverPanelRect.localPosition = curPos;

            hoverPanel.gameObject.SetActive(false);
        }

        protected override void BindEvents()
        {
            gameObject.BindEvent(OnButtonClicked);
            gameObject.BindEvent(OnPointerDown, UIEvent.Down);
            gameObject.BindEvent(OnPointerUp, UIEvent.Up);
            gameObject.BindEvent(OnPointerEnter, UIEvent.PointEnter);
            gameObject.BindEvent(OnPointerExit, UIEvent.PointExit);
        }


        private void OnButtonClicked(PointerEventData data)
        {
            if (PauseButtonPopupType == PopupType.Escape)
            {
                SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, SoundEscape, Vector3.zero);
            }
            else
            {
                SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, SoundClick, Vector3.zero);
            }

            var payload = new PopupPayload();
            payload.popupType = PauseButtonPopupType;
            pauseMenuButtonAction?.Invoke(payload);
        }

        private void OnPointerEnter(PointerEventData data)
        {
            if (PauseButtonPopupType == PopupType.Escape)
            {
                return;
            }

            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, SoundEnter, Vector3.zero);
            hoverPanel.gameObject.SetActive(true);
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
            hoverPanel.gameObject.SetActive(false);
        }

        private enum HoverObject
        {
            HoverPanel
        }
    }
}