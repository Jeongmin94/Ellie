using Assets.Scripts.Item;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Item.PopupInven;
using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Inventory
{
    public abstract class BaseSlotItem : UIBase, IDraggable
    {
        private enum Texts
        {
            ItemText
        }

        private enum Images
        {
            ItemImage
        }

        public BaseItem SlotItem { get; set; }

        private Image raycastImage;
        private RectTransform rect;

        // Item
        protected Image itemImage;
        protected TextMeshProUGUI itemText;

        // itemCount.lineSpacing = lineHeight;
        // itemCount.fontSize = fontSize;
        // itemCount.color = Color.white;
        // itemCount.alignment = TextAlignmentOptions.MidlineRight;
        // itemCount.text = string.Empty;
        //
        // defaultSprite = itemImage.sprite;

        protected Transform onDragParent;
        protected SlotItemPosition slotItemPosition;
        private bool isDropped;

        protected override void Init()
        {
            isDropped = false;

            Bind();
            InitObjects();
            BindEvents();
        }

        private void Bind()
        {
            Bind<Image>(typeof(Images));
            Bind<TextMeshProUGUI>(typeof(Texts));

            itemImage = GetImage((int)Images.ItemImage);
            itemText = GetText((int)Texts.ItemText);

            rect = GetComponent<RectTransform>();
            raycastImage = GetComponent<Image>();
        }

        private void InitObjects()
        {
            slotItemPosition = null;
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
            isDropped = false;
        }

        private void OnDragHandler(PointerEventData data)
        {
            transform.position = data.position;
        }

        private void OnEndDragHandler(PointerEventData data)
        {
            raycastImage.raycastTarget = true;

            if (!isDropped)
            {
                transform.SetParent(slotItemPosition.transform);
                ResetRect(rect);
            }

            isDropped = false;
        }

        private void ResetRect(RectTransform rectTransform)
        {
            rectTransform.sizeDelta = Vector2.one;
            rectTransform.localPosition = Vector3.zero;
        }

        public virtual void SetItem(BaseItem baseItem)
        {
            SlotItem = baseItem;
            if (slotItemPosition)
            {
                slotItemPosition.SetItem(SlotItem);
            }

            itemImage.sprite = SlotItem.ItemSprite;
            itemText.text = baseItem.itemCount.Value.ToString();
            
            baseItem.itemCount.Subscribe(OnItemCountChanged);
        }

        private void OnItemCountChanged(int value)
        {
            itemText.text = value.ToString();
        }

        public BaseItem GetBaseItem() => SlotItem;

        public void SetSlot(SlotItemPosition parent)
        {
            isDropped = true;

            if (slotItemPosition != null)
            {
                slotItemPosition.ClearItem();
            }

            slotItemPosition = parent;
            transform.SetParent(slotItemPosition.transform);
            AnchorPresets.SetAnchorPreset(rect, AnchorPresets.StretchAll);
            ResetRect(rect);
        }

        public abstract bool IsOrigin();

        public void SetOnDragParent(Transform parent)
        {
            onDragParent = parent;
        }
    }
}