using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;
using Resolution = Assets.Scripts.Managers.Resolution;

namespace Assets.Scripts.UI.Item.PopupInven
{
    public class GridArea : UIBase
    {
        // slot grid configurations
        public int padding = 5;
        public int spacing = 5;
        public int row = 3;
        public int col = 8;

        private RectTransform rectTransform;
        private GridLayoutGroup gridLayoutGroup;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            rectTransform = gameObject.GetOrAddComponent<RectTransform>();
            gridLayoutGroup = gameObject.GetOrAddComponent<GridLayoutGroup>();

            rectTransform.localScale = Vector3.one;
        }

        public void InitSlotPanel()
        {
            rectTransform.anchorMin = AnchorPresets.StretchAll.AnchorMin;
            rectTransform.anchorMax = AnchorPresets.StretchAll.AnchorMax;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.localPosition = Vector2.zero;

            OnChangeResolution(GameManager.Instance.resolution);
            CreateSlot(row * col);
        }

        private void CreateSlot(int count)
        {
            for (int i = 0; i < count; i++)
            {
                UIManager.Instance.MakeSubItem<Slot>(rectTransform, UIManager.UISlot);
            }
        }

        private void OnChangeResolution(Resolution resolution)
        {
            var paddingOffset = new RectOffset();
            paddingOffset.SetAllPadding(padding);
            var spacingOffset = new Vector2(spacing, spacing);

            float width = rectTransform.rect.width - paddingOffset.left * 2 - (col - 1) * spacingOffset.x;
            float slotWidth = width / col;

            float height = rectTransform.rect.height - paddingOffset.top * 2 - (row - 1) * spacingOffset.y;
            float slotHeight = height / row;

            gridLayoutGroup.spacing = spacingOffset;
            gridLayoutGroup.padding = paddingOffset;
            gridLayoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
            gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
            gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
            gridLayoutGroup.cellSize = new Vector2(slotWidth, slotHeight);
        }
    }
}