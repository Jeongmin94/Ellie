using Boss.Objects;
using Boss.Terrapupa;
using Assets.Scripts.Player;
using Channels.Boss;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Managers;
using Channels.Components;
using Assets.Scripts.Utils;
using Channels.Type;

namespace Centers.Boss
{
    public class TerrapupaCenter : BaseCenter
    {
        public GameObject playerStoneTemp;
        public GameObject magicStalactiteTemp;
        public GameObject magicStoneTemp;

        private MagicStoneTemp magicStone;

        public int numberOfSector = 3;
        public int stalactitePerSector = 3;
        public float fieldRadius = 25.0f;
        public float fieldHeight = 8.0f;

        public int bossDeathCheck = 0;

        [SerializeField] private TerrapupaController terrapupa;
        [SerializeField] private TerrapupaController terra;
        [SerializeField] private TerrapupaController pupa;
        [SerializeField] private PlayerController player;
        [SerializeField] private List<List<MagicStalactite>> stalactites = new List<List<MagicStalactite>>();

        private void Awake()
        {
            base.Init();
            SubscribeEvents();
            SpawnStalactites();
            SetBossTarget();
        }

        protected override void Start()
        {
            base.Start();

            CheckTicket(terrapupa.gameObject);
            CheckTicket(terra.gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                GameObject temp = Instantiate(magicStoneTemp, player.transform.position, Quaternion.identity);
                magicStone = temp.GetComponent<MagicStoneTemp>();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                terrapupa.terrapupaData.isTempted.Value = false;
                terrapupa.terrapupaData.isIntake.Value = false;
                terrapupa.terrapupaData.magicStoneTransform.Value = null;
                Destroy(magicStone.gameObject);

                magicStone = null;
            }
        }

        private void SubscribeEvents()
        {
            EventBus.Instance.Subscribe<IBaseEventPayload>(EventBusEvents.GripStoneByBoss1, OnSpawnStone);
            EventBus.Instance.Subscribe<IBaseEventPayload>(EventBusEvents.ThrowStoneByBoss1, OnThrowStone);
            EventBus.Instance.Subscribe<IBaseEventPayload>(EventBusEvents.OccurEarthQuake, OnStartEarthQuake);
            EventBus.Instance.Subscribe<BossEventPayload>(EventBusEvents.HitManaByPlayerStone, OnHitMana);
            EventBus.Instance.Subscribe<BossEventPayload>(EventBusEvents.DestroyedManaByBoss1, OnDestroyedMana);
            EventBus.Instance.Subscribe<BossEventPayload>(EventBusEvents.DropMagicStalactite, OnDropMagicStalactite);
            EventBus.Instance.Subscribe<BossEventPayload>(EventBusEvents.BossAttractedByMagicStone, OnBossAtrractedByMagicStone);
            EventBus.Instance.Subscribe<BossEventPayload>(EventBusEvents.BossUnattractedByMagicStone, OnBossUnattractedByMagicStone);
            EventBus.Instance.Subscribe<IBaseEventPayload>(EventBusEvents.IntakeMagicStoneByBoss1, OnIntakeMagicStoneByBoss1);
            EventBus.Instance.Subscribe<IBaseEventPayload>(EventBusEvents.BossDeath, OnBossDeath);
        }

        private void SetBossTarget()
        {
            terrapupa.terrapupaData.player.Value = player.transform;
            terrapupa.BossType = TerrapupaType.Pupa;
            //terrapupa.gameObject.SetActive(false);

            terra.terrapupaData.player.Value = player.transform;
            terrapupa.BossType = TerrapupaType.Pupa;
            //terra.gameObject.SetActive(false);
        }

