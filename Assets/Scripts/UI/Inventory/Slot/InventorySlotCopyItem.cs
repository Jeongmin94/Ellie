using System;
using Assets.Scripts.Item;

namespace Assets.Scripts.UI.Inventory
{
    public class InventorySlotCopyItem : BaseSlotItem
    {
        public static readonly string Path = "Slot/InventorySlotCopyItem";

        private Action<InventoryEventPayload> copyItemAction;

        private void Awake()
        {
            Init();
        }

        public void Subscribe(Action<InventoryEventPayload> listener)
        {
            copyItemAction -= listener;
            copyItemAction += listener;
        }

        public override void SetItem(BaseItem baseItem)
        {
            SlotItem = baseItem;
            if (slotItemPosition)
            {
                slotItemPosition.SetItem(SlotItem);
            }

            itemImage.sprite = SlotItem.ItemSprite;
            itemText.text = $"copy: {SlotItem.ItemCount.Value}";
        }

        private void OnDestroy()
        {
            copyItemAction = null;
            SlotItem = null;
        }

        public override bool IsOrigin() => false;
    }
}