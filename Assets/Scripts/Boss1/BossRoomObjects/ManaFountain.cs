using Channels.Boss;
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

        private void OnTriggerEnter(Collider other)
        {
            if(!isBroken)
            {
                if (other.transform.CompareTag("Stone") && !isCooldown)
                {
                    Debug.Log($"{other.name} 충돌");

                    isCooldown = true;

                    EventBus.Instance.Publish<BossEventPayload>(EventBusEvents.HitManaByPlayerStone,
                        new BossEventPayload 
                        { 
                            TransformValue1 = transform, 
                            TransformValue2 = spawnPosition 
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
    }
}
