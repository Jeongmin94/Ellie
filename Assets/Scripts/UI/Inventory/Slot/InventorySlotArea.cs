using System;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.Utils;
using Channels.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Inventory
{
    public enum SlotAreaType
    {
        Item,
        Equipment,
        Description
    }

    public class InventorySlotArea : UIBase
    {
        public static readonly string Path = "Slot/InventorySlotArea";

        private SlotAreaType SlotAreaType { get; set; } = SlotAreaType.Item;

        private RectTransform rect;
        private GridLayoutGroup grid;
        private int row;
        private int col;

        private Action<InventoryEventPayload> slotAreaInventoryAction;

        private readonly List<InventorySlot> slots = new List<InventorySlot>();

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

        public void MoveSlotArea(Transform target, Transform parent, Rect size)
        {
            transform.SetParent(target);
            SetRect(size);
            rect.SetParent(parent);
        }

        public void InitSlotRect(Transform parent, Rect size)
        {
            AnchorPresets.SetAnchorPreset(rect, AnchorPresets.MiddleCenter);
            SetRect(size);
            rect.SetParent(parent);
        }

        private void SetRect(Rect position)
        {
            rect.sizeDelta = position.GetSize();
            rect.localPosition = position.ToCanvasPos();
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

        public void MakeSlots(SlotAreaType type)
        {
            SlotAreaType = type;
            for (int i = 0; i < row * col; i++)
            {
                var slot = UIManager.Instance.MakeSubItem<InventorySlot>(rect, UIManager.InventorySlot);
                slot.Index = i;
                slot.Subscribe(OnSlotInventoryAction);
                slot.SlotType = SlotAreaType;
                slots.Add(slot);
            }
        }

        private void OnDestroy()
        {
            slotAreaInventoryAction = null;
        }

        #region InventoryChannel

        public void Subscribe(Action<InventoryEventPayload> listener)
        {
            slotAreaInventoryAction -= listener;
            slotAreaInventoryAction += listener;
        }

        private void OnSlotInventoryAction(InventoryEventPayload payload)
        {
            if (payload.eventType == InventoryEventType.CopyItemWithDrag && SlotAreaType == SlotAreaType.Equipment)
            {
                var dup = FindSlot(payload.baseItem.SlotItemData.ItemIndex);
                if (dup != null)
                {
                    return;
                }
            }

            payload.slotAreaType = SlotAreaType;
            slotAreaInventoryAction?.Invoke(payload);
        }

        public InventorySlot FindSlot(int itemIndex)
        {
            return slots.Find(s => s.SlotItemData != null && s.SlotItemData.ItemIndex == itemIndex);
        }

        public InventorySlot FindEmptySlot()
        {
            return slots.Find(s => s.SlotItemData == null);
        }

        public void AddItem(UIPayload payload)
        {
            // 1. 해당 아이템이 이미 있는 경우에는 카운트 증가
            var item = payload.itemData;
            var dup = FindSlot(item.index);
            if (dup)
            {
                dup.SlotItemData.itemCount.Value++;
                return;
            }

            // 2. 해당 아이템이 없는 경우에는 비어있는 슬롯에 차례대로 추가
            var emptySlot = FindEmptySlot();
            emptySlot.CreateSlotItem(payload);
        }

        public void ConsumeItem(UIPayload payload)
        {
            // 1. 존재하지 않는 아이템이면 무효 처리
            var item = payload.itemData;
            var slot = FindSlot(item.index);
            if (slot == null)
            {
                Debug.LogWarning($"{item.name}은 존재하지 않는 아이템입니다.");
                return;
            }

            // 2. 존재하는 아이템이면 카운트 감소
            slot.SlotItemData.itemCount.Value--;
            if (slot.SlotItemData.itemCount.Value == 0)
            {
                Debug.Log($"{slot.Index}의 아이템 삭제");

                InventoryEventPayload inventoryEvent = new InventoryEventPayload();
                inventoryEvent.eventType = InventoryEventType.SortSlotArea;
                inventoryEvent.groupType = slot.SlotItemData.itemData.groupType;

                slot.SlotItemData.Reset();
                slotAreaInventoryAction?.Invoke(inventoryEvent);
            }
        }

        public void Sort()
        {
            for (int i = 0; i < slots.Count; i++)
            {
                var current = slots[i];
                if (current.SlotItemData == null)
                    continue;

                InventorySlot emptySlot = null;
                for (int j = i - 1; j >= 0; j--)
                {
                    var s = slots[j];
                    if (s.SlotItemData == null)
                        emptySlot = s;
                }

                if (emptySlot == null)
                    continue;

                var baseSlotItem = current.SlotItemData.slotItems[SlotAreaType];
                emptySlot.InvokeCopyOrMove(baseSlotItem);
            }
        }

        #endregion
    }
}