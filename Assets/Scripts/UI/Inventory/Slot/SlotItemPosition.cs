using Assets.Scripts.Item;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Inventory
{
    public class SlotItemPosition : MonoBehaviour
    {
        public InventorySlot slot;

        // !TODO: 필요 없음. 삭제해도 됨(인벤토리 기본 구현 완료 후 삭제하기)
        private Image itemImage;

        private void Awake()
        {
            itemImage = GetComponent<Image>();
        }

        public void SetItem(BaseItem item)
        {
            slot.SlotItem = item;
        }

        public void ClearItem()
        {
            slot.SlotItem = null;
        }
    }
}