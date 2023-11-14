using Assets.Scripts.Channels.Item;
using Assets.Scripts.Utils;
using Channels.Boss;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using UnityEngine;

namespace Boss.Objects
{
    public class ManaFountain : MonoBehaviour
    {
        public TerrapupaAttackType banBossAttackType;
        public float coolDownValue = 3.0f;
        public float respawnValue = 3.0f;
        public Transform spawnPosition;

        private bool isCooldown;
        private bool isBroken;

        private const int MAGICSTONE_INDEX = 4020;
        private TicketMachine ticketMachine;

        public bool IsCooldown
        {
            get { return isCooldown; }
            set { isCooldown = value; }
        }

        public bool IsBroken
        {
            get { return isBroken; }
            set { isBroken = value; }
        }

        public void InitTicketMachine(TicketMachine ticketMachine)
        {
            this.ticketMachine = ticketMachine;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isBroken)
            {
                if (other.transform.CompareTag("Stone") && !isCooldown)
                {
                    Debug.Log($"{other.name} 충돌");

                    isCooldown = true;

                    EventBus.Instance.Publish<BossEventPayload>(EventBusEvents.HitManaByPlayerStone,
                        new BossEventPayload
                        {
                            TransformValue1 = transform,
                        });


                    Debug.Log("Mine Stone : " + MAGICSTONE_INDEX.ToString());
                    ticketMachine.SendMessage(ChannelType.Stone,
                        new StoneEventPayload
                        {
                            Type = StoneEventType.MineStone,
                            StoneSpawnPos = spawnPosition.position,
                            StoneForce = GetRandVector(),
                            StoneIdx = MAGICSTONE_INDEX,
                        });

                }
                else if (other.transform.CompareTag("Boss"))
                {
                    Debug.Log($"{other.name} 충돌");

                    isBroken = true;

                    EventBus.Instance.Publish<BossEventPayload>(EventBusEvents.DestroyedManaByBoss1,
                        new BossEventPayload
                        {
                            TransformValue1 = transform,
                            AttackTypeValue = banBossAttackType,
                            Sender = other.transform.root,
                        });
                }
                else if (other.transform.CompareTag("BattleObject"))
                {
                    // 임시 바위
                    Debug.Log($"{other.name} 충돌");

                    isBroken = true;

                    EventBus.Instance.Publish<BossEventPayload>(EventBusEvents.DestroyedManaByBoss1,
                        new BossEventPayload
                        {
                            TransformValue1 = transform,
                            TransformValue2 = other.transform,
                            AttackTypeValue = banBossAttackType,
                            Sender = other.transform,
                        });
                }
            }
        }
        private Vector3 GetRandVector()
        {
            Vector3 vec = new(Random.Range(-1.0f, 1.0f), 0.5f, 0);
            return vec.normalized;
        }
    }
}
