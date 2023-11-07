using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
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

        private readonly Color fontColor = Color.black;
        private readonly int fontSize = 18;
        private readonly float lineHeight = 25;

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

        // !TODO: 텍스트 폰트 설정(폰트가 미정)
        private void InitObjects()
        {
            rect = GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(rect, AnchorPresets.MiddleCenter);
            rect.sizeDelta = InventoryConst.DescTextRect.GetSize();
            rect.localPosition = InventoryConst.DescTextRect.ToCanvasPos();

            descText.color = fontColor;
            descText.fontSize = fontSize;
            descText.alignment = TextAlignmentOptions.TopLeft;
            descText.lineSpacing = lineHeight;
        }
    }
}