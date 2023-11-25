using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using Data.UI.Opening;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Inventory
{
    public class DescriptionNamePanel : UIBase
    {
        private enum Texts
        {
            DescriptionNameText
        }

        private RectTransform rect;
        private TextMeshProUGUI descNameText;

        private readonly Color fontColor = new Color(217, 209, 209);
        private readonly int fontSize = 31;
        private readonly float lineHeight = 79.95f;

        private void Awake()
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
            Bind<TextMeshProUGUI>(typeof(Texts));

            descNameText = GetText((int)Texts.DescriptionNameText);
        }

        // !TODO: 텍스트 폰트 설정(폰트가 미정)
        private void InitObjects()
        {
            rect = GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(rect, AnchorPresets.MiddleCenter);
            rect.sizeDelta = InventoryConst.DescNameRect.GetSize();
            rect.localPosition = InventoryConst.DescNameRect.ToCanvasPos();

            descNameText.color = fontColor;
            descNameText.fontSize = fontSize;
            descNameText.alignment = TextAlignmentOptions.Top;
            descNameText.lineSpacing = lineHeight;
        }

        public void SetTypographyData(TextTypographyData data)
        {
            descNameText.font = data.fontAsset;
            descNameText.color = data.color;
            descNameText.fontSize = data.fontSize;
            descNameText.alignment = data.alignmentOptions;
            descNameText.lineSpacing = data.lineSpacing;
            descNameText.text = data.title;
        }

        public void SetDescriptionName(string descName)
        {
            descNameText.text = descName;
        }
    }
}