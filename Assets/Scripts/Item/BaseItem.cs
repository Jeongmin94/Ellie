using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.ActionData;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Inventory;
using UnityEngine;

namespace Assets.Scripts.Item
{
    public delegate void ItemDestroyHandler();

    public class BaseItem
    {
        private ItemDestroyHandler destroyHandler;

        public ItemData itemData;

        public readonly Data<int> itemCount = new Data<int>();

        public int ItemIndex => itemData.index;
        public Sprite ItemSprite { get; private set; }
        public string ItemName => itemData.name;

        public virtual void InitResources()
        {
            ItemSprite = ResourceManager.Instance.LoadSprite(itemData.imageName);
            itemCount.Value++;
        }

        public void SubscribeDestroyHandler(ItemDestroyHandler listener)
        {
            destroyHandler -= listener;
            destroyHandler += listener;
        }

        public readonly IDictionary<SlotAreaType, InventorySlot> slots = new Dictionary<SlotAreaType, InventorySlot>();
        public readonly IDictionary<SlotAreaType, BaseSlotItem> slotItems = new Dictionary<SlotAreaType, BaseSlotItem>();

        public void ChangeSlot(SlotAreaType type, InventorySlot slot)
        {
            slots[type] = slot;
        }

        public void ChangeSlotItem(SlotAreaType type, BaseSlotItem item)
        {
            slotItems[type] = item;
        }

        public void DestroyItem()
        {
            var keys = slots.Keys;
            foreach (var key in keys)
            {
                slots[key].ClearSlotItemPosition();
            }

            slots.Clear();

            var slotItemsKeys = slotItems.Keys;
            foreach (var key in slotItemsKeys)
            {
                ResourceManager.Instance.Destroy(slotItems[key].gameObject);
            }

            slotItems.Clear();
        }
    }
}