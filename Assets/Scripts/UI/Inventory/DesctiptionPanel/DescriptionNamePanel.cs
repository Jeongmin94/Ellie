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

        private void InitObjects()
        {
            rect = GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(rect, AnchorPresets.MiddleCenter);
            rect.sizeDelta = InventoryConst.DescNameRect.GetSize();
            rect.localPosition = InventoryConst.DescNameRect.ToCanvasPos();
        }

        public void SetTypographyData(TextTypographyData data)
        {
            descNameText.color = data.color;
            descNameText.fontSize = data.fontSize;
            descNameText.alignment = data.alignmentOptions;
            descNameText.lineSpacing = data.lineSpacing;
            
            descNameText.font = data.fontAsset;
        }

        public void SetDescriptionName(string descName)
        {
            descNameText.text = descName;
        }
    }
}