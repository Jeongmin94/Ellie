using Assets.Scripts.ActionData;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.Utils;
using Data.UI.Opening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        [SerializeField] private Sprite panelSprite;

        protected GameObject imagePanel;
        private GameObject textPanel;

        private Image image;
        private Color originImageColor;

        private RectTransform rectTransform;
        private RectTransform imagePanelRect;
        private RectTransform textPanelRect;

        protected TextMeshProUGUI textMeshProUGUI;

        protected readonly Data<Color> imageColor = new Data<Color>();

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

            image = imagePanel.GetComponent<Image>();

            textMeshProUGUI = textPanel.FindChild<TextMeshProUGUI>(null, true);

            imageColor.Subscribe(OnImageColorChanged);
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

            var textRect = textMeshProUGUI.GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(textRect, AnchorPresets.StretchAll);
            textRect.sizeDelta = Vector2.zero;
            textRect.localPosition = Vector3.zero;

            image.sprite = panelSprite;
            originImageColor = image.color;
            imageColor.Value = originImageColor;
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

            if (typographyTypographyData.useOutline)
            {
                textMeshProUGUI.outlineColor = typographyTypographyData.outlineColor;
                textMeshProUGUI.outlineWidth = typographyTypographyData.outlineThickness;
            }
        }

        protected Rect GetRect() => rectTransform.rect;

        protected void SetOriginColor(Color color) => originImageColor = color;
        protected void ResetImageColor() => image.color = originImageColor;
        protected Color OriginColor() => originImageColor;

        protected void SetImageSprite(Sprite sprite) => image.sprite = sprite;
        protected void ResetImageSprite() => image.sprite = panelSprite;

        protected void SetImageAlpha(float value)
        {
            var color = imageColor.Value;
            color.a = value;
            imageColor.Value = color;
        }

        private void OnImageColorChanged(Color value)
        {
            image.color = value;
        }
    }
}