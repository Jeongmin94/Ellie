using System;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Inventory
{
    public enum SlotAreaType
    {
        Item,
        Equipment
    }

    public class InventorySlotArea : UIBase
    {
        public static readonly string Path = "Slot/InventorySlotArea";

        public RectTransform SlotRect => rect;
        public SlotAreaType SlotAreaType { get; set; } = SlotAreaType.Item;

        private RectTransform rect;
        private GridLayoutGroup grid;

        private int row;
        private int col;

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
            rect = GetComponent<RectTransform>();
            grid = gameObject.GetOrAddComponent<GridLayoutGroup>();
        }

        private void InitObjects()
        {
        }

        public void InitSlotRect(Transform parent, Rect size)
        {
            AnchorPresets.SetAnchorPreset(rect, AnchorPresets.MiddleCenter);
            rect.sizeDelta = size.GetSize();
            rect.localPosition = size.ToCanvasPos();
            rect.SetParent(parent);
        }

        public void InitGridLayoutGroup(int row, int col, int padding, int spacing, GridLayoutGroup.Corner corner, GridLayoutGroup.Axis axis, TextAnchor anchor)
        {
            float width = rect.rect.width;
            float height = rect.rect.height;

            var paddingOffset = new RectOffset();
            paddingOffset.SetAllPadding(padding);
            var spacingOffset = new Vector2(spacing, spacing);

            var w = width - paddingOffset.left * 2 - (col - 1) * spacingOffset.x;
            var slotW = w / col;
            var h = height - paddingOffset.top * 2 - (row - 1) * spacingOffset.y;
            var slotH = h / row;

            float len = Mathf.Min(slotH, slotW);
            grid.spacing = spacingOffset;
            grid.padding = paddingOffset;
            grid.startCorner = corner;
            grid.startAxis = axis;
            grid.childAlignment = anchor;
            grid.cellSize = new Vector2(len, len);

            this.row = row;
            this.col = col;
        }

        public void MakeSlots()
        {
            for (int i = 0; i < row * col; i++)
            {
                var slot = UIManager.Instance.MakeSubItem<InventorySlot>(rect, UIManager.InventorySlot);
            }
        }
    }
}