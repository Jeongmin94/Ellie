using System;
using Assets.Scripts.Item;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Item.PopupInven;
using Assets.Scripts.Utils;
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

        private readonly int fontSize = 28;
        private readonly float lineHeight = 25.0f;

        public int Index { get; set; }
        public BaseItem SlotItem { get; set; }
        public SlotAreaType SlotType { get; set; }
        public SlotItemPosition SlotItemPosition { get; private set; }

        private RectTransform rect;

        // !TODO: 필요 없음. 삭제해도 됨(인벤토리 기본 구현 완료 후 삭제하기)
        private Image itemImage;

        private Action<InventoryEventPayload> slotInventoryAction;

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
            SlotItem = null;

            rect = GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(rect, AnchorPresets.MiddleCenter);
            rect.sizeDelta = InventoryConst.SlotRect.GetSize();
            rect.localPosition = InventoryConst.SlotRect.ToCanvasPos();

            // itemCount.lineSpacing = lineHeight;
            // itemCount.fontSize = fontSize;
            // itemCount.color = Color.white;
            // itemCount.alignment = TextAlignmentOptions.MidlineRight;
            // itemCount.text = string.Empty;
            //
            // defaultSprite = itemImage.sprite;
        }

        // !TODO: 스위칭 전용 슬롯 추가 필요
        //      - 스위칭 슬롯 -> 아이템 슬롯: 이동됨, 스위칭 슬롯에서 빠짐
        //      - 아이템 슬롯 -> 스위칭 슬롯: 복사됨, 아이템 슬롯에는 그대로 있음
        //      - 스위칭 슬롯에 등록되면 static ui에 등록된 아이템 표시되어야 함

        // 슬롯에 아이템 장착
        // 아이템 정보, 슬롯 인덱스
        private void OnDropHandler(PointerEventData data)
        {
            // Description은 읽기 전용
            if (SlotType == SlotAreaType.Description)
                return;

            var droppedItem = data.pointerDrag;
            var baseSlotItem = droppedItem.GetComponent<BaseSlotItem>();
            if (baseSlotItem == null)
                return;

            var payload = new InventoryEventPayload
            {
                baseItem = baseSlotItem,
                slot = this,
            };

            // origin items
            if (baseSlotItem.IsOrigin())
            {
                // copy
                if (SlotType == SlotAreaType.Equipment)
                {
                    payload.eventType = InventoryEventType.CopyItem;
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

        public void UpdateItem()
        {
            
        }

        public void Subscribe(Action<InventoryEventPayload> listener)
        {
            slotInventoryAction -= listener;
            slotInventoryAction += listener;
        }

        private void OnDestroy()
        {
            slotInventoryAction = null;
        }
    }
}