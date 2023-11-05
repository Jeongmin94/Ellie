using Assets.Scripts.Item;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Item.PopupInven;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Inventory
{
    public class InventorySlotItem : UIBase, IDraggable
    {
        public static readonly string Path = "Slot/InventorySlotItem";

        public BaseItem SlotItem { get; set; }

        private RectTransform rect;
        private Image raycastImage;

        private Transform onDragParent;
        private Transform currentParent;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            // test
            ItemData data = new ItemData();
            data.spriteName = "UI/Item/ItemDefaultRed";
            data.name = "TestItem";

            BaseItem baseItem = new BaseItem();
            baseItem.itemData = data;
            baseItem.InitResources();

            SlotItem = baseItem;
            onDragParent = transform.parent;

            Bind();
            InitObjects();
            BindEvents();
        }

        private void Bind()
        {
            rect = GetComponent<RectTransform>();
            raycastImage = GetComponent<Image>();
        }

        private void InitObjects()
        {
        }

        private void BindEvents()
        {
            gameObject.BindEvent(OnBeginDragHandler, UIEvent.BeginDrag);
            gameObject.BindEvent(OnDragHandler, UIEvent.Drag);
            gameObject.BindEvent(OnEndDragHandler, UIEvent.EndDrag);
        }

        private void OnBeginDragHandler(PointerEventData data)
        {
            raycastImage.raycastTarget = false;
            transform.SetParent(onDragParent);
        }

        private void OnDragHandler(PointerEventData data)
        {
            transform.position = data.position;
        }

        private void OnEndDragHandler(PointerEventData data)
        {
            raycastImage.raycastTarget = true;
        }

        public void SetSlot(Transform parent)
        {
            transform.SetParent(parent);

            AnchorPresets.SetAnchorPreset(rect, AnchorPresets.StretchAll);
            rect.sizeDelta = Vector2.one;
            rect.localPosition = Vector3.zero;
        }

        public BaseItem GetBaseItem()
        {
            return SlotItem;
        }
    }
}