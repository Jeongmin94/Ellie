using System;
using Assets.Scripts.Item.Goods;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts.UI.Inventory
{
    public delegate void ToggleChangeHandler(ToggleChangeInfo changeInfo);

    public enum InventoryEventType
    {
        MoveItem,
        CopyItemWithDrag,
        CopyItemWithShortCut,
        ShowDescription,
        SortSlotArea,
    }

    public struct InventoryEventPayload
    {
        public InventoryEventType eventType;
        public SlotAreaType slotAreaType; // 슬롯 타입: Item, Equipment, Description
        public GroupType groupType;       // 아이템 타입: Consumption, Stone, Etc
        public BaseSlotItem baseItem;     // 슬롯 아이템 정보
        public InventorySlot slot;        // 슬롯 위치
    }

    public class Inventory : UIPopup
    {
        private enum GameObjects
        {
            DescriptionPanel,
            CategoryPanel,
            GoldAndStonePiecePanel,
            CategoryButtonPanel, // toggle group

            ItemSlots,
            EquipmentSlots,
            OuterRim
        }

        private enum Images
        {
            DescriptionImageArea,
        }

        [SerializeField] private GameGoods goods;

        // GameObject
        private GameObject descriptionPanel;
        private GameObject categoryPanel;
        private GameObject goldAndStonePiecePanel;
        private GameObject categoryButtonPanel;

        private GameObject itemSlots;
        private GameObject equipmentSlots;
        private GameObject outerRim;

        // Image
        private Image descImageArea;

        // Category Toggle Button
        private CategoryButtonPanel buttonPanel;

        // Close Button
        private CloseButton closeButton;

        // Description
        private DescriptionNamePanel descriptionNamePanel;
        private DescriptionTextPanel descriptionTextPanel;
        private InventorySlot descriptionSlot;

        [Tooltip("Slot Area Grid")]
        [SerializeField]
        private int row = 3;

        [SerializeField] private int col = 8;
        [SerializeField] private int padding = 1;
        [SerializeField] private int spacing = 2;

        private TicketMachine ticketMachine;

        private bool isOpened = false;

        private InventorySlot swapBuffer;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();

            Bind();
            InitObjects();
            InitTicketMachine();
        }

        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));
            Bind<Image>(typeof(Images));

            descriptionPanel = GetGameObject((int)GameObjects.DescriptionPanel);
            categoryPanel = GetGameObject((int)GameObjects.CategoryPanel);
            goldAndStonePiecePanel = GetGameObject((int)GameObjects.GoldAndStonePiecePanel);
            categoryButtonPanel = GetGameObject((int)GameObjects.CategoryButtonPanel);

            itemSlots = GetGameObject((int)GameObjects.ItemSlots);
            equipmentSlots = GetGameObject((int)GameObjects.EquipmentSlots);
            outerRim = GetGameObject((int)GameObjects.OuterRim);

            descImageArea = GetImage((int)Images.DescriptionImageArea);

            buttonPanel = categoryButtonPanel.GetOrAddComponent<CategoryButtonPanel>();
            buttonPanel.InitCategoryButtonPanel();
            buttonPanel.Subscribe(OnPanelInventoryAction);
        }

        private void InitObjects()
        {
            var descRect = descriptionPanel.GetComponent<RectTransform>();
            var ctgyRect = categoryPanel.GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(descRect, AnchorPresets.MiddleCenter);
            AnchorPresets.SetAnchorPreset(ctgyRect, AnchorPresets.MiddleCenter);
            descRect.sizeDelta = InventoryConst.DescRect.GetSize();
            ctgyRect.sizeDelta = InventoryConst.CtgyRect.GetSize();
            descRect.localPosition = InventoryConst.DescRect.ToCanvasPos();
            ctgyRect.localPosition = InventoryConst.CtgyRect.ToCanvasPos();

            var descImageRect = descImageArea.GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(descImageRect, AnchorPresets.MiddleCenter);
            descImageRect.sizeDelta = InventoryConst.DescImageRect.GetSize();
            descImageRect.localPosition = InventoryConst.DescImageRect.ToCanvasPos();
            descImageRect.SetParent(descriptionPanel.transform);

            InitButtonPanel();

            var goldRect = goldAndStonePiecePanel.GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(goldRect, AnchorPresets.MiddleCenter);
            goldRect.sizeDelta = InventoryConst.GoldRect.GetSize();
            goldRect.localPosition = InventoryConst.GoldRect.ToCanvasPos();
            goldRect.SetParent(categoryPanel.transform);

            var goldArea = UIManager.Instance.MakeSubItem<ImageAndTextArea>(transform, UIManager.ImageAndTextArea);
            goldArea.Rect.sizeDelta = Vector2.zero;
            goldArea.Rect.localPosition = Vector3.zero;

            SetValues(goldArea.Image.GetComponent<RectTransform>(), goldArea.transform, AnchorPresets.MiddleCenter, InventoryConst.GoldAreaRect);
            SetValues(goldArea.Text.GetComponent<RectTransform>(), goldArea.transform, AnchorPresets.MiddleCenter, InventoryConst.GoldAreaCountRect);

            goldArea.transform.SetParent(goldRect.transform);

            var stoneArea = UIManager.Instance.MakeSubItem<ImageAndTextArea>(transform, UIManager.ImageAndTextArea);
            stoneArea.Rect.sizeDelta = Vector2.zero;
            stoneArea.Rect.localPosition = Vector3.zero;

            SetValues(stoneArea.Image.GetComponent<RectTransform>(), stoneArea.transform, AnchorPresets.MiddleCenter, InventoryConst.StonePieceAreaRect);
            SetValues(stoneArea.Text.GetComponent<RectTransform>(), stoneArea.transform, AnchorPresets.MiddleCenter, InventoryConst.StonePieceAreaCountRect);

            stoneArea.transform.SetParent(goldRect.transform);

            // description panel
            descriptionNamePanel = UIManager.Instance.MakeSubItem<DescriptionNamePanel>(transform, UIManager.DescriptionNamePanel);
            descriptionNamePanel.transform.SetParent(descriptionPanel.transform);

            descriptionTextPanel = UIManager.Instance.MakeSubItem<DescriptionTextPanel>(transform, UIManager.DescriptionTextPanel);
            descriptionTextPanel.transform.SetParent(descriptionPanel.transform);

            descriptionSlot = UIManager.Instance.MakeSubItem<InventorySlot>(transform, UIManager.InventorySlot);
            descriptionSlot.SlotType = SlotAreaType.Description;
            descriptionSlot.transform.SetParent(descriptionPanel.transform);

            closeButton = UIManager.Instance.MakeSubItem<CloseButton>(transform, CloseButton.Path);
            SetValues(closeButton.transform, transform, AnchorPresets.MiddleCenter, InventoryConst.CloseButtonRect);
            closeButton.Subscribe(OnCloseButtonClickAction);
        }

        private void InitTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.UI);
            ticketMachine.RegisterObserver(ChannelType.UI, OnNotifyAction);

            TicketManager.Instance.Ticket(ticketMachine);
        }

        private void Start()
        {
            buttonPanel.ActivateToggle(GroupType.Stone, true);
            OnCloseButtonClickAction();
        }

        private void InitButtonPanel()
        {
            swapBuffer = UIManager.Instance.MakeSubItem<InventorySlot>(outerRim.transform, UIManager.InventorySlot);
            swapBuffer.name = "SwapBuffer";
            swapBuffer.Subscribe(OnPanelInventoryAction);
            UIManager.Instance.slotSwapBuffer = swapBuffer;

            InitItemArea();
            InitEquipmentArea();
            buttonPanel.Subscribe(ToggleChangeCallback);
        }

        private void InitItemArea()
        {
            var groupTypes = Enum.GetValues(typeof(GroupType));
            for (int i = 0; i < groupTypes.Length; i++)
            {
                var slotArea = UIManager.Instance.MakeSubItem<InventorySlotArea>(outerRim.transform, InventorySlotArea.Path);

                slotArea.InitSlotRect(itemSlots.transform, InventoryConst.SlotAreaRect);
                slotArea.InitGridLayoutGroup(row, col, padding, spacing, GridLayoutGroup.Corner.UpperLeft, GridLayoutGroup.Axis.Horizontal, TextAnchor.MiddleCenter);
                slotArea.MakeSlots(SlotAreaType.Item);

                buttonPanel.AddSlotArea(SlotAreaType.Item, slotArea);
            }
        }

        private readonly int equipmentRow = 1;
        private readonly int[] equipmentCols = new[] { 4, 5, 1 };

        private void InitEquipmentArea()
        {
            var groupTypes = Enum.GetValues(typeof(GroupType));
            for (int i = 0; i < groupTypes.Length; i++)
            {
                var slotArea = UIManager.Instance.MakeSubItem<InventorySlotArea>(outerRim.transform, InventorySlotArea.Path);
                slotArea.InitSlotRect(equipmentSlots.transform, InventoryConst.EquipSlotAreaRect);
                slotArea.InitGridLayoutGroup(equipmentRow, equipmentCols[i], padding, spacing, GridLayoutGroup.Corner.UpperLeft, GridLayoutGroup.Axis.Horizontal, TextAnchor.MiddleCenter);
                slotArea.MakeSlots(SlotAreaType.Equipment);

                buttonPanel.AddSlotArea(SlotAreaType.Equipment, slotArea);
            }
        }

        #region ToggleEvent

        private void ToggleChangeCallback(ToggleChangeInfo changeInfo)
        {
            var target = changeInfo.IsOn ? transform : outerRim.transform;
            if (changeInfo.IsOn)
            {
                buttonPanel.MoveSlotArea(SlotAreaType.Item, changeInfo.Type, target, itemSlots.transform, InventoryConst.SlotAreaRect);
                buttonPanel.MoveSlotArea(SlotAreaType.Equipment, changeInfo.Type, target, equipmentSlots.transform, InventoryConst.EquipSlotAreaRect);
            }
            else
            {
                buttonPanel.MoveSlotArea(SlotAreaType.Item, changeInfo.Type, target, itemSlots.transform, InventoryConst.SlotAreaRect);
                buttonPanel.MoveSlotArea(SlotAreaType.Equipment, changeInfo.Type, target, equipmentSlots.transform, InventoryConst.EquipSlotAreaRect);
            }
        }

        #endregion

        #region CloseButtonEvent

        private void OnCloseButtonClickAction()
        {
            isOpened = false;
            gameObject.SetActive(false);
        }

        #endregion


        #region UIChannelEvent

        private void OnNotifyAction(IBaseEventPayload payload)
        {
            if (payload is not UIPayload uiPayload)
                return;

            if (uiPayload.actionType == ActionType.ToggleInventory)
            {
                ToggleInventory();
                return;
            }

            if (uiPayload.actionType == ActionType.AddSlotItem)
            {
                AddItem(uiPayload);
                return;
            }

            if (uiPayload.actionType == ActionType.ConsumeSlotItem)
            {
                ConsumeItem(uiPayload);
                return;
            }
        }

        private void ToggleInventory()
        {
            if (isOpened)
            {
                OnCloseButtonClickAction();
            }
            else
            {
                isOpened = true;
                gameObject.SetActive(true);
            }
        }

        private void AddItem(UIPayload payload)
        {
            payload.onDragParent = transform;
            buttonPanel.AddItem(payload.slotAreaType, payload.itemData.groupType, payload);
        }

        private void ConsumeItem(UIPayload payload)
        {
            buttonPanel.ConsumeItem(payload.slotAreaType, payload.itemData.groupType, payload);
        }

        #endregion

        #region InventoryEvent

        private void OnPanelInventoryAction(InventoryEventPayload payload)
        {
            switch (payload.eventType)
            {
                case InventoryEventType.MoveItem:
                {
                    payload.baseItem.MoveSlot(payload.slot.SlotItemPosition, payload.baseItem.SlotItemData);
                    payload.baseItem.ChangeSlot(payload.slot.SlotType, payload.slot);
                }
                    break;

                case InventoryEventType.CopyItemWithDrag:
                {
                    var baseSlotItem = payload.baseItem;
                    var slot = payload.slot;

                    var copy = UIManager.Instance.MakeSubItem<InventorySlotCopyItem>(slot.transform, InventorySlotCopyItem.Path);
                    copy.MoveSlot(slot.SlotItemPosition, baseSlotItem.SlotItemData);
                    copy.SetOnDragParent(transform);

                    baseSlotItem.ChangeSlot(slot.SlotType, slot);
                    baseSlotItem.ChangeSlotItem(slot.SlotType, copy);
                }
                    break;

                case InventoryEventType.CopyItemWithShortCut:
                {
                    var baseSlotItem = payload.baseItem;
                    var slot = payload.slot;

                    var copy = UIManager.Instance.MakeSubItem<InventorySlotCopyItem>(slot.transform, InventorySlotCopyItem.Path);
                    copy.MoveSlot(slot.SlotItemPosition, baseSlotItem.SlotItemData);
                    copy.SetOnDragParent(transform);

                    baseSlotItem.ChangeSlot(slot.SlotType, slot);
                    baseSlotItem.ChangeSlotItem(slot.SlotType, copy);
                }
                    break;

                case InventoryEventType.ShowDescription:
                {
                    var baseSlotItem = payload.baseItem;
                    descriptionNamePanel.SetDescriptionName(baseSlotItem.SlotItemData.ItemName);
                    descriptionTextPanel.SetDescriptionText(baseSlotItem.SlotItemData.itemData.description);
                    descriptionSlot.SetSprite(payload.baseItem.SlotItemData.ItemSprite);
                    descriptionSlot.SetViewMode(true);
                }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}