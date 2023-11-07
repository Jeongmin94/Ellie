using Assets.Scripts.Item;
using Assets.Scripts.UI.Inventory;
using UnityEngine;

namespace Assets.Scripts.UI.Item.PopupInven
{
    public interface IDraggable
    {
        public BaseItem GetBaseItem();
        public void SetSlot(SlotItemPosition parent);
        public bool IsOrigin();
    }
}