using System;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Inventory.DesctiptionPanel
{
    public class DescriptionNamePanel : UIBase
    {
        private enum Texts
        {
            DescriptionNameText
        }

        private RectTransform rect;
        private TextMeshProUGUI descNameText;

        private Color fontColor = new Color(217, 209, 209);
        private int fontSize = 31;
        private float lineHeight = 79.95f;

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
    }
}