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

        private void OnDestroy()
        {
            copyItemAction = null;
            SlotItem = null;
        }

        public override bool IsOrigin() => false;
    }
}