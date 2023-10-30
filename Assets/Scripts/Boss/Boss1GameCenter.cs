using Assets.Scripts.Boss.Objects;
using Assets.Scripts.Boss.Terrapupa;
using Assets.Scripts.Player;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

namespace Assets.Scripts.Boss
{
    public class Boss1GameCenter : MonoBehaviour
    {
        public GameObject playerStoneTemp;

        [SerializeField] private TerrapupaController boss;
        [SerializeField] private PlayerController player;
        [SerializeField] private List<ManaFountain> manaObjects = new List<ManaFountain>();

        private void Start()
        {
            EventBus.Instance.Subscribe(EventBusEvents.GripStoneByBoss1, OnSpawnStone);
            EventBus.Instance.Subscribe<IBaseEventPayload>(EventBusEvents.ThrowStoneByBoss1, OnThrowStone);
            EventBus.Instance.Subscribe<IBaseEventPayload>(EventBusEvents.OccurEarthQuake, OnStartEarthQuake);

            EventBus.Instance.Subscribe<BossEventPayload>(EventBusEvents.HitManaByPlayerStone, OnHitMana);
            EventBus.Instance.Subscribe<BossEventPayload>(EventBusEvents.DestroyedManaByBoss1, OnDestroyedMana);
        }

        private void OnSpawnStone()
        {
            Debug.Log("OnSpawnStone :: 보스의 돌맹이 줍기");

            boss.RightHand.gameObject.SetActive(true);
        }

        private void OnThrowStone(IBaseEventPayload payload)
        {
            Debug.Log("OnThrowStone :: 보스의 돌맹이 던지기");

            BossEventPayload posPayload = payload as BossEventPayload;

            GameObject bossStone = Instantiate(
                boss.RightHand.gameObject, boss.RightHand.position, Quaternion.identity);
            bossStone.GetComponent<Rigidbody>().isKinematic = false;
            bossStone.GetComponent<TerrapupaStone>().MoveToTarget(posPayload.TransformValue2);

            boss.RightHand.gameObject.SetActive(false);
        }

        private void OnHitMana(BossEventPayload manaPayload)
        {
            Debug.Log("OnHitMana :: 돌맹이 루팅");

            for (int i = 0; i < 3; i++)
            {
                GameObject playerStone = Instantiate(
                    playerStoneTemp, manaPayload.TransformValue1.position, Quaternion.identity);

                Vector3 randomDirection = new Vector3(
                Random.Range(-1f, 1f), Random.Range(0.5f, 1f), Random.Range(-1f, 1f)).normalized;

                playerStone.GetComponent<Rigidbody>().AddForce(
                    randomDirection * 10.0f, ForceMode.Impulse);
            }

            StartCoroutine(ManaCooldown(manaPayload));
        }

        private IEnumerator ManaCooldown(BossEventPayload manaPayload)
        {
            ManaFountain mana = manaPayload.TransformValue1.GetComponent<ManaFountain>();
            mana.IsCooldown = true;

            yield return new WaitForSeconds(mana.coolDownValue);

            Debug.Log($"{mana.name} 쿨타임 완료");
            mana.IsCooldown = false;
        }

        private void OnDestroyedMana(BossEventPayload manaPayload)
        {
            Debug.Log($"OnDestroyedMana :: {manaPayload.AttackTypeValue} 공격 타입 봉인");

            if(manaPayload.TransformValue2 != null)
            {
                Destroy(manaPayload.TransformValue2.gameObject);
            }

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

            StartCoroutine(ManaRespawn(manaPayload));
        }

        private IEnumerator ManaRespawn(BossEventPayload manaPayload)
        {
            ManaFountain mana = manaPayload.TransformValue1.GetComponent<ManaFountain>();
            mana.IsBroken = true;
            mana.gameObject.SetActive(false);

            yield return new WaitForSeconds(mana.respawnValue);

            Debug.Log($"{mana.name} 리스폰 완료");
            mana.gameObject.SetActive(true);
            mana.IsBroken = false;
            mana.IsCooldown = false;

            switch (manaPayload.AttackTypeValue)
            {
                case TerrapupaAttackType.ThrowStone:
                    boss.canThrowStone.value = true;
                    break;
                case TerrapupaAttackType.EarthQuake:
                    boss.canEarthQuake.value = true;
                    break;
                case TerrapupaAttackType.Roll:
                    boss.canRoll.value = true;
                    break;
                case TerrapupaAttackType.LowAttack:
                    boss.canLowAttack.value = true;
                    break;
                default:
                    break;
            }
        }

        private void OnStartEarthQuake(IBaseEventPayload earthQuakePayload)
        {
            Debug.Log($"OnStartEarthQuake");
            BossEventPayload payload = earthQuakePayload as BossEventPayload;

            Transform player = payload.TransformValue1;
            int attack = payload.IntValue;

            float jumpCheckValue = 1.0f;
            
            // 점프 체크
            if(player != null)
            {

            }

            
        }

        private void OnGUI()
        {
            float distance = Vector3.Distance(player.transform.position, boss.transform.position);
            string distanceText = "Distance: " + distance.ToString("F2");

            int boxWidth = 200;
            int boxHeight = 30;
            int offsetX = 10;
            int offsetY = 10;

            Rect boxRect = new Rect(Screen.width - boxWidth - offsetX, offsetY, boxWidth, boxHeight);
            GUI.Box(boxRect, "");
            GUI.Label(new Rect(boxRect.x + 20, boxRect.y + 5, boxWidth, boxHeight), distanceText);
        }

        private void OnDrawGizmos()
        {
            float rayLength = 7.0f;
            Color rayColor = Color.red;

            Gizmos.color = rayColor;
            Gizmos.DrawRay(boss.transform.position, boss.transform.forward * rayLength);
        }
    }
}