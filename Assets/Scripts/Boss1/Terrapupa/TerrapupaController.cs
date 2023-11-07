using Assets.Scripts.Utils;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

namespace Boss.Terrapupa
{
    public enum TerrapupaType
    {
        None,
        Terra,
        Pupa,
    }

    public class TerrapupaController : BehaviourTreeController
    {
        [SerializeField] private Transform stone;
        [SerializeField] private TerrapupaWeakPoint weakPoint;
        [HideInInspector] public TerrapupaRootData terrapupaData;
        
        private TicketMachine ticketMachine;
        private TerrapupaType bossType;

        public Transform Stone
        {
            get { return stone; }
            set { stone = value; }
        }

        public TerrapupaWeakPoint WeakPoint
        {
            get { return weakPoint; }
            set { weakPoint = value; }
        }

        public TerrapupaType BossType
        {
            get { return bossType; }
            set
            {
                if(bossType == TerrapupaType.None)
                {
                    bossType = value;
                }
            }
        }


        private void Awake()
        {
            InitTicketMachine();
            InitStatus();
            SubscribeEvent();
        }

        private void InitTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.Combat, ChannelType.Terrapupa, ChannelType.BossInteraction);
        }

        private void InitStatus()
        {
            terrapupaData = rootTreeData as TerrapupaRootData;
        }

        private void SubscribeEvent()
        {
            weakPoint.SubscribeCollisionAction(OnCollidedCoreByPlayerStone);
        }

        private void OnCollidedCoreByPlayerStone(IBaseEventPayload payload)
        {
            // 플레이어 총알 -> Combat Channel -> TerrapupaWeakPoint :: ReceiveDamage() -> TerrapupaController
            Debug.Log($"OnCollidedCoreByPlayerStone :: {payload}");

            if(terrapupaData.isStuned.value)
            {
                CombatPayload combatPayload = payload as CombatPayload;
                int damage = combatPayload.Damage;

                terrapupaData.currentHP.Value -= damage;
                Debug.Log($"기절 상태, 데미지 입음 {terrapupaData.currentHP.Value}");
                
            }
            else 
            {
                Debug.Log("기절 상태가 아님");
            }
        }
    }
}