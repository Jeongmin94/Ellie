using Boss.Objects;
using Boss.Terrapupa;
using Assets.Scripts.Player;
using Channels.Boss;
using System.Collections;
using UnityEngine;
using Assets.Scripts.Managers;
using Channels.Type;
using Channels.Combat;
using Assets.Scripts.StatusEffects;
using System.Collections.Generic;
using Assets.Scripts.Particle;
using Channels.Components;

namespace Centers.Boss
{
    public class TerrapupaCenter : MonoBehaviour
    {
        //[SerializeField] private StoneHatchery hatchery;
        [SerializeField] private TerrapupaController terrapupa;
        [SerializeField] private TerrapupaController terra;
        [SerializeField] private TerrapupaController pupa;
        [SerializeField] private List<TerrapupaMinionController> minions;
        [SerializeField] private PlayerController player;
        [SerializeField] private TerrapupaMapObjectController terrapupaMapObjects;

        public float fallCheckLatency = 5.0f;
        public bool isActiveTerrapupa = true;
        public bool isActiveTerra = false;
        public bool isActivePupa = false;
        public bool isActiveMinions = false;

        public int currentLevel = 1;   // 1페이즈, 2페이즈 체크용
        public int minionDeathCheck = 4;

        private void Awake()
        {
            SubscribeEvents();
        }
        private void Start()
        {
            CheckTickets();
            SetBossInfo();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("테라푸파 사망 치트");
                Vector3 pos = terrapupa.transform.position;

                terrapupa.transform.position = new Vector3(pos.x, -1.0f, pos.z);
                terrapupa.terrapupaData.currentHP.Value = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("테라, 푸파 사망 치트");
                Vector3 pos1 = terra.transform.position;
                Vector3 pos2 = pupa.transform.position;

                terrapupa.transform.position = new Vector3(pos1.x, -1.0f, pos1.z);
                terrapupa.transform.position = new Vector3(pos2.x, -1.0f, pos2.z);
                terra.terrapupaData.currentHP.Value = 0;
                pupa.terrapupaData.currentHP.Value = 0;
            }
        }

        #region 1. 초기화 함수
        private void SetBossInfo()
        {
            terrapupa.terrapupaData.player.Value = player.transform;
            terra.terrapupaData.player.Value = player.transform;
            pupa.terrapupaData.player.Value = player.transform;
            foreach (var minion in minions)
            {
                minion.minionData.player.Value = player.transform;
            }

            terrapupa.gameObject.SetActive(isActiveTerrapupa);
            terra.gameObject.SetActive(isActiveTerra);
            pupa.gameObject.SetActive(isActivePupa);
            foreach (var minion in minions)
            {
                minion.gameObject.SetActive(isActiveMinions);
            }

            // 1, 2페이즈 체크 (임시)
            currentLevel = isActiveTerrapupa ? 1 : 2;
        }
        private void CheckTickets()
        {
            TicketManager.Instance.CheckTicket(terrapupa.gameObject);
            TicketManager.Instance.CheckTicket(terra.gameObject);
            TicketManager.Instance.CheckTicket(pupa.gameObject);
            TicketManager.Instance.CheckTicket(terrapupaMapObjects.gameObject);
            foreach (var minion in minions)
            {
                TicketManager.Instance.CheckTicket(minion.gameObject);
            }
        }
        private void SubscribeEvents()
        {
            EventBus.Instance.Subscribe<IBaseEventPayload>(EventBusEvents.GripStoneByBoss1, OnSpawnStone);
            EventBus.Instance.Subscribe<IBaseEventPayload>(EventBusEvents.ThrowStoneByBoss1, OnThrowStone);
            EventBus.Instance.Subscribe<IBaseEventPayload>(EventBusEvents.OccurEarthQuake, OnStartEarthQuake);
            EventBus.Instance.Subscribe<BossEventPayload>(EventBusEvents.ApplyBossCooldown, OnBossApplyAttackCooldown);
            EventBus.Instance.Subscribe<BossEventPayload>(EventBusEvents.BossAttractedByMagicStone, OnBossAtrractedByMagicStone);
            EventBus.Instance.Subscribe<BossEventPayload>(EventBusEvents.BossUnattractedByMagicStone, OnBossUnattractedByMagicStone);
            EventBus.Instance.Subscribe<IBaseEventPayload>(EventBusEvents.IntakeMagicStoneByBoss1, OnIntakeMagicStoneByBoss1);
            EventBus.Instance.Subscribe<IBaseEventPayload>(EventBusEvents.BossDeath, OnBossDeath);
            EventBus.Instance.Subscribe<BossEventPayload>(EventBusEvents.HitStone, OnHitStone);
            EventBus.Instance.Subscribe<IBaseEventPayload>(EventBusEvents.BossMeleeAttack, OnBossMeleeAttack);
            EventBus.Instance.Subscribe<IBaseEventPayload>(EventBusEvents.BossLowAttack, OnBossLowAttack);
            EventBus.Instance.Subscribe<IBaseEventPayload>(EventBusEvents.BossMinionAttack, OnBossMinionAttack);
            EventBus.Instance.Subscribe(EventBusEvents.DestroyAllManaFountain, OnDestroyAllManaFountains);
            EventBus.Instance.Subscribe<IBaseEventPayload>(EventBusEvents.ApplySingleBossCooldown, OnApplySingleBossCooldown);
        }
        #endregion

