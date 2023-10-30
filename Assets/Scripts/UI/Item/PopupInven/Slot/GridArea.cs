using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

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
        }

        public void SetGrid(int padding, int spacing, int row, int col)
        {
            this.padding = padding;
            this.spacing = spacing;
            this.row = row;
            this.col = col;
        }

        public void InitSlotPanel()
        {
            AnchorPresets.SetAnchorPreset(rectTransform, AnchorPresets.StretchAll);
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.localPosition = Vector2.zero;

            InitGridLayout();
            CreateSlot(row * col);
        }

        private void CreateSlot(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var slot = UIManager.Instance.MakeSubItem<Slot>(rectTransform, UIManager.UISlot);

                UIManager.Instance.MakeSubItem<SlotItem>(slot.ItemPosition, UIManager.UISlotItem);
            }
        }

        private void InitGridLayout()
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