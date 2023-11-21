using Assets.Scripts.Item;
using UnityEngine;

namespace Assets.Scripts.UI.Inventory
{
    public class SlotItemPosition : MonoBehaviour
    {
        public InventorySlot slot;

        // 슬롯에 아이템 할당
        public void SetItem(BaseItem item)
        {
            slot.SlotItemData = item;
            GroupType _groupType = slot.SlotItemData.itemData.groupType;
            slot.InvokeEquipmentFrameAction(new InventoryEventPayload() { slot = slot, groupType = _groupType });
        }

        // 슬롯에 있는 아이템 제거
        public void ClearItem()
        {
            GroupType _groupType = slot.SlotItemData.itemData.groupType;
            slot.SlotItemData = null;
            slot.InvokeEquipmentFrameAction(new InventoryEventPayload() { slot = slot, groupType = _groupType });
        }
    }
}