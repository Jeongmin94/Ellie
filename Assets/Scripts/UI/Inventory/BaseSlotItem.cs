using System.Collections.Generic;
using Assets.Scripts.Item;
using Assets.Scripts.UI.Framework;
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
        protected RectTransform rect;

        // Item
        protected Image itemImage;
        protected TextMeshProUGUI itemText;

        protected Transform onDragParent;
        protected Transform slotParent;
        protected bool isDropped;

        public void InitResource(BaseItem baseItem)
        {
            SlotItem = baseItem;
            SlotItem.InitResources();
        }

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
                transform.SetParent(slotParent);
                ResetRect(rect);
            }

            isDropped = false;
        }

        protected void ResetRect(RectTransform rectTransform)
        {
            rectTransform.sizeDelta = Vector2.one;
            rectTransform.localPosition = Vector3.zero;
        }

        public abstract BaseItem GetBaseItem();

        public abstract void SetSlot(Transform parent);

        public abstract bool IsOrigin();

        public void SetOnDragParent(Transform parent)
        {
            onDragParent = parent;
        }
    }
}