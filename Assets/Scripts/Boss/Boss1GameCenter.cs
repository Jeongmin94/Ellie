using Assets.Scripts.Boss.Objects;
using Assets.Scripts.Boss.Terrapupa;
using Assets.Scripts.Player;

using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

namespace Assets.Scripts.Boss
{
    public class Boss1GameCenter : MonoBehaviour
    {
        [SerializeField] private TerrapupaController boss;
        [SerializeField] private PlayerController player;
        [SerializeField] private List<ManaFountain> manaObjects = new List<ManaFountain>();

        private void Start()
        {
            EventBus.Instance.Subscribe(EventBusEvents.GripStoneByBoss1, OnSpawnStone);
            EventBus.Instance.Subscribe<BaseEventPayload>(EventBusEvents.ThrowStoneByBoss1, OnThrowStone);

            EventBus.Instance.Subscribe<BossEventPayload>(EventBusEvents.HitManaByPlayerStone, OnHitMana);
            EventBus.Instance.Subscribe<BossEventPayload>(EventBusEvents.DestroyedManaByBoss1, OnDestroyedMana);
        }

        private void OnSpawnStone()
        {
            Debug.Log("OnSpawnStone");

            boss.RightHand.gameObject.SetActive(true);
        }

        private void OnThrowStone(BaseEventPayload payload)
        {
            Debug.Log("OnThrowStone");

            BossEventPayload posPayload = payload as BossEventPayload;

            GameObject bossStone = Instantiate(boss.RightHand.gameObject, boss.RightHand.position, Quaternion.identity);
            bossStone.GetComponent<TerrapupaStone>().MoveToTarget(posPayload.TransformValue2);

            boss.RightHand.gameObject.SetActive(false);
        }

        private void OnHitMana(BossEventPayload manaPayload)
        {
            // 마나의 샘 마법 돌맹이 루팅 이벤트
            Debug.Log("OnHitMana");
        }

        private void OnDestroyedMana(BossEventPayload manaPayload)
        {
            // 마나의 샘 부서졌을 때, 보스 공격 타입 봉인
            Debug.Log("OnDestroyedMana");
            Debug.Log($"{manaPayload.AttackTypeValue} 공격 타입 봉인");

            switch (manaPayload.AttackTypeValue)
            {
                case TerrapupaAttackType.ThrowStone:
                    boss.canThrowStone.value = false;
                    break;
                case TerrapupaAttackType.EarthQuake:
                    boss.canEarthQuake.value = false;
                    break;
                case TerrapupaAttackType.Roll:
                    boss.canRoll.value = false;
                    break;
                case TerrapupaAttackType.LowAttack:
                    boss.canLowAttack.value = false;
                    break;
                default:
                    break;
            }
        }
    }
}