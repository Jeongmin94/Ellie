using System;
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
    public class InventorySlot : UIBase, ISettable
    {
        private enum Images
        {
            ItemImage
        }

        private enum Texts
        {
            ItemCount
        }

        private readonly int fontSize = 28;
        private readonly float lineHeight = 25.0f;

        // !TODO: 스크립터블 오브젝트 아이템으로 현재 슬롯에 위치한 아이템 확인하기
        // 슬롯에 필요한 것들
        //  - 아이템 개수
        //  - 아이템 이미지
        //  - 이동하는 것은 InventorySlotItem
        public InventorySlotItem SlotItem { get; set; }
        public int Index { get; set; }
        public SlotAreaType SlotType { get; set; }

        private RectTransform rect;
        private Image itemImage;
        private TextMeshProUGUI itemCount;
        private Sprite defaultSprite;
        private RectTransform itemPosition;

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
            Bind<TextMeshProUGUI>(typeof(Texts));

            itemImage = GetImage((int)Images.ItemImage);
            itemCount = GetText((int)Texts.ItemCount);
            itemPosition = itemImage.GetComponent<RectTransform>();

            gameObject.BindEvent(OnDropHandler, UIEvent.Drop);
        }

        private void InitObjects()
        {
            rect = GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(rect, AnchorPresets.MiddleCenter);
            rect.sizeDelta = InventoryConst.SlotRect.GetSize();
            rect.localPosition = InventoryConst.SlotRect.ToCanvasPos();

            itemCount.lineSpacing = lineHeight;
            itemCount.fontSize = fontSize;
            itemCount.color = Color.white;
            itemCount.alignment = TextAlignmentOptions.MidlineRight;
            itemCount.text = string.Empty;

            defaultSprite = itemImage.sprite;
        }

        // !TODO: 스위칭 전용 슬롯 추가 필요
        //      - 스위칭 슬롯 -> 아이템 슬롯: 이동됨, 스위칭 슬롯에서 빠짐
        //      - 아이템 슬롯 -> 스위칭 슬롯: 복사됨, 아이템 슬롯에는 그대로 있음
        //      - 스위칭 슬롯에 등록되면 static ui에 등록된 아이템 표시되어야 함

        // 슬롯에 아이템 장착
        // 아이템 정보, 슬롯 인덱스
        private void OnDropHandler(PointerEventData data)
        {
            if (SlotType == SlotAreaType.Description)
                return;

            var droppedItem = data.pointerDrag;
            var baseSlotItem = droppedItem.GetComponent<BaseSlotItem>();
            if (baseSlotItem == null)
                return;

            var payload = new InventoryEventPayload
            {
                baseItem = baseSlotItem,
                slotIndex = Index
            };

            Debug.Log($"{baseSlotItem.name} - isOrigin: {baseSlotItem.IsOrigin()}");

            // 장착 슬롯, 아이템 슬롯 구분 필요
            // origin item
            if (baseSlotItem.IsOrigin())
            {
                if (SlotType == SlotAreaType.Equipment)
                {
                    // copy
                    // !TODO: 카피본 만들 때 inventory를 drag parent 등록
                    // !TODO: 카피본 만드는 이벤트를 Inventory로 전달해서 생성할 때 dragParent 등록시켜주기
                    // !TODO: 카피본 만들 때, 다른 슬롯에 등록되어 있는 아이템의 경우 위치만 옮겨줘야 함
                    Debug.Log($"copy 발생");
                    var copy = UIManager.Instance.MakeSubItem<InventorySlotCopyItem>(transform, InventorySlotCopyItem.Path);
                    copy.SetCopyItem(baseSlotItem.SlotItem);
                    copy.SetOnDragParent(UIManager.Instance.OnDragParent);
                    copy.SetSlot(itemPosition);
                }
                else
                {
                    // move
                    baseSlotItem.SetSlot(itemPosition);
                }
            }
            // copy item
            else
            {
                if (SlotType == SlotAreaType.Equipment)
                {
                    // move
                    baseSlotItem.SetSlot(itemPosition);
                }
                else
                {
                    // do nothing
                }
            }

            slotInventoryAction?.Invoke(payload);
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