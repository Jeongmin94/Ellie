using Assets.Scripts.Managers;
using Data.UI.Opening;
using Item;
using Managers.Sound;
using Managers.UI;
using TMPro;
using UI.Framework;
using UI.Framework.Presets;
using UI.Item.PopupInven.Slot.Interface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace UI.Inventory.Slot
{
    public abstract class BaseSlotItem : UIBase, IDraggable
    {
        private const int ItemMinCount = 0;
        private const int ItemMaxCount = 9999;

        private static readonly string SoundClick = "inven2";

        [SerializeField] private TextTypographyData itemCountData;
        private bool isDropped;

        // Item
        private Image itemImage;
        private TextMeshProUGUI itemText;

        private Transform onDragParent;

        private Image raycastImage;
        private RectTransform rect;
        private SlotItemPosition slotItemPosition;

        public BaseItem SlotItemData { get; set; }

        public BaseItem GetBaseItem()
        {
            return SlotItemData;
        }

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

            itemText.font = itemCountData.fontAsset;
            itemText.fontSize = itemCountData.fontSize;
            itemText.color = itemCountData.color;
            itemText.alignment = itemCountData.alignmentOptions;
            itemText.lineSpacing = itemCountData.lineSpacing;

            if (itemCountData.useOutline)
            {
                itemText.outlineColor = itemCountData.outlineColor;
                itemText.outlineWidth = itemCountData.outlineThickness;
            }
        }

        private void BindEvents()
        {
            gameObject.BindEvent(OnBeginDragHandler, UIEvent.BeginDrag);
            gameObject.BindEvent(OnDragHandler, UIEvent.Drag);
            gameObject.BindEvent(OnEndDragHandler, UIEvent.EndDrag);
            gameObject.BindEvent(OnDropHandler, UIEvent.Drop);
            gameObject.BindEvent(OnClickHandler);
        }

        private void OnClickHandler(PointerEventData data)
        {
            var payload = new InventoryEventPayload();
            // 좌클릭 => 설명에 등록
            if (data.button == PointerEventData.InputButton.Left)
            {
                if (slotItemPosition.slot.SlotType == SlotAreaType.Description)
                {
                    return;
                }

                payload.eventType = InventoryEventType.ShowDescription;
            }
            // 우클릭 => 장착
            else if (data.button == PointerEventData.InputButton.Right)
            {
                if (slotItemPosition.slot.SlotType == SlotAreaType.Description)
                {
                    return;
                }

                // 장착 + 우클릭 -> 장착 해제
                if (slotItemPosition.slot.SlotType == SlotAreaType.Equipment)
                {
                    SlotItemData.ClearSlot(SlotAreaType.Equipment);
                    SlotItemData.DestroyItem(SlotAreaType.Equipment);
                    SoundManager.Instance.PlaySound(SoundManager.SoundType.UISfx, SoundClick);

                    return;
                }

                // 아이템 + 우클릭 -> 장착에 등록
                payload.eventType = InventoryEventType.CopyItemWithShortCut;
            }

            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, SoundClick, Vector3.zero);

            payload.baseSlotItem = this;
            slotItemPosition.slot.InvokeSlotItemEvent(payload);
        }

        private void OnDropHandler(PointerEventData data)
        {
            var droppedItem = data.pointerDrag;
            var otherSlotItem = droppedItem.GetComponent<BaseSlotItem>();

            if (otherSlotItem == null)
            {
                return;
            }

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
            {
                return slotItemPosition.slot;
            }

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

        private void OnItemCountChanged(int value)
        {
            if (value < ItemMinCount)
            {
                value = ItemMinCount;
            }

            if (value > ItemMaxCount)
            {
                value = ItemMaxCount;
            }

            itemText.text = value.ToString();
        }

        public void SetOnDragParent(Transform parent)
        {
            onDragParent = parent;
        }

        private enum Texts
        {
            ItemText
        }

        private enum Images
        {
            ItemImage
        }
    }
}