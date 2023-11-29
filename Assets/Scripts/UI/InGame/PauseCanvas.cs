using System.Collections.Generic;
using Assets.Scripts.Data.UI.Transform;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Inventory;
using Assets.Scripts.UI.PopupMenu;
using Data.UI.Opening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
    public class PauseCanvas : UIPopup
    {
        private enum GameObjects
        {
            ButtonPanel,
            EscapePanel,
        }

        private enum Images
        {
            EscapeImage,
        }

        [SerializeField] private UITransformData buttonPanelTransformData;
        [SerializeField] private UITransformData escapeTransformData;
        [SerializeField] private UITransformData escapeImageTransformData;

        [SerializeField] private TextTypographyData pauseMenuTypographyData;
        [SerializeField] private TextTypographyData escapeTypographyData;

        [Header("메뉴 이름")] [SerializeField] private string[] menuTitles;
        [Header("팝업 타입")] [SerializeField] private PopupType[] popupTypes;

        private GameObject buttonPanel;
        private GameObject escapePanel;

        private RectTransform buttonPanelRect;
        private RectTransform escapePanelRect;
        private RectTransform escapeImageRect;

        private Image escapeImage;

        private readonly List<PauseMenuButton> menuButtons = new List<PauseMenuButton>();

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
            Bind<Image>(typeof(Images));

            buttonPanel = GetGameObject((int)GameObjects.ButtonPanel);
            escapePanel = GetGameObject((int)GameObjects.EscapePanel);

            escapeImage = GetImage((int)Images.EscapeImage);

            buttonPanelRect = buttonPanel.GetComponent<RectTransform>();
            escapePanelRect = escapePanel.GetComponent<RectTransform>();
            escapeImageRect = escapeImage.GetComponent<RectTransform>();
        }

        private void InitObjects()
        {
            AnchorPresets.SetAnchorPreset(buttonPanelRect, AnchorPresets.MiddleCenter);
            buttonPanelRect.sizeDelta = buttonPanelTransformData.actionRect.Value.GetSize();
            buttonPanelRect.localPosition = buttonPanelTransformData.actionRect.Value.ToCanvasPos();
            buttonPanelRect.localScale = buttonPanelTransformData.actionScale.Value;

            AnchorPresets.SetAnchorPreset(escapePanelRect, AnchorPresets.MiddleCenter);
            escapePanelRect.sizeDelta = escapeTransformData.actionRect.Value.GetSize();
            escapePanelRect.localPosition = escapeTransformData.actionRect.Value.ToCanvasPos();
            escapePanelRect.localScale = escapeTransformData.actionScale.Value;

            AnchorPresets.SetAnchorPreset(escapeImageRect, AnchorPresets.MiddleCenter);
            escapeImageRect.sizeDelta = escapeImageTransformData.actionRect.Value.GetSize();
            escapeImageRect.localPosition = escapeImageTransformData.actionRect.Value.ToCanvasPos();
            escapeImageRect.localScale = escapeTransformData.actionScale.Value;

            InitPauseMenuButtons();
        }

        private void InitPauseMenuButtons()
        {
            // menu
            int idx = 0;
            foreach (var title in menuTitles)
            {
                var button = UIManager.Instance.MakeSubItem<PauseMenuButton>(buttonPanelRect, PauseMenuButton.Path);

                pauseMenuTypographyData.title = title;
                button.name += $"#{title}";
                button.InitText();
                button.InitTypography(pauseMenuTypographyData);
                button.InitPauseMenuButton(popupTypes[idx++]);
                menuButtons.Add(button);
            }

            // escape
            var escapeButton = UIManager.Instance.MakeSubItem<PauseMenuButton>(escapePanelRect, PauseMenuButton.Path);
            escapeButton.name += $"#{escapeTypographyData.title}";
            escapeButton.InitText();
            escapeButton.InitTypography(escapeTypographyData);
            escapeButton.InitPauseMenuButton(PopupType.Escape);
        }

        private void OnButtonClicked(PopupPayload payload)
        {
            if (payload.buttonType == ButtonType.No)
            {
                gameObject.SetActive(false);
            }
        }
    }
}