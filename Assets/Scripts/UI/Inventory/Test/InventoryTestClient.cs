using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Item.Goods;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Equipment;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using Data.UI.Inventory;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.UI.Inventory.Test
{
    public class InventoryTestClient : MonoBehaviour
    {
        [SerializeField] private ItemDataParsingInfo itemDataParsingInfo;
        [SerializeField] private GameGoods gameGoods;

        private FrameCanvas consumptionCanvas;
        private FrameCanvas stoneCanvas;

        private TicketMachine ticketMachine;

        private UIPayload testPayload;

        private void Awake()
        {
            InitConsumptionCanvas();
            InitStoneCanvas();
            InitTicketMachine();
            gameGoods.Init();

            UIManager.Instance.MakePopup<Inventory>(UIManager.Inventory);
        }

        private void InitConsumptionCanvas()
        {
            consumptionCanvas = UIManager.Instance.MakeStatic<FrameCanvas>(FrameCanvas.Path);
            consumptionCanvas.FrameWidth = 86.0f;
            consumptionCanvas.FrameHeight = 86.0f;
            consumptionCanvas.FramePanelRect = EquipmentConst.ConsumptionPanelRect;
            consumptionCanvas.FrameImage = ResourceManager.Instance.LoadSprite("UI/Item/Equipment/ConsumptionFrameRotated");

            consumptionCanvas.InitFrameCanvas();

            Vector2[] directions =
            {
                new Vector2(0.0f, consumptionCanvas.FrameHeight / 2.0f),
                new Vector2(-consumptionCanvas.FrameWidth / 2.0f, 0.0f),
                new Vector2(0.0f, -consumptionCanvas.FrameHeight / 2.0f),
                new Vector2(consumptionCanvas.FrameWidth / 2.0f, 0.0f),
            };

            consumptionCanvas.InitFrame(directions);
        }

        private void InitStoneCanvas()
        {
            stoneCanvas = UIManager.Instance.MakeStatic<FrameCanvas>(FrameCanvas.Path);
            stoneCanvas.FrameWidth = 113.0f;
            stoneCanvas.FrameHeight = 113.0f;
            stoneCanvas.FramePanelRect = EquipmentConst.StonePanelRect;
            stoneCanvas.FrameImage = ResourceManager.Instance.LoadSprite("UI/Item/Equipment/StoneFrame");

            stoneCanvas.InitFrameCanvas();

            Vector2[] directions =
            {
                new Vector2(0.0f, stoneCanvas.FrameHeight / 2.0f),
                new Vector2(-stoneCanvas.FrameWidth / 2.0f, -stoneCanvas.FrameHeight / 2.0f),
                new Vector2(stoneCanvas.FrameWidth / 2.0f, -stoneCanvas.FrameWidth / 2.0f),
            };

            stoneCanvas.InitFrame(directions);
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
            Debug.Log($"{itemDataParsingInfo} 파싱 완료");
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
                ticketMachine.SendMessage(ChannelType.UI, testPayload);
                ticketMachine.SendMessage(ChannelType.UI, MakeAddItemPayload2());
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                var payload = MakeAddItemPayload2();
                var testItemInfo = itemDataParsingInfo.items[Random.Range(1, itemDataParsingInfo.items.Count)];
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

            var testItemInfo = itemDataParsingInfo.items[0];
            testItemInfo.imageName = "UI/Item/ItemDefaultRed";

            payload.itemData = testItemInfo;

            return payload;
        }

        private UIPayload MakeAddItemPayload2()
        {
            var ret = MakeAddItemPayload();

            ret.itemData = itemDataParsingInfo.items[1];
            ret.itemData.imageName = "UI/Item/ItemDefaultWhite";

            return ret;
        }

        private UIPayload MakeConsumeItemPayload()
        {
            var payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.ConsumeSlotItem;

            var testItemInfo = itemDataParsingInfo.items[0];
            testItemInfo.imageName = "UI/Item/ItemDefaultRed";

            payload.itemData = testItemInfo;

            return payload;
        }
    }
}