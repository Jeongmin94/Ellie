using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Channels.Item;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Type;
using UnityEngine;

    public class ItemHatchery : MonoBehaviour
    {
        private TicketMachine ticketMachine;

        private const int poolSize = 10;
        private Pool itemPool;

        [SerializeField] private GameObject item;

        private void Awake()
        {
            SetTicketMachine();
            InitPool();
        }

        private void SetTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTicket(ChannelType.Item);
            ticketMachine.RegisterObserver(ChannelType.Item, DropItemEvent);
        }

        private void InitPool()
        {
            itemPool = PoolManager.Instance.CreatePool(item, poolSize);
        }

        public void DropItemEvent(IBaseEventPayload payload)
        {
            ItemEventPayload itemPayload = payload as ItemEventPayload;
            TestItemDrop item =  DropItem(itemPayload.itemIndex) as TestItemDrop;
            item.transform.position = itemPayload.itemDropPosition;
        }

        public Poolable DropItem(int itemIndex)
        {
            Poolable obj = itemPool.Pop();
            obj.GetComponent<TestItemDrop>().data = DataManager.Instance.GetIndexData<ConsumableItemData, ConsumableItemDataParsingInfo>(itemIndex);

            return obj;
        }

        public void PickItem(TestItemDrop item)
        {
            itemPool.Push(item);
        }
    }