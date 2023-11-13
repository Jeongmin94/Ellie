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
            slot.InvokeEquipmentFrameAction(new InventoryEventPayload() { slot = slot });
        }

        // 슬롯에 있는 아이템 제거
        public void ClearItem()
        {
            slot.SlotItemData = null;
            slot.InvokeEquipmentFrameAction(new InventoryEventPayload() { slot = slot });
        }
    }
}