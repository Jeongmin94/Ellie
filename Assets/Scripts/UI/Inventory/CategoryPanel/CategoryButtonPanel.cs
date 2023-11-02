using Assets.Scripts.UI.Framework;
using UnityEngine;

namespace Assets.Scripts.UI.Inventory
{
    // !TODO: 버튼 패널에 있는 버튼을 누를 때마다 카테고리 슬롯 항목들이 변경되어야 함
    public class CategoryButtonPanel : UIBase
    {
        private void Awake()
        {
            Init();
        }

        private RectTransform rect;

        protected override void Init()
        {
            Bind();
            InitObjects();
        }

        private void Bind()
        {
            rect = GetComponent<RectTransform>();
        }

        private void InitObjects()
        {
            // AnchorPresets.SetAnchorPreset(rect, AnchorPresets.StretchAll);
        }
    }
}