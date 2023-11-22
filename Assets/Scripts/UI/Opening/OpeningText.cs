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

        private GameObject imagePanel;
        private GameObject textPanel;

        private RectTransform rectTransform;
        private RectTransform imagePanelRect;
        private RectTransform textPanelRect;

        private TextMeshProUGUI textMeshProUGUI;

        public void InitOpeningText()
        {
            Init();
        }

        protected override void Init()
        {
            Bind();
            InitObjects();
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

        private void InitObjects()
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

        public void InitTypography(OpeningTextData typographyData)
        {
            textMeshProUGUI.font = typographyData.fontAsset;
            textMeshProUGUI.fontSize = typographyData.fontSize;
            textMeshProUGUI.lineSpacing = typographyData.lineSpacing;
            textMeshProUGUI.color = typographyData.color;
            textMeshProUGUI.alignment = typographyData.alignmentOptions;
            textMeshProUGUI.enableAutoSizing = typographyData.enableAutoSizing;
        }
    }
}