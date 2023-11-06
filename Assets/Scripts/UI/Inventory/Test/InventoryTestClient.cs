using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using Data.UI.Inventory;
using UnityEngine;

namespace Assets.Scripts.UI.Inventory.Test
{
    public class InventoryTestClient : MonoBehaviour
    {
        [SerializeField] private InventoryChannel inventoryChannel;
        [SerializeField] private ItemDataParsingInfo itemDataParsingInfo;

        private TicketMachine ticketMachine;

        private void Awake()
        {
            InitTicketMachine();

            StartCoroutine(CheckParse());
        }

        private void InitTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.UI);

            TicketManager.Instance.Ticket(ticketMachine);
        }

        private IEnumerator CheckParse()
        {
            yield return DataManager.Instance.CheckIsParseDone();
            Debug.Log($"{itemDataParsingInfo} 파싱 완료");
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
                ticketMachine.SendMessage(ChannelType.UI, MakeAddItemPayload());
                ticketMachine.SendMessage(ChannelType.UI, MakeAddItemPayload2());
            }

            // 아이템 소모
            if (Input.GetKeyDown(KeyCode.S))
            {
                ticketMachine.SendMessage(ChannelType.UI, MakeConsumeItemPayload());
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

            var testItemInfo = itemDataParsingInfo.items[0];
            testItemInfo.imageName = "UI/Item/ItemDefaultRed";

            payload.itemData = testItemInfo;

            return payload;
        }

        private UIPayload MakeAddItemPayload2()
        {
            var ret = MakeAddItemPayload();

            ret.itemData = itemDataParsingInfo.items[2];
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