using Item;
using UI.Inventory.Slot;

namespace UI.Item.PopupInven.Slot.Interface
{
    public interface IDraggable
    {
        public BaseItem GetBaseItem();
        public void SetSlot(SlotItemPosition parent);
        public bool IsOrigin();
    }
}