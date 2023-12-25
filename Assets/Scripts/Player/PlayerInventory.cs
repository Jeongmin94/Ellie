using Assets.Scripts.Managers;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using Data.GoogleSheet._4000Stone;
using Data.GoogleSheet._4100Item;
using Data.Item.Goods;
using Item;
using Managers.Data;
using Managers.Input;
using Managers.Sound;
using Managers.UI;
using UI.Inventory;
using UI.Inventory.CategoryPanel;
using UI.Inventory.Slot;
using UnityEngine;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        private const int CONSUMABLEEQUIPMENTSLOTCOUNT = 4;

        public ItemMetaData[] consumableEquipmentSlot = new ItemMetaData[CONSUMABLEEQUIPMENTSLOTCOUNT];
        public int curSlotIdx;
        public bool canUseConsumable;
        public int itemIdx;
        [SerializeField] private GameGoods gameGoods;

        public bool isOpen;
        private PlayerController controller;
        private PlayerStatus playerStatus;
        private TicketMachine ticketMachine;

        public Inventory Inventory { get; private set; }

        private void Awake()
        {
            Inventory = UIManager.Instance.MakePopup<Inventory>(UIManager.Inventory);
            gameGoods.Init();
        }

        private void Start()
        {
            controller = GetComponent<PlayerController>();
            playerStatus = GetComponent<PlayerStatus>();
            ticketMachine = controller.TicketMachine;
            isOpen = false;
        }

        private void Update()
        {
            if (!InputManager.Instance.CanInput)
            {
                return;
            }

            if (controller.GetCurState() == PlayerStateName.Loading)
            {
                return;
            }

            //인벤토리 열고 닫기
            if (Input.GetKeyDown(KeyCode.I))
            {
                ticketMachine.SendMessage(ChannelType.UI, MakeInventoryOpenPayload());
                OnInventoryToggle();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                ticketMachine.SendMessage(ChannelType.UI, MakeConsumeItemCWPayload());
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                ticketMachine.SendMessage(ChannelType.UI, MakeConsumeItemCCWPayload());
            }

            var wheelInput = Input.mouseScrollDelta;

            if (wheelInput != Vector2.zero)
            {
                if (wheelInput.y > 0)
                {
                    ticketMachine.SendMessage(ChannelType.UI, MakeStoneItemCCWPayload());
                }
                else
                {
                    ticketMachine.SendMessage(ChannelType.UI, MakeStoneItemCWPayload());
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (itemIdx == 0)
                {
                    SoundManager.Instance.PlaySound(SoundManager.SoundType.UISfx, "ellie_sound9");
                    return;
                }

                if (controller.GetCurState() == PlayerStateName.Idle ||
                    controller.GetCurState() == PlayerStateName.Walk ||
                    controller.GetCurState() == PlayerStateName.Sprint)
                {
                    controller.ChangeState(PlayerStateName.ConsumingItem);
                }
            }
        }

        private UIPayload MakeInventoryOpenPayload()
        {
            var payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.ToggleInventory;

            return payload;
        }

        private UIPayload MakeConsumeItemCCWPayload()
        {
            var payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.MoveCounterClockwise;
            payload.groupType = GroupType.Item;
            payload.slotAreaType = SlotAreaType.Equipment;

            return payload;
        }

        private UIPayload MakeConsumeItemCWPayload()
        {
            var payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.MoveClockwise;
            payload.groupType = GroupType.Item;
            payload.slotAreaType = SlotAreaType.Equipment;

            return payload;
        }

        private UIPayload MakeStoneItemCCWPayload()
        {
            var payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.MoveCounterClockwise;
            payload.groupType = GroupType.Stone;
            payload.slotAreaType = SlotAreaType.Equipment;

            return payload;
        }

        private UIPayload MakeStoneItemCWPayload()
        {
            var payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.MoveClockwise;
            payload.groupType = GroupType.Stone;
            payload.slotAreaType = SlotAreaType.Equipment;

            return payload;
        }

        public void OnInventoryToggle()
        {
            if (Inventory.IsOpened)
            {
                controller.canAttack = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                GetComponent<PlayerAim>().canAim = false;
            }
            else
            {
                controller.canAttack = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                GetComponent<PlayerAim>().canAim = true;
            }
        }

        private UIPayload GenerateStoneAcquirePayloadTest(int index)
        {
            //for test
            var payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.AddSlotItem;
            payload.slotAreaType = SlotAreaType.Item;
            payload.groupType = GroupType.Stone;
            payload.itemData = DataManager.Instance.GetIndexData<StoneData, StoneDataParsingInfo>(index);
            return payload;
        }

        public void ConsumeItemEvent()
        {
            var data = DataManager.Instance.GetIndexData<ItemData, ItemDataParsingInfo>(itemIdx);
            playerStatus.ApplyConsumableItemEffect(GenerateConsumableItemData(data));
            ticketMachine.SendMessage(ChannelType.UI, GenerateConsumeItemPayload());
        }

        private ConsumableItemData GenerateConsumableItemData(ItemData data)
        {
            var consumableItemData = new ConsumableItemData();
            consumableItemData.HPRecoveryAmount = data.increasePoint;
            return consumableItemData;
        }

        private UIPayload GenerateConsumeItemPayload()
        {
            var payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.ConsumeSlotItem;
            payload.slotAreaType = SlotAreaType.Item;
            payload.groupType = GroupType.Item;
            payload.itemData = DataManager.Instance.GetIndexData<ItemData, ItemDataParsingInfo>(itemIdx);
            return payload;
        }

        public struct ConsumableItemData
        {
            public int HPRecoveryAmount;
            // !TODO : 추가될 소모품의 효과들에 대한 정의가 필요
        }
    }
}