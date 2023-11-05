using System;
using Assets.Scripts.Item;
using Assets.Scripts.UI.Framework.Presets;
using UnityEngine;

namespace Assets.Scripts.UI.Inventory
{
    public class InventorySlotCopyItem : BaseSlotItem
    {
        public static readonly string Path = "Slot/InventorySlotCopyItem";

        private Action<InventoryEventPayload> copyItemAction;
        private BaseItem originItem;

        private void Awake()
        {
            Init();
        }

        public void SetCopyItem(BaseItem baseItem)
        {
            originItem = baseItem;
            itemImage.sprite = originItem.ItemSprite;
            itemText.text = $"copy: {originItem.ItemCount}";
        }

        public void Subscribe(Action<InventoryEventPayload> listener)
        {
            copyItemAction -= listener;
            copyItemAction += listener;
        }

        private void OnDestroy()
        {
            copyItemAction = null;
            originItem = null;
        }

        public override void SetSlot(Transform parent)
        {
            isDropped = true;

            slotParent = parent;
            transform.SetParent(slotParent);
            AnchorPresets.SetAnchorPreset(rect, AnchorPresets.StretchAll);
            ResetRect(rect);
        }

        public override BaseItem GetBaseItem() => originItem;
        public override bool IsOrigin() => false;
    }
}