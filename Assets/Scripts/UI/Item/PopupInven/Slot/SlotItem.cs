using Assets.Scripts.ActionData;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// !TODO
// - 드래그 앤 드롭을 할 때, 슬롯이 아닌 위치에서 드롭을 하면 원래 자리로 돌아가야 함
// - 슬롯 아이템이 생성될 때, 
namespace Assets.Scripts.UI.Item.PopupInven
{
    public class SlotItem : UIBase, IDraggable
    {
        private enum Images
        {
            ItemImage
        }

        private enum Texts
        {
            ItemText
        }

        private RectTransform rectTransform;
        private Image raycastImage;

        private Image itemImage;
        private TextMeshProUGUI itemText;

        public Image ItemImage => itemImage;
        public TextMeshProUGUI ItemText => itemText;
        public string ItemName { get; set; }
        public int ItemCount { get; set; }

        private Slot slot;

        private readonly Data<SlotInfo> slotInfo = new Data<SlotInfo>();

        private void Awake()
        {
            Init();
        }

        private void OnEnable()
        {
            slotInfo.Subscribe(OnSlotInfoChanged);
        }

        protected override void Init()
        {
            BindObjects();
            InitObjects();
            BindEvents();
        }

        private void BindObjects()
        {
            Bind<Image>(typeof(Images));
            Bind<TextMeshProUGUI>(typeof(Texts));
        }

        private void InitObjects()
        {
            raycastImage = gameObject.GetComponent<Image>();
            rectTransform = gameObject.GetOrAddComponent<RectTransform>();
            itemImage = GetImage((int)Images.ItemImage);
            itemText = GetText((int)Texts.ItemText);
        }

        private void BindEvents()
        {
            gameObject.BindEvent(OnBeginDragHandler, UIEvent.BeginDrag);
            gameObject.BindEvent(OnDragHandler, UIEvent.Drag);
            gameObject.BindEvent(OnEndDragHandler, UIEvent.EndDrag);
        }

        // !TODO: 드래그 할 때 색, 크기, 알파 변환
        // Slot에서 OnDrop을 받기 위해서는 Raycast를 받을 수 있게 해줘야 하기 때문에 SlotItem의 raycastTarget을 비활성화 처리
        private void OnBeginDragHandler(PointerEventData data)
        {
            raycastImage.raycastTarget = false;
        }

        private void OnDragHandler(PointerEventData data)
        {
            transform.position = data.position;
        }

        private void OnEndDragHandler(PointerEventData data)
        {
            raycastImage.raycastTarget = true;
        }

        public SlotInfo GetSlotInfo()
        {
            return slotInfo.Value;
        }

        public void SetSlotInfo(SlotInfo info)
        {
            if (slotInfo.Value.slot)
            {
                slotInfo.Value.slot.IsUsed = false;
                slotInfo.Value.slot.slotItem = null;
            }

            slotInfo.Value = info;
        }

        private void OnSlotInfoChanged(SlotInfo info)
        {
            ResetSlotItem(info);
        }

        private void ResetSlotItem(SlotInfo info)
        {
            rectTransform.SetParent(info.slot.ItemPosition);

            AnchorPresets.SetAnchorPreset(rectTransform, AnchorPresets.StretchAll);
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.localPosition = Vector2.zero;
            rectTransform.localScale = Vector3.one;
        }

        public void AddCount(int count)
        {
            ItemCount++;
            itemText.text = ItemCount.ToString();
        }
    }
}