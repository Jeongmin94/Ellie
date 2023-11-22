using System;
using System.Collections.Generic;
using Assets.Scripts.Data.UI.Transform;
using Assets.Scripts.Item;
using Assets.Scripts.Item.Goods;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Equipment;
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
    public delegate void ActivateButtonPanelHandler(ToggleChangeInfo changeInfo);

    public enum InventoryEventType
    {
        MoveItem,
        CopyItemWithDrag,
        CopyItemWithShortCut,
        ShowDescription,
        SortSlotArea,
        EquipItem,
        UnEquipItem,
        UpdateEquipItem,
        SendMessageToPlayer,
    }

    public struct InventoryEventPayload
    {
        public InventoryEventType eventType;
        public SlotAreaType slotAreaType; // 슬롯 타입: Item, Equipment, Description
        public GroupType groupType;       // 아이템 타입: Consumption, Stone, Etc
        public BaseSlotItem baseSlotItem; // 슬롯 아이템 정보
        public InventorySlot slot;        // 슬롯 위치
        public BaseItem baseItem;         // 아이템 정보
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

        [SerializeField] private UITransformData consumptionTransformData;
        [SerializeField] private UITransformData stoneTransformData;
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

        // GameGoods
        private ImageAndTextArea goldImageAndTextArea;
        private ImageAndTextArea stonePieceImageAndTextArea;

        // Frame Canvas
        private FrameCanvas consumptionCanvas;
        private FrameCanvas stoneCanvas;
        private readonly IDictionary<GroupType, FrameCanvas> frameCanvasMap = new Dictionary<GroupType, FrameCanvas>();

        [Tooltip("Slot Area Grid")]
        [SerializeField]
        private int row = 3;

        [SerializeField] private int col = 8;
        [SerializeField] private int padding = 1;
        [SerializeField] private int spacing = 2;

        private TicketMachine ticketMachine;

        private bool isOpened = false;
        public bool IsOpened => isOpened;

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

            InitDescriptionPanel();
            InitButtonPanel();
            InitGoodsPanel();

            closeButton = UIManager.Instance.MakeSubItem<CloseButton>(transform, CloseButton.Path);
            SetValues(closeButton.transform, transform, AnchorPresets.MiddleCenter, InventoryConst.CloseButtonRect);
            closeButton.Subscribe(OnCloseButtonClickAction);

            InitConsumptionCanvas();
            InitStoneCanvas();
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

        private void InitGoodsPanel()
        {
            var goldRect = goldAndStonePiecePanel.GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(goldRect, AnchorPresets.MiddleCenter);
            goldRect.sizeDelta = InventoryConst.GoldRect.GetSize();
            goldRect.localPosition = InventoryConst.GoldRect.ToCanvasPos();
            goldRect.SetParent(categoryPanel.transform);

            goldImageAndTextArea = UIManager.Instance.MakeSubItem<ImageAndTextArea>(transform, UIManager.ImageAndTextArea);
            goldImageAndTextArea.Rect.sizeDelta = Vector2.zero;
            goldImageAndTextArea.Rect.localPosition = Vector3.zero;
            goldImageAndTextArea.Image.sprite = ResourceManager.Instance.LoadSprite(ImageAndTextArea.GoldPath);

            SetValues(goldImageAndTextArea.Image.GetComponent<RectTransform>(), goldImageAndTextArea.transform, AnchorPresets.MiddleCenter, InventoryConst.GoldAreaRect);
            SetValues(goldImageAndTextArea.Text.GetComponent<RectTransform>(), goldImageAndTextArea.transform, AnchorPresets.MiddleCenter, InventoryConst.GoldAreaCountRect);

            goldImageAndTextArea.transform.SetParent(goldRect.transform);
            goods.gold.Subscribe(goldImageAndTextArea.OnGoodsCountChanged);

            stonePieceImageAndTextArea = UIManager.Instance.MakeSubItem<ImageAndTextArea>(transform, UIManager.ImageAndTextArea);
            stonePieceImageAndTextArea.Rect.sizeDelta = Vector2.zero;
            stonePieceImageAndTextArea.Rect.localPosition = Vector3.zero;
            stonePieceImageAndTextArea.Image.sprite = ResourceManager.Instance.LoadSprite(ImageAndTextArea.StonePiecePath);

            SetValues(stonePieceImageAndTextArea.Image.GetComponent<RectTransform>(), stonePieceImageAndTextArea.transform, AnchorPresets.MiddleCenter, InventoryConst.StonePieceAreaRect);
            SetValues(stonePieceImageAndTextArea.Text.GetComponent<RectTransform>(), stonePieceImageAndTextArea.transform, AnchorPresets.MiddleCenter, InventoryConst.StonePieceAreaCountRect);

            stonePieceImageAndTextArea.transform.SetParent(goldRect.transform);
            goods.stonePiece.Subscribe(stonePieceImageAndTextArea.OnGoodsCountChanged);
        }

        private void InitDescriptionPanel()
        {
            // description panel
            descriptionNamePanel = UIManager.Instance.MakeSubItem<DescriptionNamePanel>(transform, UIManager.DescriptionNamePanel);
            descriptionNamePanel.transform.SetParent(descriptionPanel.transform);

            descriptionTextPanel = UIManager.Instance.MakeSubItem<DescriptionTextPanel>(transform, UIManager.DescriptionTextPanel);
            descriptionTextPanel.transform.SetParent(descriptionPanel.transform);

            descriptionSlot = UIManager.Instance.MakeSubItem<InventorySlot>(transform, UIManager.InventorySlot);
            descriptionSlot.SlotType = SlotAreaType.Description;
            descriptionSlot.transform.SetParent(descriptionPanel.transform);
        }

        private void InitButtonPanel()
        {
            swapBuffer = UIManager.Instance.MakeSubItem<InventorySlot>(outerRim.transform, UIManager.InventorySlot);
            swapBuffer.name = "SwapBuffer";
            swapBuffer.Subscribe(OnPanelInventoryAction);
            UIManager.Instance.slotSwapBuffer = swapBuffer;

            InitItemArea();
            InitEquipmentArea();
            buttonPanel.Subscribe(ActivateButtonPanelCallback);
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

        private void ActivateButtonPanelCallback(ToggleChangeInfo changeInfo)
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
            UIPayload payload = new()
            {
                uiType = UIType.Notify,
                actionType = ActionType.ClickCloseButton,
            };
            ticketMachine.SendMessage(ChannelType.UI, payload);
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

            if (uiPayload.actionType == ActionType.MoveClockwise ||
                uiPayload.actionType == ActionType.MoveCounterClockwise)
            {
                MoveEquipmentSlot(uiPayload);
                return;
            }
            // !TODO : 플레이어 돌맹이 불 프로퍼티에 대한 로직 작성

        }

        private void MoveEquipmentSlot(UIPayload payload)
        {
            buttonPanel.MoveItem(payload.slotAreaType, payload);
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
                // 아이템 이동
                case InventoryEventType.MoveItem:
                {
                    payload.baseSlotItem.MoveSlot(payload.slot.SlotItemPosition, payload.baseSlotItem.SlotItemData);
                    payload.baseSlotItem.ChangeSlot(payload.slot.SlotType, payload.slot);
                }
                    break;

                // 드래그 앤 드롭으로 장착
                case InventoryEventType.CopyItemWithDrag:
                {
                    var baseSlotItem = payload.baseSlotItem;
                    var slot = payload.slot;

                    var copy = UIManager.Instance.MakeSubItem<InventorySlotCopyItem>(slot.transform, InventorySlotCopyItem.Path);
                    copy.InitBaseSlotItem();
                    copy.MoveSlot(slot.SlotItemPosition, baseSlotItem.SlotItemData);
                    copy.SetOnDragParent(transform);

                    baseSlotItem.ChangeSlot(slot.SlotType, slot);
                    baseSlotItem.ChangeSlotItem(slot.SlotType, copy);
                }
                    break;

                // 우클릭으로 장착
                case InventoryEventType.CopyItemWithShortCut:
                {
                    var baseSlotItem = payload.baseSlotItem;
                    var slot = payload.slot;

                    var copy = UIManager.Instance.MakeSubItem<InventorySlotCopyItem>(slot.transform, InventorySlotCopyItem.Path);
                    copy.InitBaseSlotItem();
                    copy.MoveSlot(slot.SlotItemPosition, baseSlotItem.SlotItemData);
                    copy.SetOnDragParent(transform);

                    baseSlotItem.ChangeSlot(slot.SlotType, slot);
                    baseSlotItem.ChangeSlotItem(slot.SlotType, copy);
                }
                    break;

                // 설명창에 아이템 표시
                case InventoryEventType.ShowDescription:
                {
                    var baseSlotItem = payload.baseSlotItem;
                    descriptionNamePanel.SetDescriptionName(baseSlotItem.SlotItemData.ItemName);
                    descriptionTextPanel.SetDescriptionText(baseSlotItem.SlotItemData.itemData.description);
                    descriptionSlot.SetSprite(payload.baseSlotItem.SlotItemData.ItemSprite);
                    descriptionSlot.SetViewMode(true);
                }
                    break;

                // 아이템 처음 습득시 카피 + 장착
                case InventoryEventType.EquipItem:
                {
                    // 1. 장착 슬롯에 아이템 장착 요청하기
                    var baseSlotItem = payload.baseSlotItem;
                    var slot = payload.slot;

                    var copy = UIManager.Instance.MakeSubItem<InventorySlotCopyItem>(slot.transform, InventorySlotCopyItem.Path);
                    copy.InitBaseSlotItem();
                    copy.MoveSlot(slot.SlotItemPosition, baseSlotItem.SlotItemData);
                    copy.SetOnDragParent(transform);

                    baseSlotItem.ChangeSlot(slot.SlotType, slot);
                    baseSlotItem.ChangeSlotItem(slot.SlotType, copy);
                }
                    break;

                case InventoryEventType.UpdateEquipItem:
                {
                    payload.slot.InvokeEquipmentFrameAction(payload);
                }
                    break;

                case InventoryEventType.SendMessageToPlayer:
                {
                    // !TODO : UIChannel에 플레이어의 has 변수를 바꿔줄 이벤트 쏴야됨
                        Debug.Log("Inventory: " + payload.slot.name);
                    
                    ticketMachine.SendMessage(ChannelType.UI, GeneratePayloadToPlayer(payload));
                }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private UIPayload GeneratePayloadToPlayer(InventoryEventPayload payload)
        {
            UIPayload uiPayload = new UIPayload();
            uiPayload.uiType = UIType.Notify;
            Debug.Log("GeneratePayloadToPlayer groupType : " +  payload.groupType);
            if(payload.slot.SlotItemData != null)
                uiPayload.itemData = payload.slot.SlotItemData.itemData;
            uiPayload.actionType = ActionType.SetPlayerProperty;
            uiPayload.groupType = payload.groupType;
            if(payload.slot.SlotItemData == null)
                uiPayload.isStoneNull = true;
            else
            {
                //uiPayload.itemData = payload.slot.SlotItemData.itemData;
                uiPayload.isStoneNull = false;
            }
            return uiPayload;
        }
        #endregion

        #region FrameCanvas

        private void InitConsumptionCanvas()
        {
            consumptionCanvas = UIManager.Instance.MakeStatic<FrameCanvas>(FrameCanvas.Path);
            consumptionCanvas.FrameWidth = 86.0f;
            consumptionCanvas.FrameHeight = 86.0f;
            consumptionCanvas.FrameImage = ResourceManager.Instance.LoadSprite("UI/Item/Equipment/ConsumptionFrameRotated");

            consumptionCanvas.InitFrameCanvas(consumptionTransformData);

            Vector2[] directions =
            {
                new Vector2(0.0f, consumptionCanvas.FrameHeight / 2.0f),
                new Vector2(-consumptionCanvas.FrameWidth / 2.0f, 0.0f),
                new Vector2(0.0f, -consumptionCanvas.FrameHeight / 2.0f),
                new Vector2(consumptionCanvas.FrameWidth / 2.0f, 0.0f),
            };

            var consumptionArea = buttonPanel.GetSlotArea(SlotAreaType.Equipment, GroupType.Consumption);
            consumptionCanvas.InitFrame(directions, EquipmentFrame.DefaultPath);
            consumptionCanvas.RegisterObservers(consumptionArea.GetSlots());
            consumptionCanvas.groupType = GroupType.Consumption;

            frameCanvasMap.TryAdd(GroupType.Consumption, consumptionCanvas);
        }

        private void InitStoneCanvas()
        {
            stoneCanvas = UIManager.Instance.MakeStatic<FrameCanvas>(FrameCanvas.Path);
            stoneCanvas.FrameWidth = 113.0f;
            stoneCanvas.FrameHeight = 113.0f;
            stoneCanvas.FrameImage = ResourceManager.Instance.LoadSprite("UI/Item/Equipment/StoneFrame");

            stoneCanvas.InitFrameCanvas(stoneTransformData);

            const float INF = 9999.0f;
            Vector2[] directions =
            {
                new Vector2(0.0f, stoneCanvas.FrameHeight / 2.0f),
                new Vector2(-stoneCanvas.FrameWidth / 2.0f, -stoneCanvas.FrameHeight / 2.0f),
                new Vector2(stoneCanvas.FrameWidth / 2.0f, -stoneCanvas.FrameHeight / 2.0f),
                // new Vector2(-stoneCanvas.FrameWidth / 2.0f, -stoneCanvas.FrameHeight * 1.5f),
                // new Vector2(stoneCanvas.FrameWidth / 2.0f, -stoneCanvas.FrameHeight * 1.5f),
                new Vector2(INF, INF),
                new Vector2(INF, INF),
            };

            var stoneArea = buttonPanel.GetSlotArea(SlotAreaType.Equipment, GroupType.Stone);
            stoneCanvas.InitFrame(directions, EquipmentFrame.StonePath);
            stoneCanvas.RegisterObservers(stoneArea.GetSlots());
            stoneCanvas.groupType = GroupType.Stone;

            frameCanvasMap.TryAdd(GroupType.Stone, stoneCanvas);
        }

        #endregion
    }
}