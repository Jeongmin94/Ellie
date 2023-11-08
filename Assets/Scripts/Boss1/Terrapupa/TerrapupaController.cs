using Assets.Scripts.Utils;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

namespace Boss.Terrapupa
{
    public class TerrapupaController : BehaviourTreeController
    {
        [SerializeField] private Transform stone;
        [SerializeField] private TerrapupaWeakPoint weakPoint;
        [HideInInspector] public TerrapupaRootData terrapupaData;
        
        private TicketMachine ticketMachine;

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

        private void Awake()
        {
            InitTicketMachine();
            InitStatus();
            SubscribeEvent();
            StartCoroutine(FallCheck());
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

        private IEnumerator FallCheck()
        {
            LayerMask groundMask = LayerMask.GetMask("Ground");
            float checkDistance = 30.0f;
            float fallCheckLatency = 5.0f;
            float rayStartOffset = 10.0f;

            while (true)
            {
                RaycastHit hit;

                Vector3 rayStart = transform.position + Vector3.up * rayStartOffset;

                bool hitGround = Physics.Raycast(rayStart, -Vector3.up, out hit, checkDistance, groundMask);

                if (!hitGround)
                {
                    Debug.Log("추락 방지, 포지션 초기화");
                    transform.position = Vector3.zero;
                }
                else
                {
                    Debug.Log(hit.distance);
                }

                yield return new WaitForSeconds(fallCheckLatency);
            }
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