using System;
using Assets.Scripts.Item;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using Data.UI.Inventory;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts.UI.Inventory
{
    public delegate void ToggleChangeHandler(ToggleChangeInfo changeInfo);

    public struct InventoryEventPayload
    {
        public InventorySlotItem slotItem;
        public int slotIndex;
        public SlotAreaType slotAreaType;
        public GroupType groupType;
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

        private void Awake()
        {
            Init();
        }

        [SerializeField] private InventoryChannel inventoryChannel;

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

        [Tooltip("Slot Area Grid")]
        [SerializeField]
        private int row = 3;

        [SerializeField] private int col = 8;
        [SerializeField] private int padding = 1;
        [SerializeField] private int spacing = 2;

        private TicketMachine ticketMachine;

        private bool isOpened = false;

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
            var descName = UIManager.Instance.MakeSubItem<DescriptionNamePanel>(transform, UIManager.DescriptionNamePanel);
            descName.transform.SetParent(descriptionPanel.transform);

            var descText = UIManager.Instance.MakeSubItem<DescriptionTextPanel>(transform, UIManager.DescriptionTextPanel);
            descText.transform.SetParent(descriptionPanel.transform);

            var slot = UIManager.Instance.MakeSubItem<InventorySlot>(transform, UIManager.InventorySlot);
            slot.transform.SetParent(descriptionPanel.transform);

            closeButton = UIManager.Instance.MakeSubItem<CloseButton>(transform, CloseButton.Path);
            SetValues(closeButton.transform, transform, AnchorPresets.MiddleCenter, InventoryConst.CloseButtonRect);
            closeButton.Subscribe(OnCloseButtonClickAction);

            buttonPanel.ActivateToggle(GroupType.Stone, true);
        }

        private void InitButtonPanel()
        {
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
                slotArea.SlotAreaType = SlotAreaType.Item;
                slotArea.MakeSlots();

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
                slotArea.SlotAreaType = SlotAreaType.Equipment;
                slotArea.MakeSlots();

                buttonPanel.AddSlotArea(SlotAreaType.Equipment, slotArea);
            }
        }

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

        private void OnCloseButtonClickAction()
        {
            isOpened = false;
            gameObject.SetActive(false);
        }

        private void OnNotifyAction(IBaseEventPayload payload)
        {
            if (payload is not UIPayload uiPayload)
                return;

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

        private void OnPanelInventoryAction(InventoryEventPayload payload)
        {
        }
    }
}