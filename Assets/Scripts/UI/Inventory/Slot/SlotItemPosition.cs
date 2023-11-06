using Assets.Scripts.Item;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Inventory
{
    public class SlotItemPosition : MonoBehaviour
    {
        public InventorySlot slot;

        public void SetItem(BaseItem item)
        {
            slot.SlotItemData = item;
        }

        public void ClearItem()
        {
            slot.SlotItemData = null;
        }
    }
}