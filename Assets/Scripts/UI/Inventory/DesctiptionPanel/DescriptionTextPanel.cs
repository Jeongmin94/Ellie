using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using Data.UI.Opening;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Inventory
{
    public class DescriptionTextPanel : UIBase
    {
        private enum Texts
        {
            DescriptionText,
        }

        private RectTransform rect;
        private TextMeshProUGUI descText;

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

            descText = GetText((int)Texts.DescriptionText);
        }

        private void InitObjects()
        {
            rect = GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(rect, AnchorPresets.MiddleCenter);
            rect.sizeDelta = InventoryConst.DescTextRect.GetSize();
            rect.localPosition = InventoryConst.DescTextRect.ToCanvasPos();
        }

        public void SetTypographyData(TextTypographyData data)
        {
            descText.color = data.color;
            descText.fontSize = data.fontSize;
            descText.alignment = data.alignmentOptions;
            descText.lineSpacing = data.lineSpacing;
            descText.characterSpacing = data.characterSpacing;

            descText.font = data.fontAsset;
        }

        public void SetDescriptionText(string text)
        {
            descText.text = text;
        }
    }
}