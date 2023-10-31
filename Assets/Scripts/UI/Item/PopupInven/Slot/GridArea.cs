using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
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

        private readonly List<Slot> slots = new List<Slot>();

        // grid 요소 관리
        private int autoIdx;

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

        public void InitGridArea()
        {
            AnchorPresets.SetAnchorPreset(rectTransform, AnchorPresets.StretchAll);
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.localPosition = Vector2.zero;

            InitGridLayout();
        }

        // !TODO: 슬롯 만들 때, 슬롯 관리 필요(인덱스, 슬롯 포지션 등)
        public void InitSlot(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var slot = UIManager.Instance.MakeSubItem<Slot>(rectTransform, UIManager.UISlot);
                slot.Index = i;
                slots.Add(slot);
            }
        }

        public void AddItem(SlotItem item)
        {
            var unusedSlot = slots.Find(s => !s.IsUsed);
            if (unusedSlot == null)
                return;

            unusedSlot.IsUsed = true;
            item.SetSlotInfo(SlotInfo.Of(unusedSlot));
        }

        public void RemoveItem(SlotItem item)
        {
            var info = item.GetSlotInfo();
            var slot = info.slot;

            if (!slot.IsUsed || slot.Index >= slots.Count)
                return;

            slot.IsUsed = false;
            ResourceManager.Instance.Destroy(item.gameObject);
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

            float len = slotWidth > slotHeight ? slotHeight : slotWidth;
            gridLayoutGroup.spacing = spacingOffset;
            gridLayoutGroup.padding = paddingOffset;
            gridLayoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
            gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
            gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
            gridLayoutGroup.cellSize = new Vector2(len, len);
        }
    }
}