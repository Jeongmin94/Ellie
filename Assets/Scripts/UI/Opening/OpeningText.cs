using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.Utils;
using Data.UI.Opening;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Opening
{
    public class OpeningText : UIBase
    {
        public static readonly string Path = "Opening/OpeningText";

        private enum GameObjects
        {
            ImagePanel,
            TextPanel,
        }

        protected GameObject imagePanel;
        private GameObject textPanel;

        private RectTransform rectTransform;
        private RectTransform imagePanelRect;
        private RectTransform textPanelRect;

        private TextMeshProUGUI textMeshProUGUI;

        public void InitText()
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
            Bind<GameObject>(typeof(GameObjects));

            imagePanel = GetGameObject((int)GameObjects.ImagePanel);
            textPanel = GetGameObject((int)GameObjects.TextPanel);

            rectTransform = gameObject.GetComponent<RectTransform>();
            imagePanelRect = imagePanel.GetComponent<RectTransform>();
            textPanelRect = textPanel.GetComponent<RectTransform>();

            textMeshProUGUI = textPanel.FindChild<TextMeshProUGUI>(null, true);
        }

        protected virtual void InitObjects()
        {
            AnchorPresets.SetAnchorPreset(rectTransform, AnchorPresets.StretchAll);
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.localPosition = Vector3.zero;

            AnchorPresets.SetAnchorPreset(imagePanelRect, AnchorPresets.StretchAll);
            imagePanelRect.sizeDelta = Vector2.zero;
            imagePanelRect.localPosition = Vector3.zero;

            AnchorPresets.SetAnchorPreset(textPanelRect, AnchorPresets.StretchAll);
            textPanelRect.sizeDelta = Vector2.zero;
            textPanelRect.localPosition = Vector3.zero;
        }

        protected virtual void BindEvents()
        {
        }

        public virtual void InitTypography(TextTypographyData typographyTypographyData)
        {
            textMeshProUGUI.font = typographyTypographyData.fontAsset;
            textMeshProUGUI.fontSize = typographyTypographyData.fontSize;
            textMeshProUGUI.lineSpacing = typographyTypographyData.lineSpacing;
            textMeshProUGUI.color = typographyTypographyData.color;
            textMeshProUGUI.alignment = typographyTypographyData.alignmentOptions;
            textMeshProUGUI.enableAutoSizing = typographyTypographyData.enableAutoSizing;
            textMeshProUGUI.text = typographyTypographyData.title;
        }
    }
}