        #region 2. 이벤트 핸들러
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
            stone.GetComponent<TerrapupaStone>().Init(actor.Stone.position, actor.transform.localScale, stonePayload.FloatValue, stonePayload.IntValue, stonePayload.PrefabValue, stonePayload.Sender, actor.TicketMachine);
            stone.GetComponent<TerrapupaStone>().MoveToTarget(stonePayload.TransformValue1);

            actor.Stone.gameObject.SetActive(false);
        }
        private void OnBossApplyAttackCooldown(BossEventPayload payload)
        {
            Debug.Log("OnBossApplyAttackCooldown :: 쿨타임 적용");

            TerrapupaAttackType type = payload.AttackTypeValue;
            bool isCooldownDone = payload.BoolValue;

            // 봉인 적용
            terrapupa.ApplyAttackAvailable(type, isCooldownDone);
            terra.ApplyAttackAvailable(type, isCooldownDone);
            pupa.ApplyAttackAvailable(type, isCooldownDone);
        }
        private void OnStartEarthQuake(IBaseEventPayload earthQuakePayload)
        {
            Debug.Log($"OnStartEarthQuake :: 내려찍기 공격 피격 체크");
            BossEventPayload payload = earthQuakePayload as BossEventPayload;

            if(payload == null)
            {
                return;
            }

            GameObject hitEffect = payload.PrefabValue;
            Transform playerTransform = payload.TransformValue1;
            Transform manaTransform = payload.TransformValue2;
            Transform bossTransform = payload.TransformValue3;
            Transform boss = payload.Sender;
            int attack = payload.IntValue;

            float jumpCheckValue = 1.0f;

            if (playerTransform != null)
            {
                // 플레이어 아래 광선을 쏴서 점프 체크
                RaycastHit hit;

                LayerMask groundLayer = 1 << LayerMask.NameToLayer("Ground");
                bool isJumping = !Physics.Raycast(playerTransform.position + Vector3.up * 0.1f, -Vector3.up, out hit, jumpCheckValue + 0.1f, groundLayer);

                Debug.Log($"Raycast distance: {hit.distance}");
                if (!isJumping)
                {
                    TerrapupaController bossController = boss.GetComponent<TerrapupaController>();

                    HitedPlayer(bossController.TicketMachine, boss, playerTransform, payload.CombatPayload);
                    ParticleManager.Instance.GetParticle(hitEffect, playerTransform, 0.5f);
                }
            }
            if (manaTransform != null)
            {
                HitedManaFountaine(boss, manaTransform, hitEffect);
            }
            if(bossTransform != null)
            {
                // 보스 체크
                TerrapupaRootData target = bossTransform.GetComponent<TerrapupaController>().terrapupaData;

                target.hitEarthQuake.Value = true;
                ParticleManager.Instance.GetParticle(hitEffect, bossTransform, 1.0f);
            }
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
            int healValue = payload.IntValue;

            TerrapupaController actor = payload.Sender.GetComponent<TerrapupaController>();
            actor.GetHealed(healValue);
            actor.terrapupaData.isTempted.Value = false;
            actor.terrapupaData.isIntake.Value = false;
            actor.terrapupaData.magicStoneTransform.Value = null;

            if (_magicStone != null)
            {
                Destroy(_magicStone.gameObject);
            }
        }
        private void OnBossDeath(IBaseEventPayload bossPayload)
        {
            Debug.Log($"OnBossDeath :: 보스가 사망");

            BossEventPayload payload = bossPayload as BossEventPayload;
            Debug.Log(payload.Sender);
            if(payload.Sender != null)
            {
                string bossName = payload.Sender.name;

                if(bossName == "Terrapupa")
                {
                    Debug.Log("테라푸파 사망, 2페이즈");
                    SpawnTerraAndPupa();
                }
                else if(bossName == "Terra" || bossName == "Pupa")
                {
                    Debug.Log("테라 혹은 푸파 사망, 미니언 2마리 소환");
                    SpawnMinions(payload.Sender);
                }
                else
                {
                    minionDeathCheck--;
                    Debug.Log($"미니언 사망, 남은 미니언 {minionDeathCheck}");
                }

                payload.Sender.gameObject.SetActive(false);
            }

            if(minionDeathCheck == 0)
            {
                Debug.Log("보스 클리어");
            }
        }
        private void OnHitStone(BossEventPayload bossPayload)
        {
            Debug.Log($"OnHitStone :: 보스가 돌에 맞음");

            TerrapupaRootData target = bossPayload.TransformValue1.GetComponent<TerrapupaController>().terrapupaData;
            target.hitThrowStone.Value = true;
        }
        private void OnBossMeleeAttack(IBaseEventPayload bossPayload)
        {
            Debug.Log($"OnMeleeAttack :: 보스의 근접 공격");

            BossEventPayload payload = bossPayload as BossEventPayload;

            if (payload == null)
            {
                return;
            }

            Transform playerTransform = payload.TransformValue1;
            Transform manaTransform = payload.TransformValue2;
            Transform boss = payload.Sender;
            int attackValue = payload.IntValue;

            if (playerTransform != null)
            {
                TerrapupaController bossController = boss.GetComponent<TerrapupaController>();
                HitedPlayer(bossController.TicketMachine, boss, playerTransform, payload.CombatPayload);
            }
            if (manaTransform != null)
            {
                HitedManaFountaine(boss, manaTransform);
            }
        }
        private void OnBossLowAttack(IBaseEventPayload bossPayload)
        {
            Debug.Log($"OnLowAttack");
            BossEventPayload payload = bossPayload as BossEventPayload;

            if (payload == null)
            {
                return;
            }

            Transform playerTransform = payload.TransformValue1;
            Transform manaTransform = payload.TransformValue2;
            Transform boss = payload.Sender;

            float jumpCheckValue = 1.0f;

            if (playerTransform != null)
            {
                // 플레이어 아래 광선을 쏴서 점프 체크
                RaycastHit hit;

                LayerMask groundLayer = 1 << LayerMask.NameToLayer("Ground");
                bool isJumping = !Physics.Raycast(playerTransform.position + Vector3.up * 0.1f, -Vector3.up, out hit, jumpCheckValue + 0.1f, groundLayer);

                Debug.Log($"Raycast distance: {hit.distance}");
                if (!isJumping)
                { 
                    TerrapupaController bossController = boss.GetComponent<TerrapupaController>();
                    HitedPlayer(bossController.TicketMachine, boss, playerTransform, payload.CombatPayload);
                }
            }
            if (manaTransform != null)
            {
                HitedManaFountaine(payload.Sender, manaTransform);
            }
        }
        private void OnBossMinionAttack(IBaseEventPayload bossPayload)
        {
            Debug.Log($"OnMinionAttack :: 보스 미니언의 공격");

            BossEventPayload payload = bossPayload as BossEventPayload;

            if (payload == null)
            {
                return;
            }

            Transform playerTransform = payload.TransformValue1;
            Transform minion = payload.Sender;
            int attackValue = payload.IntValue;

            Debug.Log(playerTransform);

            if (playerTransform != null)
            {
                TerrapupaMinionController minionController = minion.GetComponent<TerrapupaMinionController>();
                HitedPlayer(minionController.TicketMachine, minion, playerTransform, new CombatPayload
                {
                    Damage = attackValue,
                    PlayerStatusEffectName = StatusEffectName.WeakRigidity,
                    statusEffectduration = 0.05f,
                });
            }

            StartCoroutine(MinionAttackCooldown(payload));
        }
        private void OnDestroyAllManaFountains()
        {
            Debug.Log($"OnDestroyAllManaFountains :: 보스 즉사");

            switch (currentLevel)
            {
                case 1:
                    currentLevel++;
                    Debug.Log("테라푸파 사망, 즉사");
                    Vector3 pos = terrapupa.transform.position;
                    
                    terrapupa.transform.position = new Vector3(pos.x, -1.0f, pos.z);
                    terrapupa.terrapupaData.currentHP.Value = 0;
                    break;
                case 2:
                    Debug.Log("테라, 푸파 사망, 즉사");
                    Vector3 pos1 = terra.transform.position;
                    Vector3 pos2 = pupa.transform.position;

                    terrapupa.transform.position = new Vector3(pos1.x, -1.0f, pos1.z);
                    terrapupa.transform.position = new Vector3(pos2.x, -1.0f, pos2.z);
                    terra.terrapupaData.currentHP.Value = 0;
                    pupa.terrapupaData.currentHP.Value = 0;
                    break;
            }
        }
        private void OnApplySingleBossCooldown(IBaseEventPayload cooldownPayload)
        {
            Debug.Log("ApplyBossAttackCooldown :: 쿨타임 적용");
            BossEventPayload payload = cooldownPayload as BossEventPayload;

            TerrapupaController bossController = payload.Sender.GetComponent<TerrapupaController>();
            float cooldown = payload.FloatValue;
            TerrapupaAttackType banType = payload.AttackTypeValue;

            bossController.Cooldown(cooldown, banType);
        }
        #endregion

        #region 3. 코루틴 함수
        private IEnumerator MinionAttackCooldown(BossEventPayload payload)
        {
            TerrapupaMinionController minionController = payload.Sender.GetComponent<TerrapupaMinionController>();
            Debug.Log($"{minionController} 공격 봉인");
            minionController.minionData.canAttack.Value = false;

            yield return new WaitForSeconds(5.0f);

            Debug.Log($"MinionAttackCooldown :: {minionController} 쿨다운 완료");
            minionController.minionData.canAttack.Value = true;
        }
        #endregion
        
        #region 4. 기타 함수
        private void HitedPlayer(TicketMachine ticketMachine, Transform attacker, Transform player, CombatPayload payload)
        {
            Debug.Log($"플레이어 피해 {payload.Damage} 입음");
            payload.Attacker = attacker;
            payload.Defender = player;

            ticketMachine.SendMessage(ChannelType.Combat, payload);
        }
        private void HitedManaFountaine(Transform attacker, Transform manaTransform, GameObject hitEffect = null)
        {
            ManaFountain manaFountain = manaTransform.GetComponent<ManaFountain>();
            manaFountain.IsBroken = true;

            EventBus.Instance.Publish(EventBusEvents.DestroyedManaByBoss1,
                new BossEventPayload
                {
                    PrefabValue = hitEffect,
                    Sender = attacker,
                    TransformValue1 = manaTransform,
                    AttackTypeValue = manaFountain.banBossAttackType,
                });
        }
        private void SpawnTerraAndPupa()
        {
            Debug.Log("SpawnTerraAndPupa :: 테라, 푸파 소환");

            terra.gameObject.SetActive(true);
            pupa.gameObject.SetActive(true);

            terra.transform.position = terrapupa.transform.position;
            pupa.transform.position = terrapupa.transform.position;

            terra.terrapupaData.player.Value = player.transform;
            pupa.terrapupaData.player.Value = player.transform;
        }
        private void SpawnMinions(Transform obj)
        {
            Debug.Log("SpawnMinions :: 테, 라, 푸, 파 랜덤 2마리 소환");

            Vector3 position = obj.position;

            for (int i = 0; i < 2; i++)
            {
                int index = Random.Range(0, minions.Count);
                TerrapupaMinionController minion = minions[index];

                minion.gameObject.SetActive(true);

                minion.transform.position = position;
                minion.minionData.player.Value = player.transform;

                minions.RemoveAt(index);
            }
        }
        #endregion
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
            if (pupa)
            {
                offsetY = 170;
                Vector3 directionToBoss = pupa.transform.position - player.transform.position;
                float distance = directionToBoss.magnitude;
                string distanceText = "Distance: " + distance.ToString("F2");
                string hpText = "HP: " + pupa.terrapupaData.currentHP.value.ToString("F2");
                string nameText = "Name: " + pupa.gameObject.name;

                Rect boxRect = new Rect(Screen.width - boxWidth - offsetX, offsetY, boxWidth, boxHeight);
                GUI.Box(boxRect, nameText);
                GUI.Label(new Rect(boxRect.x + 20, boxRect.y + 25, boxWidth, boxHeight), distanceText);
                GUI.Label(new Rect(boxRect.x + 20, boxRect.y + 45, boxWidth, boxHeight), hpText);
            }
        }
    }
}