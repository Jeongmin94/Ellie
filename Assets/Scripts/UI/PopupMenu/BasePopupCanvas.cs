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
    public enum PopupMenuType
    {
        Load,
        Start,
        Config,
        Exit
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
        [SerializeField] private TextTypographyData popupButtonTypography;

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
        protected TextMeshProUGUI popupText;
        protected readonly List<NormalMenuButton> popupButtons = new List<NormalMenuButton>();

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();

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

            AnchorPresets.SetAnchorPreset(popupButtonGridPanelRect, AnchorPresets.MiddleCenter);
            popupButtonGridPanelRect.sizeDelta = popupButtonGridTransform.actionRect.Value.GetSize();
            popupButtonGridPanelRect.localPosition = popupButtonGridTransform.actionRect.Value.ToCanvasPos();
            popupButtonGridPanelRect.localScale = popupButtonGridTransform.actionScale.Value;

            // 구현부에서 처리
            // popupText.font = popupButtonTypography.fontAsset;
            // popupText.fontSize = popupButtonTypography.fontSize;
            // popupText.alignment = popupButtonTypography.alignmentOptions;
            // popupText.lineSpacing = popupButtonTypography.lineSpacing;
            // popupText.text = popupButtonTypography.title;

            InitButtons();
        }

        private readonly List<string> buttonTexts = new List<string> { "예", "아니오" };

        private void InitButtons()
        {
            // yes, no
            for (int i = 0; i < buttonTexts.Count; i++)
            {
                var button = UIManager.Instance.MakeSubItem<NormalMenuButton>(popupButtonGridPanelRect, NormalMenuButton.Path);
                button.name += $"#{buttonTexts[i]}";
                button.InitText();
                popupButtonTypography.title = buttonTexts[i];
                button.InitTypography(popupButtonTypography);

                popupButtons.Add(button);
            }
        }

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