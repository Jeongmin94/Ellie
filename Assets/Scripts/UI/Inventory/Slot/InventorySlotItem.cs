using Assets.Scripts.Item;
using Assets.Scripts.UI.Framework.Presets;
using UnityEngine;

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
            // test
            // !TODO
            // 1. 아이템 파싱해서 아이템 클래스 생성
            // 2. 아이템 추가할 때, 아이템 클래스 정보 받고, inventory를 onDragParent로 등록
            ItemData data = new ItemData();
            data.imageName = "UI/Item/ItemDefaultRed";
            data.name = "TestItem";

            BaseItem baseItem = new BaseItem();
            baseItem.itemData = data;
            baseItem.InitResources();

            itemImage.sprite = baseItem.ItemSprite;
            itemText.text = $"origin: {baseItem.ItemCount}";
            SlotItem = baseItem;

            // 아이템 슬롯이 추가될 때, inventory를 onDragParent에 추가시켜야 함
            onDragParent = transform.parent;
        }

        public override bool IsOrigin() => true;
    }
}