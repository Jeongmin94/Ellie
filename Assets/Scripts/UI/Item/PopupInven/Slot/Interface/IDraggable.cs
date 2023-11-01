namespace Assets.Scripts.UI.Item.PopupInven
{
    public interface IDraggable
    {
        public SlotInfo GetSlotInfo();
        public void SetSlotInfo(SlotInfo info);
    }
}