using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.Item;
using Assets.Scripts.Item.Goods;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Inventory;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        public struct ConsumableItemData
        {
            public int HPRecoveryAmount;
            // !TODO : 추가될 소모품의 효과들에 대한 정의가 필요
        }
        private const int CONSUMABLEEQUIPMENTSLOTCOUNT = 4;
        private PlayerController controller;
        private PlayerStatus playerStatus;
        private TicketMachine ticketMachine;
        private Inventory inventory;
        public Inventory Inventory { get { return inventory; } }
        public ItemMetaData[] consumableEquipmentSlot = new ItemMetaData[CONSUMABLEEQUIPMENTSLOTCOUNT];
        public int curSlotIdx;
        [SerializeField] private GameGoods gameGoods;

        public bool isOpen;

        private void Awake()
        {
            inventory = UIManager.Instance.MakePopup<Inventory>(UIManager.Inventory);
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
            if (Input.GetKeyDown(KeyCode.I))
            {
                ticketMachine.SendMessage(ChannelType.UI, MakeInventoryOpenPayload());
                OnInventoryToggle();
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                ticketMachine.SendMessage(ChannelType.UI, MakeCCWPayload());
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                ticketMachine.SendMessage(ChannelType.UI, MakeCWPayload());
            }

            if (Input.GetKeyDown(KeyCode.Escape) && Inventory.IsOpened)
            {
                ticketMachine.SendMessage(ChannelType.UI, MakeInventoryOpenPayload());
                OnInventoryToggle();
            }

            //for test
            if (Input.GetKeyDown(KeyCode.Q))
            {
                for (int i = 0; i < 20; i++)
                {
                    ticketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayloadTest(4017));
                    ticketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayloadTest(4000));
                }
                for (int i = 0; i < 5; i++)
                {
                    ticketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayloadTest(4020));
                    ticketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayloadTest(4021));
                }
                for (int i = 0; i < 5; i++)
                {
                    ticketMachine.SendMessage(ChannelType.UI, new UIPayload
                    {
                        uiType = UIType.Notify,
                        groupType = UI.Inventory.GroupType.Item,
                        slotAreaType = UI.Inventory.SlotAreaType.Item,
                        actionType = ActionType.AddSlotItem,
                        itemData = DataManager.Instance.GetIndexData<ItemData, ItemDataParsingInfo>(4100),
                    });

                    ticketMachine.SendMessage(ChannelType.UI, new UIPayload
                    {
                        uiType = UIType.Notify,
                        groupType = UI.Inventory.GroupType.Item,
                        slotAreaType = UI.Inventory.SlotAreaType.Item,
                        actionType = ActionType.AddSlotItem,
                        itemData = DataManager.Instance.GetIndexData<ItemData, ItemDataParsingInfo>(4101),
                    });
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //1번 슬롯에 아이템이 있을 경우에만 sendmessage
                if (consumableEquipmentSlot[0] == null) return;
                if (controller.GetCurState() == PlayerStateName.Idle ||
                    controller.GetCurState() == PlayerStateName.Walk ||
                    controller.GetCurState() == PlayerStateName.Sprint)
                {
                    curSlotIdx = 0;
                    controller.ChangeState(PlayerStateName.ConsumingItem);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //1번 슬롯에 아이템이 있을 경우에만 sendmessage
                if (consumableEquipmentSlot[1] == null) return;
                if (controller.GetCurState() == PlayerStateName.Idle ||
                    controller.GetCurState() == PlayerStateName.Walk ||
                    controller.GetCurState() == PlayerStateName.Sprint)
                {
                    curSlotIdx = 1;
                    controller.ChangeState(PlayerStateName.ConsumingItem);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                //1번 슬롯에 아이템이 있을 경우에만 sendmessage
                if (consumableEquipmentSlot[2] == null) return;
                if (controller.GetCurState() == PlayerStateName.Idle ||
                    controller.GetCurState() == PlayerStateName.Walk ||
                    controller.GetCurState() == PlayerStateName.Sprint)
                {
                    curSlotIdx = 2;
                    controller.ChangeState(PlayerStateName.ConsumingItem);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                //1번 슬롯에 아이템이 있을 경우에만 sendmessage
                if (consumableEquipmentSlot[3] == null) return;
                if (controller.GetCurState() == PlayerStateName.Idle ||
                    controller.GetCurState() == PlayerStateName.Walk ||
                    controller.GetCurState() == PlayerStateName.Sprint)
                {
                    curSlotIdx = 3;
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

        private UIPayload MakeCCWPayload()
        {
            var payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.MoveCounterClockwise;
            payload.slotAreaType = SlotAreaType.Equipment;

            return payload;
        }

        private UIPayload MakeCWPayload()
        {
            var payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.MoveClockwise;
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
            UIPayload payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.AddSlotItem;
            payload.slotAreaType = SlotAreaType.Item;
            payload.groupType = GroupType.Stone;
            payload.itemData = DataManager.Instance.GetIndexData<StoneData, StoneDataParsingInfo>(index);
            return payload;
        }

        public void ConsumeItemEvent()
        {
            ticketMachine.SendMessage(ChannelType.UI, GenerateConsumeItemPayload());
            ItemData data = DataManager.Instance.GetIndexData<ItemData, ItemDataParsingInfo>(consumableEquipmentSlot[curSlotIdx].index);
            playerStatus.ApplyConsumableItemEffect(GenerateConsumableItemData(data));
        }

        private ConsumableItemData GenerateConsumableItemData(ItemData data)
        {
            ConsumableItemData consumableItemData = new ConsumableItemData();
            consumableItemData.HPRecoveryAmount = data.increasePoint;
            return consumableItemData;
        }
        private UIPayload GenerateConsumeItemPayload()
        {
            UIPayload payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.ConsumeSlotItem;
            payload.slotAreaType = SlotAreaType.Item;
            payload.groupType = GroupType.Item;
            payload.itemData = consumableEquipmentSlot[curSlotIdx];
            return payload;
        }
    }
}