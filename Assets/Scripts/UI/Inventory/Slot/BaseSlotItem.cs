using Assets.Scripts.Item;
using Assets.Scripts.Managers;
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

        public BaseItem SlotItemData { get; set; }

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

        public virtual void InitBaseSlotItem()
        {
            Init();
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
            slotItemPosition = null;
        }

        private void BindEvents()
        {
            gameObject.BindEvent(OnBeginDragHandler, UIEvent.BeginDrag);
            gameObject.BindEvent(OnDragHandler, UIEvent.Drag);
            gameObject.BindEvent(OnEndDragHandler, UIEvent.EndDrag);
            gameObject.BindEvent(OnDropHandler, UIEvent.Drop);
            gameObject.BindEvent(OnClickHandler, UIEvent.Click);
        }

        private void OnClickHandler(PointerEventData data)
        {
            var payload = new InventoryEventPayload();
            // 좌클릭 => 설명에 등록
            if (data.button == PointerEventData.InputButton.Left)
            {
                if (slotItemPosition.slot.SlotType == SlotAreaType.Description)
                    return;

                payload.eventType = InventoryEventType.ShowDescription;
            }
            // 우클릭 => 장착
            else if (data.button == PointerEventData.InputButton.Right)
            {
                if (slotItemPosition.slot.SlotType == SlotAreaType.Description)
                    return;

                // 장착 + 우클릭 -> 장착 해제
                if (slotItemPosition.slot.SlotType == SlotAreaType.Equipment)
                {
                    SlotItemData.ClearSlot(SlotAreaType.Equipment);
                    SlotItemData.DestroyItem(SlotAreaType.Equipment);
                    return;
                }

                // 아이템 + 우클릭 -> 장착에 등록
                payload.eventType = InventoryEventType.CopyItemWithShortCut;
            }

            payload.baseSlotItem = this;
            slotItemPosition.slot.InvokeSlotItemEvent(payload);
        }

        private void OnDropHandler(PointerEventData data)
        {
            var droppedItem = data.pointerDrag;
            var otherSlotItem = droppedItem.GetComponent<BaseSlotItem>();

            if (otherSlotItem == null)
                return;

            var otherSlot = otherSlotItem.slotItemPosition.slot;
            var thisSlot = slotItemPosition.slot;
            if (otherSlot.SlotType != thisSlot.SlotType)
            {
                return;
            }

            var swapSlot = UIManager.Instance.slotSwapBuffer;
            swapSlot.SlotType = thisSlot.SlotType;
            swapSlot.InvokeCopyOrMove(this);

            thisSlot.InvokeCopyOrMove(otherSlotItem);

            otherSlot.InvokeCopyOrMove(this);
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

        public virtual void SetItemData(BaseItem baseItem)
        {
            SlotItemData = baseItem;

            if (slotItemPosition)
            {
                slotItemPosition.SetItem(SlotItemData);
            }

            itemImage.sprite = SlotItemData.ItemSprite;
            itemText.text = SlotItemData.itemCount.Value.ToString();

            SlotItemData.itemCount.Subscribe(OnItemCountChanged);
        }

        public InventorySlot GetSlot()
        {
            if (slotItemPosition)
                return slotItemPosition.slot;

            return null;
        }

        // 슬롯의 위치 이동
        public void MoveSlot(SlotItemPosition position, BaseItem data)
        {
            SetSlot(position);
            SetItemData(data);
        }

        // SlotItemData에서 참조하는 슬롯 변경
        public void ChangeSlot(SlotAreaType type, InventorySlot slot)
        {
            SlotItemData?.ChangeSlot(type, slot);
        }

        // SlotItemData에서 참조하는 슬롯 아이템 변경
        public void ChangeSlotItem(SlotAreaType type, BaseSlotItem item)
        {
            SlotItemData?.ChangeSlotItem(type, item);
        }

        // Equipment Frame에서 참조하는 슬롯 아이템 변경
        public void ChangeEquipmentSlotItem(SlotAreaType type, BaseSlotItem item)
        {
            SlotItemData?.ChangeEquipmentSlot(type, item);
        }

        private const int ItemMinCount = 0;
        private const int ItemMaxCount = 9999;

        private void OnItemCountChanged(int value)
        {
            if (value < ItemMinCount)
                value = ItemMinCount;
            if (value > ItemMaxCount)
                value = ItemMaxCount;
            itemText.text = value.ToString();
        }

        public BaseItem GetBaseItem() => SlotItemData;

        public void SetSlot(SlotItemPosition parent)
        {
            isDropped = true;

            if (slotItemPosition != null)
            {
                // 이전 슬롯의 아이템 설정 초기화
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