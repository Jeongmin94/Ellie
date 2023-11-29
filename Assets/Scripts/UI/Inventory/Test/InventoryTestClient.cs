using System.Collections;
using Assets.Scripts.Item.Goods;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.UI.Inventory.Test
{
    public class InventoryTestClient : MonoBehaviour
    {
        [SerializeField] private ItemDataParsingInfo consumableItemDataParsingInfo;
        [SerializeField] private GameGoods gameGoods;

        private TicketMachine ticketMachine;

        private UIPayload testPayload;

        private void Awake()
        {
            InitTicketMachine();
            gameGoods.Init();

            UIManager.Instance.MakePopup<Inventory>(UIManager.Inventory);
        }

        private void InitTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.UI);
            TicketManager.Instance.Ticket(ticketMachine);
        }

        private void Start()
        {
            StartCoroutine(CheckParse());
        }

        private IEnumerator CheckParse()
        {
            yield return DataManager.Instance.CheckIsParseDone();
            Debug.Log($"{consumableItemDataParsingInfo} 파싱 완료");
            testPayload = MakeAddItemPayload();
        }

        private void Update()
        {
            // 인벤토리 On/Off
            if (Input.GetKeyDown(KeyCode.I))
            {
                ticketMachine.SendMessage(ChannelType.UI, MakeInventoryOpenPayload());
            }

            // 아이템 생성
            if (Input.GetKeyDown(KeyCode.A))
            {
                //ticketMachine.SendMessage(ChannelType.UI, testPayload);
                ticketMachine.SendMessage(ChannelType.UI, MakeAddItemPayload2());
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                var payload = MakeAddItemPayload2();
                var testItemInfo = consumableItemDataParsingInfo.items[Random.Range(1, consumableItemDataParsingInfo.items.Count)];
                testItemInfo.imageName = "UI/Item/ItemDefaultWhite";
                payload.itemData = testItemInfo;

                ticketMachine.SendMessage(ChannelType.UI, payload);
            }

            // 아이템 소모
            if (Input.GetKeyDown(KeyCode.S))
            {
                ticketMachine.SendMessage(ChannelType.UI, MakeConsumeItemPayload());
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log($"{testPayload.itemData.name}, {testPayload.itemData.description}");
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                gameGoods.gold.Value--;
                gameGoods.stonePiece.Value--;
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                gameGoods.gold.Value++;
                gameGoods.stonePiece.Value++;
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                ticketMachine.SendMessage(ChannelType.UI, MakeCCWPayload());
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                ticketMachine.SendMessage(ChannelType.UI, MakeCWPayload());
            }
        }

        private UIPayload MakeInventoryOpenPayload()
        {
            var payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.ToggleInventory;

            return payload;
        }

        private UIPayload MakeAddItemPayload()
        {
            var payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.AddSlotItem;
            payload.slotAreaType = SlotAreaType.Item;

            var testItemInfo = consumableItemDataParsingInfo.items[0];

            payload.itemData = testItemInfo;

            return payload;
        }

        private UIPayload MakeAddItemPayload2()
        {
            var ret = MakeAddItemPayload();

            ret.itemData = consumableItemDataParsingInfo.items[1];

            return ret;
        }

        private UIPayload MakeConsumeItemPayload()
        {
            var payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.ConsumeSlotItem;
            payload.slotAreaType = SlotAreaType.Item;

            var testItemInfo = consumableItemDataParsingInfo.items[0];

            payload.itemData = testItemInfo;

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
    }
}