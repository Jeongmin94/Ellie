using Assets.Scripts.Channels.Item;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Item.Stone
{
    public class StoneHatchery : MonoBehaviour
    {
        private TicketMachine ticketMachine;
        private Pool stonePool;

        [SerializeField] private GameObject stone;
        private const int initialPoolSize = 10;
        private void Awake()
        {
            SetTicketMachine();
            InitStonePool();
        }
        private void SetTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.Combat, ChannelType.Item);
            //ticketMachine.GetTicket(ChannelType.Combat).SubscribeNotifyAction(ReleaseStoneEvent);
            ticketMachine.RegisterObserver(ChannelType.Item, ReleaseStoneEvent);
        }

        private void InitStonePool()
        {
            //돌맹이 일정량만큼 풀에서 받아서 걔네 티켓 만들어주고 해처리의 공격함수 구독
            stonePool = PoolManager.Instance.CreatePool(stone, initialPoolSize);
        }

        private void Attack(CombatPayload payload)
        {
            ticketMachine.SendMessage(ChannelType.Combat, payload);
        }

        public Poolable GetStone()
        {
            Poolable obj = stonePool.Pop();

            obj.GetComponent<BaseStone>().Subscribe(Attack);
            return obj;
        }

        public void CollectStone(BaseStone stone)
        {
            stone.UnSubscribe(Attack);
            stonePool.Push(stone);
        }

        public void ReleaseStoneEvent(IBaseEventPayload payload)
        {
            Debug.Log("hatchery : make stone");
            ItemPayload itemPayload = payload as ItemPayload;
            BaseStone stone = GetStone() as BaseStone;
            Vector3 startPos = itemPayload.StoneSpawnPos;
            Vector3 direction = itemPayload.StoneDirection;
            float strength = itemPayload.StoneStrength;
            ReleaseStone(stone, startPos, direction, strength);
        }

        private void ReleaseStone(BaseStone stone, Vector3 startPos, Vector3 direction, float strength)
        {
            stone.SetPosition(startPos);
            stone.MoveStone(direction, strength);
        }
    }
}