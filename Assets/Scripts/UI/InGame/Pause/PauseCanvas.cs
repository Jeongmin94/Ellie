using System;
using System.Collections.Generic;
using Assets.Scripts.Centers;
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

        private static readonly string SoundOpen = "pause1";


        [SerializeField] private UITransformData buttonPanelTransformData;
        [SerializeField] private UITransformData escapeTransformData;
        [SerializeField] private UITransformData escapeImageTransformData;

        [SerializeField] private TextTypographyData pauseMenuTypographyData;
        [SerializeField] private TextTypographyData escapeTypographyData;

        [Header("팝업 타입")][SerializeField] private PopupType[] popupTypes;

        [SerializeField] private string[] buttonNames;

        private GameObject buttonPanel;
        private GameObject escapePanel;

        private RectTransform buttonPanelRect;
        private RectTransform escapePanelRect;
        private RectTransform escapeImageRect;

        private Image escapeImage;

        private ConfigCanvas configCanvas;

        private readonly IDictionary<PopupType, PauseMenuButton> menuButtonMap = new Dictionary<PopupType, PauseMenuButton>();
        private readonly IDictionary<PopupType, BasePopupCanvas> popupCanvasMap = new Dictionary<PopupType, BasePopupCanvas>();

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();

            InputManager.Instance.Subscribe(InputType.Escape, OnEscapeAction);

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

            configCanvas = UIManager.Instance.MakePopup<ConfigCanvas>(ConfigCanvas.Path);
            configCanvas.configCanvasAction -= OnPopupCanvasAction;
            configCanvas.configCanvasAction += OnPopupCanvasAction;

            Color color = configCanvas.Background.color;
            color.a = 0.7f;
            configCanvas.Background.color = color;

            configCanvas.gameObject.SetActive(false);

            InitPopupCanvas();
            InitPauseMenuButtons();
        }

        private void InitPopupCanvas()
        {
            foreach (var type in popupTypes)
            {
                var popup = UIManager.Instance.MakePopup<BasePopupCanvas>(BasePopupCanvas.Path);
                popup.InitPopupCanvas(type);
                popup.Subscribe(OnPopupCanvasAction);
                popup.name += $"#{type}";
                popup.gameObject.SetActive(false);

                popupCanvasMap[type] = popup;
            }
        }

        private void InitPauseMenuButtons()
        {
            // menu
            for (int i = 0; i < buttonNames.Length; i++)
            {
                var button = UIManager.Instance.MakeSubItem<PauseMenuButton>(buttonPanelRect, PauseMenuButton.Path);

                string title = buttonNames[i];
                pauseMenuTypographyData.title = title;
                button.name += $"#{title}";
                button.InitText();
                button.InitTypography(pauseMenuTypographyData);
                button.InitPauseMenuButton(popupTypes[i], OnButtonClicked);

                menuButtonMap[popupTypes[i]] = button;
            }

            // escape
            var escapeButton = UIManager.Instance.MakeSubItem<PauseMenuButton>(escapePanelRect, PauseMenuButton.Path);
            escapeButton.name += $"#{escapeTypographyData.title}";
            escapeButton.InitText();
            escapeButton.InitTypography(escapeTypographyData);
            escapeButton.InitPauseMenuButton(PopupType.Escape, OnButtonClicked);

            menuButtonMap[PopupType.Escape] = escapeButton;
        }

        private void OnEscapeAction()
        {
            if (!gameObject.activeSelf)
            {
                SoundManager.Instance.PlaySound(SoundManager.SoundType.UISfx, SoundOpen);
                gameObject.SetActive(true);
            }
            else
            {
                bool allPopupClosed = true;
                foreach (var popup in popupCanvasMap.Values)
                {
                    if (popup.gameObject.activeSelf)
                        allPopupClosed = false;
                }

                if (configCanvas.gameObject.activeSelf)
                    allPopupClosed = false;

                if (allPopupClosed)
                    gameObject.SetActive(false);
            }
        }

        private void OnButtonClicked(PopupPayload payload)
        {
            if (payload.popupType == PopupType.Config)
            {
                configCanvas.gameObject.SetActive(true);
            }
            else if (payload.popupType == PopupType.Escape)
            {
                gameObject.SetActive(false);
            }
            else
            {
                popupCanvasMap[payload.popupType].gameObject.SetActive(true);
            }
        }

        private void OnPopupCanvasAction(PopupPayload payload)
        {
            switch (payload.buttonType)
            {
                case ButtonType.Yes:
                    {
                        if (payload.popupType == PopupType.Load)
                        {
                            SaveLoadManager.Instance.IsLoadData = true;
                            SceneLoadManager.Instance.LoadScene(SceneName.InGame);
                        }
                        // !TODO
                    }
                    break;

                case ButtonType.No:
                    {
                        if (payload.popupType == PopupType.Config)
                        {
                            configCanvas.gameObject.SetActive(false);
                        }
                        else
                        {
                            popupCanvasMap[payload.popupType].gameObject.SetActive(false);
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}