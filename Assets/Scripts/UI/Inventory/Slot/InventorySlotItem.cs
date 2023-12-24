namespace UI.Inventory.Slot
{
    public class InventorySlotItem : BaseSlotItem
    {
        public static readonly string Path = "Slot/InventorySlotItem";

        public override void InitBaseSlotItem()
        {
            Init();
        }


        protected override void Init()
        {
            base.Init();

            InitObjects();
        }

        private void InitObjects()
        {
        }

        public override bool IsOrigin()
        {
            return true;
        }
    }
}