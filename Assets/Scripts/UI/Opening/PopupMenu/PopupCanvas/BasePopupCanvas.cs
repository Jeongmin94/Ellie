using System;
using System.Collections.Generic;
using Assets.Scripts.Data.UI.Transform;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Inventory;
using Assets.Scripts.Utils;
using Data.UI.Opening;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.PopupMenu
{
    public enum PopupType
    {
        Load,
        Start,
        Config,
        Exit,
        Main,
        Escape
    }

    public struct PopupPayload
    {
        public PopupType popupType;
        public ConfigType configType;
        public ButtonType buttonType;

        public bool isOn;
    }

    public class BasePopupCanvas : UIPopup
    {
        public static readonly string Path = "PopupMenuCanvas";

        private enum GameObjects
        {
            PopupBackground,
            PopupTextPanel,
            PopupButtonGridPanel
        }

        [SerializeField] private UITransformData popupMenuTransform;
        [SerializeField] private UITransformData popupTextPanelTransform;
        [SerializeField] private UITransformData popupButtonGridTransform;
        [SerializeField] private TextTypographyData popupTitleTypography;
        [SerializeField] private TextTypographyData popupButtonTypography;
        [SerializeField] private PopupData popupData;

        private readonly TransformController popupBackgroundController = new TransformController();
        private readonly TransformController popupTextPanelController = new TransformController();
        private readonly TransformController popupButtonGridController = new TransformController();

        private GameObject popupBackground;
        private GameObject popupTextPanel;
        private GameObject popupButtonGridPanel;

        private RectTransform popupBackgroundRect;
        private RectTransform popupTextPanelRect;
        private RectTransform popupButtonGridPanelRect;

        // 팝업 내용
        private TextMeshProUGUI popupText;
        private readonly List<BaseNormalMenuButton> popupButtons = new List<BaseNormalMenuButton>();

        private PopupType popupType;
        private PopupCanvas popupCanvas;

        public void Subscribe(Action<PopupPayload> listener)
        {
            popupCanvas.Subscribe(listener);
        }

        private void OnEnable()
        {
            popupButtons.ForEach(button => button.gameObject.SetActive(true));
        }

        private void OnDisable()
        {
            popupButtons.ForEach(button => button.gameObject.SetActive(false));
        }

        public void InitPopupCanvas(PopupType type)
        {
            base.Init();

            Init();

            popupType = type;
            SetTitle(popupData.GetTitle(type));
            popupCanvas = gameObject.AddPopupCanvas(type);
        }

        protected override void Init()
        {
            Bind();
            InitGameObjects();
        }

        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));

            popupBackground = GetGameObject((int)GameObjects.PopupBackground);
            popupTextPanel = GetGameObject((int)GameObjects.PopupTextPanel);
            popupButtonGridPanel = GetGameObject((int)GameObjects.PopupButtonGridPanel);

            popupBackgroundRect = popupBackground.GetComponent<RectTransform>();
            popupTextPanelRect = popupTextPanel.GetComponent<RectTransform>();
            popupButtonGridPanelRect = popupButtonGridPanel.GetComponent<RectTransform>();

            popupText = popupTextPanel.FindChild<TextMeshProUGUI>(null, true);
        }

        private void InitGameObjects()
        {
#if UNITY_EDITOR
            popupMenuTransform.actionRect.Subscribe(popupBackgroundController.OnRectChange);
            popupMenuTransform.actionScale.Subscribe(popupBackgroundController.OnScaleChange);

            popupTextPanelTransform.actionRect.Subscribe(popupTextPanelController.OnRectChange);
            popupTextPanelTransform.actionScale.Subscribe(popupTextPanelController.OnScaleChange);

            popupButtonGridTransform.actionRect.Subscribe(popupButtonGridController.OnRectChange);
            popupButtonGridTransform.actionScale.Subscribe(popupButtonGridController.OnScaleChange);
#endif
            AnchorPresets.SetAnchorPreset(popupBackgroundRect, AnchorPresets.MiddleCenter);
            popupBackgroundRect.sizeDelta = popupMenuTransform.actionRect.Value.GetSize();
            popupBackgroundRect.localPosition = popupMenuTransform.actionRect.Value.ToCanvasPos();
            popupBackgroundRect.localScale = popupMenuTransform.actionScale.Value;

            AnchorPresets.SetAnchorPreset(popupTextPanelRect, AnchorPresets.MiddleCenter);
            popupTextPanelRect.sizeDelta = popupTextPanelTransform.actionRect.Value.GetSize();
            popupTextPanelRect.localPosition = popupTextPanelTransform.actionRect.Value.ToCanvasPos();
            popupTextPanelRect.localScale = popupTextPanelTransform.actionScale.Value;

            var textRect = popupText.GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(textRect, AnchorPresets.StretchAll);

            AnchorPresets.SetAnchorPreset(popupButtonGridPanelRect, AnchorPresets.MiddleCenter);
            popupButtonGridPanelRect.sizeDelta = popupButtonGridTransform.actionRect.Value.GetSize();
            popupButtonGridPanelRect.localPosition = popupButtonGridTransform.actionRect.Value.ToCanvasPos();
            popupButtonGridPanelRect.localScale = popupButtonGridTransform.actionScale.Value;

            InitButtons();
            SetPopupTitleTypography();
        }

        private readonly List<string> buttonTexts = new List<string> { "예", "아니오" };
        private readonly List<ButtonType> buttonTypes = new List<ButtonType>() { ButtonType.Yes, ButtonType.No };

        private void InitButtons()
        {
            // yes, no
            for (int i = 0; i < buttonTexts.Count; i++)
            {
                var button = UIManager.Instance.MakeSubItem<BaseNormalMenuButton>(popupButtonGridPanelRect, BaseNormalMenuButton.Path);
                button.name += $"#{buttonTexts[i]}";
                button.InitText();
                popupButtonTypography.title = buttonTexts[i];
                button.InitTypography(popupButtonTypography);
                button.InitMenuButton(buttonTypes[i]);
                button.Subscribe(OnButtonClicked);

                button.gameObject.SetActive(false);

                popupButtons.Add(button);
            }
        }

        private void SetPopupTitleTypography()
        {
            popupText.font = popupTitleTypography.fontAsset;
            popupText.fontSize = popupTitleTypography.fontSize;
            popupText.alignment = popupTitleTypography.alignmentOptions;
            popupText.lineSpacing = popupTitleTypography.lineSpacing;
        }

        private void SetTitle(string title) => popupText.text = title;

        #region PopupEvent

        private void OnButtonClicked(PopupPayload payload)
        {
            payload.popupType = popupType;
            popupCanvas.Invoke(payload);
        }

        #endregion

        private void LateUpdate()
        {
#if UNITY_EDITOR
            popupBackgroundController.CheckQueue(popupBackgroundRect);
            popupTextPanelController.CheckQueue(popupTextPanelRect);
            popupButtonGridController.CheckQueue(popupButtonGridPanelRect);
#endif
        }
    }
}