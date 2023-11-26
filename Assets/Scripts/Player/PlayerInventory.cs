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
        private const int CONSUMABLEEQUIPMENTSLOTCOUNT = 4;
        private PlayerController controller;
        private TicketMachine ticketMachine;
        private Inventory inventory;
        public Inventory Inventory { get { return inventory; } }
        public ItemMetaData[] consumableEquipmentSlot = new ItemMetaData[CONSUMABLEEQUIPMENTSLOTCOUNT];
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

            if(Input.GetKeyDown(KeyCode.Escape) && Inventory.IsOpened)
            {
                ticketMachine.SendMessage(ChannelType.UI, MakeInventoryOpenPayload());
                OnInventoryToggle();
            }

            //for test
            if(Input.GetKeyDown(KeyCode.Q))
            {
                ticketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayloadTest());
            }

            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                //1번 슬롯에 아이템이 있을 경우에만 sendmessage
                ticketMachine.SendMessage(ChannelType.UI, GenerateConsumeItemPayload(0));

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
            if(Inventory.IsOpened)
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

        private UIPayload GenerateStoneAcquirePayloadTest()
        {
            //for test
            UIPayload payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.AddSlotItem;
            payload.slotAreaType = SlotAreaType.Item;
            payload.groupType = GroupType.Stone;
            payload.itemData = DataManager.Instance.GetIndexData<StoneData, StoneDataParsingInfo>(4000);
            return payload;
        }

        private UIPayload GenerateConsumeItemPayload(int equipmentSlotIdx)
        {
            UIPayload payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.ConsumeSlotItem;
            payload.slotAreaType = SlotAreaType.Item;
            payload.groupType = GroupType.Item;
            payload.itemData = consumableEquipmentSlot[equipmentSlotIdx];
            return payload;
        }
    }
}