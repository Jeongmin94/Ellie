using Item;
using UnityEngine;

namespace UI.Inventory.Slot
{
    public class SlotItemPosition : MonoBehaviour
    {
        public InventorySlot slot;

        // 슬롯에 아이템 할당
        public void SetItem(BaseItem item)
        {
            slot.SlotItemData = item;
            var _groupType = slot.SlotItemData.itemData.groupType;
            slot.InvokeEquipmentFrameAction(new InventoryEventPayload { slot = slot, groupType = _groupType });
        }

        // 슬롯에 있는 아이템 제거
        public void ClearItem()
        {
            var _groupType = slot.SlotItemData.itemData.groupType;
            slot.SlotItemData = null;
            slot.InvokeEquipmentFrameAction(new InventoryEventPayload { slot = slot, groupType = _groupType });
        }
    }
}