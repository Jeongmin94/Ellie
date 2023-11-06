using Assets.Scripts.Item;

namespace Assets.Scripts.UI.Inventory
{
    public class InventorySlotItem : BaseSlotItem
    {
        public static readonly string Path = "Slot/InventorySlotItem";

        private void Awake()
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

        public override bool IsOrigin() => true;
    }
}