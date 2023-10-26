using Assets.Scripts.Boss.Terrapupa;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Boss.Objects
{ 
    public class ManaFountain : MonoBehaviour
    {
        public TerrapupaAttackType banBossAttackType;

        public float cooldownValue = 3.0f;
        public float respawnValue = 3.0f;

        [SerializeField] private bool isCooldown;
        [SerializeField] private bool isBroken;

        public bool IsCooldown
        {
            get { return isCooldown; }
            private set { isCooldown = value; }
        }

        public bool IsBroken
        {
            get { return isBroken; }
            private set { isBroken = value; }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!isBroken)
            {
                if (other.transform.CompareTag("Stone") && !isCooldown)
                {
                    Debug.Log("돌과 충돌");

                    isCooldown = true;
                    StartCoroutine(StartCooldown());
                    EventBus.Instance.Publish<BossEventPayload>(EventBusEvents.DestroyedManaByBoss1,
                        new BossEventPayload { TransformValue1 = transform });
                }
                // 임시로 플레이어
                else if (other.transform.CompareTag("Boss"))
                {
                    Debug.Log("보스와 충돌");

                    isBroken = true;
                    StartCoroutine(StartRespawn());
                    EventBus.Instance.Publish<BossEventPayload>(EventBusEvents.DestroyedManaByBoss1,
                        new BossEventPayload { TransformValue1 = transform, AttackTypeValue = banBossAttackType });
                }
            }
        }

        private IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(cooldownValue);

            isCooldown = false;
            Debug.Log($"{name} 쿨타임 완료");
        }

        private IEnumerator StartRespawn()
        {
            yield return new WaitForSeconds(respawnValue);

            isBroken = false;
            Debug.Log($"{name} 리스폰 완료");

            EventBus.Instance.Publish<BossEventPayload>(EventBusEvents.RespawnMana,
                new BossEventPayload { AttackTypeValue = banBossAttackType });
        }
    }
}
