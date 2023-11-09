using System;

namespace Assets.Scripts.UI.Inventory
{
    public class InventorySlotCopyItem : BaseSlotItem
    {
        public static readonly string Path = "Slot/InventorySlotCopyItem";

        private Action<InventoryEventPayload> copyItemAction;

        public override void InitBaseSlotItem()
        {
            Init();
        }

        public void Subscribe(Action<InventoryEventPayload> listener)
        {
            copyItemAction -= listener;
            copyItemAction += listener;
        }

        private void OnDestroy()
        {
            copyItemAction = null;
            SlotItemData = null;
        }

        public override bool IsOrigin() => false;
    }
}