using Assets.Scripts.Boss.Objects;
using Assets.Scripts.Boss.Terrapupa;
using Assets.Scripts.Player;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Boss
{
    public class Boss1GameCenter : MonoBehaviour
    {
        public GameObject playerStoneTemp;
        public GameObject magicStalactiteTemp;
        public GameObject magicStoneTemp;
        private MagicStone magicStone;

        public int numberOfSector = 3;
        public int stalactitePerSector = 3;
        public float fieldRadius = 25.0f;
        public float fieldHeight = 8.0f;

        [SerializeField] private TerrapupaController boss;
        [SerializeField] private PlayerController player;
        [SerializeField] private List<ManaFountain> manaObjects = new List<ManaFountain>();
        [SerializeField] private List<List<MagicStalactite>> stalactites = new List<List<MagicStalactite>>();

        private void Start()
        {
            SubscribeEvents();
            SpawnStalactites();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                GameObject temp = Instantiate(magicStoneTemp, Vector3.zero, Quaternion.identity);
                magicStone = temp.GetComponent<MagicStone>();
            }
        }

        private void SubscribeEvents()
        {
            EventBus.Instance.Subscribe(EventBusEvents.GripStoneByBoss1, OnSpawnStone);
            EventBus.Instance.Subscribe<IBaseEventPayload>(EventBusEvents.ThrowStoneByBoss1, OnThrowStone);
            EventBus.Instance.Subscribe<IBaseEventPayload>(EventBusEvents.OccurEarthQuake, OnStartEarthQuake);
            EventBus.Instance.Subscribe<BossEventPayload>(EventBusEvents.HitManaByPlayerStone, OnHitMana);
            EventBus.Instance.Subscribe<BossEventPayload>(EventBusEvents.DestroyedManaByBoss1, OnDestroyedMana);
            EventBus.Instance.Subscribe<BossEventPayload>(EventBusEvents.DropMagicStalactite, OnDropMagicStalactite);
        }

        private void SpawnStalactites()
        {
            for (int i = 0; i < numberOfSector; i++)
            {
                List<MagicStalactite> sectorList = new List<MagicStalactite>();
                for (int j = 0; j < stalactitePerSector; j++)
                {
                    Vector3 position = GenerateRandomPositionInSector(i);
                    GameObject stalactite = Instantiate(magicStalactiteTemp, position, Quaternion.identity);
                    sectorList.Add(stalactite.GetComponent<MagicStalactite>());
                }
                stalactites.Add(sectorList);
            }
        }

        private Vector3 GenerateRandomPositionInSector(int sectorIndex)
        {
            float sectorAngleSize = 360f / numberOfSector;
            float minAngle = sectorAngleSize * sectorIndex;
            float maxAngle = minAngle + sectorAngleSize;

            float angle = Random.Range(minAngle, maxAngle) * Mathf.Deg2Rad;
            float distance = Mathf.Sqrt(Random.Range(0f, 1f)) * fieldRadius;

            return new Vector3(
                Mathf.Cos(angle) * distance,
                fieldHeight,
                Mathf.Sin(angle) * distance
            );
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

            Transform playerTransform = payload.TransformValue1;
            Transform manaTransform = payload.TransformValue2;
            int attack = payload.IntValue;

            Debug.Log(playerTransform);
            Debug.Log(manaTransform);

            float jumpCheckValue = 1.0f;

            if (playerTransform != null)
            {
                // 플레이어 아래 광선을 쏴서 점프 체크
                RaycastHit hit;
                bool isJumping = !Physics.Raycast(playerTransform.position, -Vector3.up, out hit, jumpCheckValue);

                LayerMask groundLayer = LayerMask.GetMask("Ground");
                isJumping = !Physics.Raycast(playerTransform.position, -Vector3.up, out hit, jumpCheckValue, groundLayer);

                if (!isJumping)
                {
                    Debug.Log($"플레이어 피해 {attack} 입음");
                }
            }
            if (manaTransform != null)
            {
                // 해당 마나의 샘 쿨타임 적용, 삭제
                ManaFountain manaFountain = manaTransform.GetComponent<ManaFountain>();
                manaFountain.IsBroken = true;

                OnDestroyedMana(new BossEventPayload
                {
                    TransformValue1 = manaTransform,
                    AttackTypeValue = manaFountain.banBossAttackType,
                });
            }
        }

        private void OnDropMagicStalactite(BossEventPayload manaPayload)
        {
            Debug.Log($"OnDropMagicStalactite :: 종마석 드랍");

            
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