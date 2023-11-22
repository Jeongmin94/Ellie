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
        public ItemMetaData itemData;

        public readonly Data<int> itemCount = new Data<int>();

        public int ItemIndex => itemData.index;
        public Sprite ItemSprite { get; private set; }
        public string ItemName => itemData.name;

        public virtual void InitResources()
        {
            ItemSprite = ResourceManager.Instance.LoadSprite(itemData.imageName);
            itemCount.Value++;
        }

        public readonly IDictionary<SlotAreaType, InventorySlot> slots = new Dictionary<SlotAreaType, InventorySlot>();
        public readonly IDictionary<SlotAreaType, BaseSlotItem> slotItems = new Dictionary<SlotAreaType, BaseSlotItem>();
        public readonly IDictionary<SlotAreaType, BaseSlotItem> equipmentSlotItems = new Dictionary<SlotAreaType, BaseSlotItem>();

        public void ChangeSlot(SlotAreaType type, InventorySlot slot)
        {
            slots[type] = slot;
        }

        public void ChangeSlotItem(SlotAreaType type, BaseSlotItem item)
        {
            slotItems[type] = item;
        }

        public void ChangeEquipmentSlot(SlotAreaType type, BaseSlotItem item)
        {
            equipmentSlotItems[type] = item;
        }

        public void Reset()
        {
            ClearAllSlot();
            DestroyAllItem();
        }

        public void ClearAllSlot()
        {
            var keys = slots.Keys;
            foreach (var key in keys)
            {
                slots[key].ClearSlotItemPosition();
            }

            slots.Clear();
        }

        public void ClearSlot(SlotAreaType type)
        {
            slots[type].ClearSlotItemPosition();
            slots.Remove(type);
        }

        public void DestroyAllItem()
        {
            var slotItemsKeys = slotItems.Keys;
            foreach (var key in slotItemsKeys)
            {
                slotItems[key].SlotItemData = null;
                ResourceManager.Instance.Destroy(slotItems[key].gameObject);
            }

            slotItems.Clear();
        }

        public void DestroyItem(SlotAreaType type)
        {
            slotItems[type].SlotItemData = null;
            ResourceManager.Instance.Destroy(slotItems[type].gameObject);
            slotItems.Remove(type);
        }
    }
}