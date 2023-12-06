using Assets.Scripts.Item;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Item.PopupInven;
using Assets.Scripts.Utils;
using Channels.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Inventory
{
    public class InventorySlot : UIBase, ISettable
    {
        private enum Images
        {
            ItemImage
        }

        public int Index { get; set; }
        public BaseItem SlotItemData { get; set; }
        public SlotAreaType SlotType { get; set; }
        public SlotItemPosition SlotItemPosition { get; private set; }

        private RectTransform rect;
        private readonly List<InventorySlot> copylist = new List<InventorySlot>();
        private Image itemImage;

        private Action<InventoryEventPayload> slotInventoryAction;

        // for Equipment Frame
        private Action<InventoryEventPayload> equipmentFrameAction;

        public BaseSlotItem GetBaseSlotItem()
        {
            if (SlotItemData == null)
                return null;

            return SlotItemData.slotItems[SlotType];
        }

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            Bind();
            InitObjects();
        }

        private void Bind()
        {
            Bind<Image>(typeof(Images));

            itemImage = GetImage((int)Images.ItemImage);
            SlotItemPosition = itemImage.gameObject.GetOrAddComponent<SlotItemPosition>();

            gameObject.BindEvent(OnDropHandler, UIEvent.Drop);
        }

        private void InitObjects()
        {
            SlotItemPosition.slot = this;
            SlotItemData = null;

            rect = GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(rect, AnchorPresets.MiddleCenter);
            rect.sizeDelta = InventoryConst.SlotRect.GetSize();
            rect.localPosition = InventoryConst.SlotRect.ToCanvasPos();
        }

        public void SetViewMode(bool isTrue)
        {
            var itemColor = itemImage.color;
            itemColor.a = isTrue ? 1.0f : 0.0f;
            itemImage.color = itemColor;
        }

        public void SetSprite(Sprite sprite)
        {
            itemImage.sprite = sprite;
        }

        // baseSlotItem에서 이벤트 호출할 수 있도록 열어둠
        public void InvokeSlotItemEvent(InventoryEventPayload payload)
        {
            slotInventoryAction?.Invoke(payload);
        }

        public void InvokeCopyOrMove(BaseSlotItem baseSlotItem)
        {
            var payload = new InventoryEventPayload
            {
                baseSlotItem = baseSlotItem,
                slot = this,
            };

            // origin items
            if (baseSlotItem.IsOrigin())
            {
                // copy
                if (SlotType == SlotAreaType.Equipment)
                {
                    payload.eventType = InventoryEventType.CopyItemWithDrag;
                }
                else
                {
                    // move
                    payload.eventType = InventoryEventType.MoveItem;
                }
            }
            // copy items
            else
            {
                if (SlotType == SlotAreaType.Equipment)
                {
                    // move
                    payload.eventType = InventoryEventType.MoveItem;
                }
                else
                {
                    // do nothing
                    return;
                }
            }

            slotInventoryAction?.Invoke(payload);
        }

        // 슬롯에 아이템 장착
        // 아이템 정보, 슬롯 인덱스
        private void OnDropHandler(PointerEventData data)
        {
            // Description은 읽기 전용
            if (SlotType == SlotAreaType.Description)
                return;

            if (SlotItemData != null)
            {
                Debug.Log($"이미 아이템이 존재하는 슬롯 {SlotItemData.ItemName}");
                return;
            }

            var droppedItem = data.pointerDrag;
            var baseSlotItem = droppedItem.GetComponent<BaseSlotItem>();
            if (baseSlotItem == null)
                return;

            InvokeCopyOrMove(baseSlotItem);
        }

        private InventorySlotItem CreateOrigin(UIPayload payload)
        {
            BaseItem baseItem = new BaseItem();
            baseItem.itemData = payload.itemData;
            baseItem.InitResources();

            var origin = UIManager.Instance.MakeSubItem<InventorySlotItem>(transform, InventorySlotItem.Path);
            origin.InitBaseSlotItem();
            origin.SetSlot(SlotItemPosition);
            origin.SetItemData(baseItem);
            origin.SetOnDragParent(payload.onDragParent);

            baseItem.slotItems[SlotType] = origin;
            baseItem.slots[SlotType] = this;

            return origin;
        }

        public void CreateSlotItem(UIPayload payload)
        {
            InvokeEquipmentFrameEvent(InventoryEventType.EquipItem, payload.itemData.groupType, CreateOrigin(payload));
        }

        public void InvokeEquipmentFrameEvent(InventoryEventType eventType, GroupType groupType, BaseSlotItem baseSlotItem)
        {
            var inventoryEventPayload = new InventoryEventPayload
            {
                eventType = eventType,
                groupType = groupType,
                baseSlotItem = baseSlotItem,
            };

            slotInventoryAction?.Invoke(inventoryEventPayload);
        }

        public void ClearSlotItemPosition()
        {
            SlotItemPosition.ClearItem();
        }

        public void Subscribe(Action<InventoryEventPayload> listener)
        {
            slotInventoryAction -= listener;
            slotInventoryAction += listener;
        }

        public void SubscribeEquipmentFrameAction(Action<InventoryEventPayload> listener)
        {
            equipmentFrameAction -= listener;
            equipmentFrameAction += listener;
        }

        public void InvokeEquipmentFrameAction(InventoryEventPayload payload)
        {
            equipmentFrameAction?.Invoke(payload);

            //0번 인덱스에 대해서만 추가적인 invoke
            if (SlotType == SlotAreaType.Equipment)
            {
                payload.eventType = InventoryEventType.SendMessageToPlayer;
                slotInventoryAction.Invoke(payload);
            }
        }

        private void OnDestroy()
        {
            slotInventoryAction = null;
            equipmentFrameAction = null;
        }

        #region SaveLoad

        public void LoadItem(UIPayload payload)
        {
            CreateOrigin(payload);
        }

        #endregion
    }
}