        private void SpawnStalactites()
        {
            Transform objectTransform = transform.GetChild(0);

            for (int i = 0; i < numberOfSector; i++)
            {
                List<MagicStalactite> sectorList = new List<MagicStalactite>();
                for (int j = 0; j < stalactitePerSector; j++)
                {
                    Vector3 position = GenerateRandomPositionInSector(i);
                    GameObject stalactite = Instantiate(magicStalactiteTemp, position, Quaternion.identity, objectTransform);
                    MagicStalactite instantStalactite = stalactite.GetComponent<MagicStalactite>();
                    instantStalactite.MyIndex = i;
                    sectorList.Add(instantStalactite);
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

        private void OnSpawnStone(IBaseEventPayload payload)
        {
            Debug.Log("OnSpawnStone :: 보스의 돌맹이 줍기");
            BossEventPayload stonePayload = payload as BossEventPayload;
            TerrapupaController actor = stonePayload.Sender.GetComponent<TerrapupaController>();

            actor.Stone.gameObject.SetActive(true);
        }

        private void OnThrowStone(IBaseEventPayload payload)
        {
            Debug.Log("OnThrowStone :: 보스의 돌맹이 던지기");

            BossEventPayload stonePayload = payload as BossEventPayload;
            TerrapupaController actor = stonePayload.Sender.GetComponent<TerrapupaController>();

            Poolable stone = PoolManager.Instance.Pop(actor.Stone.gameObject, transform);
            stone.GetComponent<TerrapupaStone>().Init(actor.Stone.position, actor.transform.localScale, stonePayload.FloatValue, stonePayload.IntValue, stonePayload.Sender);
            stone.GetComponent<TerrapupaStone>().MoveToTarget(stonePayload.TransformValue1);

            actor.Stone.gameObject.SetActive(false);
        }

        private void OnHitMana(BossEventPayload manaPayload)
        {
            Debug.Log("OnHitMana :: 돌맹이 루팅");

            for (int i = 0; i < 3; i++)
            {
                Poolable playerStone = PoolManager.Instance.Pop(playerStoneTemp, transform);
                playerStone.transform.position = manaPayload.TransformValue2.position;

                Vector3 randomDirection = new Vector3(
                Random.Range(-1f, 1f), Random.Range(0.5f, 1f), Random.Range(-1f, 1f)).normalized;

                playerStone.GetComponent<Rigidbody>().AddForce(
                    randomDirection * 5.0f, ForceMode.Impulse);
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

            if (manaPayload.TransformValue2 != null)
            {
                Destroy(manaPayload.TransformValue2.gameObject);
            }

            TerrapupaController actor = manaPayload.Sender.GetComponent<TerrapupaController>();
            if (actor == null)
            {
                actor = manaPayload.Sender.GetComponent<TerrapupaStone>().Owner.GetComponent<TerrapupaController>();
                manaPayload.Sender = actor.transform;
            }

            switch (manaPayload.AttackTypeValue)
            {
                case TerrapupaAttackType.ThrowStone:
                    actor.terrapupaData.canThrowStone.Value = false;
                    break;
                case TerrapupaAttackType.EarthQuake:
                    actor.terrapupaData.canEarthQuake.Value = false;
                    break;
                case TerrapupaAttackType.Roll:
                    actor.terrapupaData.canRoll.Value = false;
                    break;
                case TerrapupaAttackType.LowAttack:
                    actor.terrapupaData.canLowAttack.Value = false;
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

            TerrapupaController actor = manaPayload.Sender.GetComponent<TerrapupaController>();
            switch (manaPayload.AttackTypeValue)
            {
                case TerrapupaAttackType.ThrowStone:
                    actor.terrapupaData.canThrowStone.Value = true;
                    break;
                case TerrapupaAttackType.EarthQuake:
                    actor.terrapupaData.canEarthQuake.Value = true;
                    break;
                case TerrapupaAttackType.Roll:
                    actor.terrapupaData.canRoll.Value = true;
                    break;
                case TerrapupaAttackType.LowAttack:
                    actor.terrapupaData.canLowAttack.Value = true;
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

            float jumpCheckValue = 0.3f;

            if (playerTransform != null)
            {
                // 플레이어 아래 광선을 쏴서 점프 체크
                RaycastHit hit;

                LayerMask groundLayer = LayerMask.GetMask("Ground");
                bool isJumping = !Physics.Raycast(playerTransform.position, -Vector3.up, out hit, jumpCheckValue, groundLayer);

                Debug.Log($"Raycast distance: {hit.distance}");
                if (isJumping)
                {
                    Debug.Log($"플레이어 피해 {attack} 입음");
                    float upPower = 10.0f;
                    Vector3 forceDirection = (Vector3.up * upPower);
                    playerTransform.gameObject.GetComponent<Rigidbody>().AddForce(forceDirection, ForceMode.Impulse);
                }
            }
            if (manaTransform != null)
            {
                // 해당 마나의 샘 쿨타임 적용, 삭제
                ManaFountain manaFountain = manaTransform.GetComponent<ManaFountain>();
                manaFountain.IsBroken = true;

                OnDestroyedMana(new BossEventPayload
                {
                    Sender = payload.Sender,
                    TransformValue1 = manaTransform,
                    AttackTypeValue = manaFountain.banBossAttackType,
                });
            }
        }

        private void OnDropMagicStalactite(BossEventPayload stalactitePayload)
        {
            Debug.Log($"OnDropMagicStalactite :: 종마석 드랍");

            Transform boss = stalactitePayload.TransformValue2;
            if (boss != null)
            {
                TerrapupaController actor = boss.GetComponent<TerrapupaController>();
                if (stalactitePayload.TransformValue2 != null)
                {
                    Debug.Log("보스 타격");

                    if (actor.terrapupaData.isIntake.Value)
                    {
                        Debug.Log("섭취 -> 기절");

                        actor.terrapupaData.isStuned.Value = true;
                        actor.terrapupaData.isTempted.Value = false;
                        actor.terrapupaData.isIntake.Value = false;
                    }
                }
            }

            StartCoroutine(RespawnMagicStalactite(stalactitePayload));
        }

        private IEnumerator RespawnMagicStalactite(BossEventPayload payload)
        {
            float respawnTime = payload.FloatValue;
            Debug.Log($"{respawnTime}초 이후 재생성");

            yield return new WaitForSeconds(respawnTime);

            Vector3 position = GenerateRandomPositionInSector(payload.IntValue);
            payload.TransformValue1.position = position;
            payload.TransformValue1.gameObject.SetActive(true);
        }

        private void OnBossAtrractedByMagicStone(BossEventPayload magicStonePayload)
        {
            Debug.Log($"OnBossAtrractedByMagicStone :: 보스 마법 돌맹이를 추적 시작");

            TerrapupaController actor = magicStonePayload.TransformValue2.GetComponent<TerrapupaController>();
            actor.terrapupaData.isTempted.Value = true;
            actor.terrapupaData.isIntake.Value = false;
            actor.terrapupaData.magicStoneTransform.Value = magicStonePayload.TransformValue1;
        }

        private void OnBossUnattractedByMagicStone(BossEventPayload magicStonePayload)
        {
            Debug.Log($"OnBossUnattractedByMagicStone :: 보스 마법 돌맹이를 추적 종료");

            TerrapupaController actor = magicStonePayload.TransformValue2.GetComponent<TerrapupaController>();
            actor.terrapupaData.isTempted.Value = false;
            actor.terrapupaData.isIntake.Value = false;
            actor.terrapupaData.magicStoneTransform.Value = null;
        }

        private void OnIntakeMagicStoneByBoss1(IBaseEventPayload bossPayload)
        {
            Debug.Log($"OnIntakeMagicStoneByBoss1 :: 보스가 마법 돌맹이를 섭취함");

            BossEventPayload payload = bossPayload as BossEventPayload;

            Transform _magicStone = payload.TransformValue1;

            TerrapupaController actor = payload.Sender.GetComponent<TerrapupaController>();
            actor.terrapupaData.isTempted.Value = false;
            actor.terrapupaData.isIntake.Value = false;
            actor.terrapupaData.magicStoneTransform.Value = null;
            Destroy(_magicStone.gameObject);
            magicStone = null;
        }

        private void OnBossDeath(IBaseEventPayload bossPayload)
        {
            Debug.Log($"OnBossDeath :: 보스가 사망");

            BossEventPayload payload = bossPayload as BossEventPayload;
            payload.Sender.gameObject.SetActive(false);

            bossDeathCheck++;

            // 2페이즈 확인용 임시
            switch (bossDeathCheck)
            {
                case 1:
                    Debug.Log("한마리 잡음");
                    break;
                case 2:
                    Debug.Log("다잡음");
                    break;
                case 3:
                    break;
                default:
                    break;
            }

        }

        private void OnGUI()
        {
            int boxWidth = 200;
            int boxHeight = 70; // 충분한 높이 설정
            int offsetX = 10;
            int offsetY = 10;

            if (terrapupa)
            {
                offsetY = 10;
                Vector3 directionToBoss = terrapupa.transform.position - player.transform.position;
                float distance = directionToBoss.magnitude;
                string distanceText = "Distance: " + distance.ToString("F2");
                string hpText = "HP: " + terrapupa.terrapupaData.currentHP.value.ToString("F2");
                string nameText = "Name: " + terrapupa.gameObject.name;

                Rect boxRect = new Rect(Screen.width - boxWidth - offsetX, offsetY, boxWidth, boxHeight);
                GUI.Box(boxRect, nameText);
                GUI.Label(new Rect(boxRect.x + 20, boxRect.y + 25, boxWidth, boxHeight), distanceText);
                GUI.Label(new Rect(boxRect.x + 20, boxRect.y + 45, boxWidth, boxHeight), hpText);
            }
            if (terra)
            {
                offsetY = 90;
                Vector3 directionToBoss = terra.transform.position - player.transform.position;
                float distance = directionToBoss.magnitude;
                string distanceText = "Distance: " + distance.ToString("F2");
                string hpText = "HP: " + terra.terrapupaData.currentHP.value.ToString("F2");
                string nameText = "Name: " + terra.gameObject.name;

                Rect boxRect = new Rect(Screen.width - boxWidth - offsetX, offsetY, boxWidth, boxHeight);
                GUI.Box(boxRect, nameText);
                GUI.Label(new Rect(boxRect.x + 20, boxRect.y + 25, boxWidth, boxHeight), distanceText);
                GUI.Label(new Rect(boxRect.x + 20, boxRect.y + 45, boxWidth, boxHeight), hpText);
            }
            if (magicStone)
            {
                offsetY = 170;
                Vector3 directionToBoss = magicStone.transform.position - player.transform.position;
                float distance = directionToBoss.magnitude;
                string distanceText = "Distance: " + distance.ToString("F2");
                string nameText = "Name: " + magicStone.gameObject.name;

                Rect boxRect = new Rect(Screen.width - boxWidth - offsetX, offsetY, boxWidth, boxHeight);
                GUI.Box(boxRect, nameText);
                GUI.Label(new Rect(boxRect.x + 20, boxRect.y + 25, boxWidth, boxHeight), distanceText);
            }
        }

        private void OnDrawGizmos()
        {
            float rayLength = 7.0f;
            Color rayColor = Color.red;
            Gizmos.color = rayColor;

            if (terrapupa)
            {
                Gizmos.DrawRay(terrapupa.transform.position, terrapupa.transform.forward * rayLength);
            }
            if (terra)
            {
                Gizmos.DrawRay(terra.transform.position, terra.transform.forward * rayLength);
            }
        }
    }
}