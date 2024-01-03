﻿using System.Collections;
using System.Collections.Generic;
using Boss1.BossRoomObjects;
using Boss1.Terrapupa;
using Boss1.TerrapupaMinion;
using Channels.Boss;
using Channels.Combat;
using Channels.Components;
using Channels.Stone;
using Channels.Type;
using Controller;
using Item.Stone;
using Managers.Event;
using Managers.Particle;
using Managers.Sound;
using Player;
using Sirenix.OdinInspector;
using TheKiwiCoder;
using UnityEngine;
using Utils;

namespace Boss1
{
    public class TerrapupaLogicController : BaseController
    {
        [Title("테라푸파 보스 객체")] 
        [SerializeField] [Required] private PlayerController player;
        [SerializeField] [Required] private TerrapupaBehaviourController terrapupa;
        [SerializeField] [Required] private TerrapupaBehaviourController terra;
        [SerializeField] [Required] private TerrapupaBehaviourController pupa;
        [SerializeField] [Required] private List<TerrapupaMinionBehaviourController> minions;

        [Title("현재 페이즈 상태 체크용")] 
        [SerializeField] [ReadOnly] private int currentLevel = 1; // 1페이즈, 2페이즈 체크용
        [SerializeField] [ReadOnly] private int minionDeathCheck = 4; // 3페이즈 미니언 4마리 체크
        [SerializeField] [ReadOnly] private int currentMinionSpawnIndex;
        [SerializeField] [ReadOnly] private float fallCheckLatency = 5.0f;

        [Title("보스몬스터 생성 여부")] 
        [InfoBox("박스 체크 시 해당 몬스터가 활성화 됩니다")] 
        public bool isActiveTerrapupa = true;
        public bool isActiveTerra;
        public bool isActivePupa;
        public bool isActiveMinions;
        
        private TicketMachine ticketMachine;
        
        private int golemCoreCount;
        private bool isFirstDestroyAllManaFountain;
        private bool isFirstTerrapupaHeal;

        private const int GOLEM_CORE_INDEX = 4021;

        private void Start()
        {
            ShuffleMinions();
            SetBossInfo();
            StartCoroutine(FallCheck());
            StartCoroutine(NullCheck());
        }

        public override void InitController()
        {
            Debug.Log($"{name} InitController");

            SubscribeEvents();
            InitTicketMachine();
        }

        #region 1. 초기화 함수

        private void SetBossInfo()
        {
            terrapupa.TerrapupaData.player.Value = player.transform;
            terra.TerrapupaData.player.Value = player.transform;
            pupa.TerrapupaData.player.Value = player.transform;
            foreach (var minion in minions)
            {
                minion.MinionData.player.Value = player.transform;
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

        private void InitTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets
                (ChannelType.Stone, ChannelType.Combat, ChannelType.Camera, 
                    ChannelType.BossBattle, ChannelType.BossDialog);
            ticketMachine.RegisterObserver(ChannelType.BossBattle, OnNotifyBossBattle);

            terrapupa.InitTicketMachine(ticketMachine);
            terra.InitTicketMachine(ticketMachine);
            pupa.InitTicketMachine(ticketMachine);
            foreach (var minion in minions)
            {
                minion.InitTicketMachine(ticketMachine);
            }
        }

        private void SubscribeEvents()
        {
            EventBus.Instance.Subscribe(EventBusEvents.GripStoneByBoss1, OnSpawnStone);
            EventBus.Instance.Subscribe(EventBusEvents.ThrowStoneByBoss1, OnThrowStone);
            EventBus.Instance.Subscribe(EventBusEvents.OccurEarthQuake, OnStartEarthQuake);
            EventBus.Instance.Subscribe(EventBusEvents.BossMeleeAttack, OnBossMeleeAttack);
            EventBus.Instance.Subscribe(EventBusEvents.BossLowAttack, OnBossLowAttack);
            EventBus.Instance.Subscribe(EventBusEvents.BossMinionAttack, OnBossMinionAttack);
            EventBus.Instance.Subscribe(EventBusEvents.ApplyBossCooldown, OnBossApplyAttackCooldown);
            EventBus.Instance.Subscribe(EventBusEvents.BossAttractedByMagicStone, OnBossAtrractedByMagicStone);
            EventBus.Instance.Subscribe(EventBusEvents.BossUnattractedByMagicStone, OnBossUnattractedByMagicStone);
            EventBus.Instance.Subscribe(EventBusEvents.IntakeMagicStoneByBoss1, OnIntakeMagicStoneByBoss1);
            EventBus.Instance.Subscribe(EventBusEvents.BossDeath, OnBossDeath);
            EventBus.Instance.Subscribe(EventBusEvents.HitStone, OnHitStone);
            EventBus.Instance.Subscribe(EventBusEvents.DestroyAllManaFountain, OnDestroyAllManaFountains);
            EventBus.Instance.Subscribe(EventBusEvents.ApplySingleBossCooldown, OnApplySingleBossCooldown);
            EventBus.Instance.Subscribe(EventBusEvents.StartIntakeMagicStone, OnStartIntakeMagicStone);
        }

        private void ShuffleMinions()
        {
            // 미니언 리스트 랜덤 셔플
            for (var i = 0; i < minions.Count; i++)
            {
                var randomIndex = Random.Range(i, minions.Count);
                (minions[i], minions[randomIndex]) = (minions[randomIndex], minions[i]);
            }
        }

        #endregion

        #region 2. 이벤트 핸들러

        private void OnNotifyBossBattle(IBaseEventPayload payload)
        {
            if (payload is not TerrapupaBattlePayload bPayload)
            {
                return;
            }

            switch (bPayload.SituationType)
            {
                case TerrapupaSituationType.StartBattle:
                    OnStartBattle();
                    break;
                case TerrapupaSituationType.StartSeconPhase:
                    OnStartSecondPhase();
                    break;
                case TerrapupaSituationType.StartThirdPhase:
                    //OnStartThirdPhase(bPayload.Sender);
                    break;
            }
        }

        private void OnStartBattle()
        {
            Debug.Log("OnStartBattle()");
            // 전투 시작 시, 테라푸파 활성화
            terrapupa.StartBattle(player.transform);
        }

        private void OnStartSecondPhase()
        {
            Debug.Log("OnStartSeconPhase()");
            // 테라, 푸파 활성화
            SpawnTerraAndPupa();
            TerrapupaDialogChannel.SendMessage(TerrapupaDialogTriggerType.StartSecondPhase, ticketMachine);
        }

        private void OnStartThirdPhase(Transform boss)
        {
            Debug.Log("OnStartThirdPhase()");
            // 테라, 푸파 활성화
            SpawnMinions(boss.position);
            TerrapupaDialogChannel.SendMessage(TerrapupaDialogTriggerType.StartThirdPhase, ticketMachine);
        }

        private void OnSpawnStone(IBaseEventPayload payload)
        {
            Debug.Log("OnSpawnStone :: 보스의 돌맹이 줍기");
            var stonePayload = payload as BossEventPayload;

            var actor = stonePayload.Sender.GetComponent<TerrapupaBehaviourController>();

            actor.Stone.gameObject.SetActive(true);
        }

        private void OnThrowStone(IBaseEventPayload payload)
        {
            Debug.Log("OnThrowStone :: 보스의 돌맹이 던지기");

            var stonePayload = payload as BossEventPayload;
            var actor = stonePayload.Sender.GetComponent<TerrapupaBehaviourController>();

            var stone = Instantiate(actor.Stone.gameObject, transform).GetComponent<TerrapupaStone>();
            
            stone.Init(actor.Stone.position, actor.transform.localScale,
                stonePayload.FloatValue, stonePayload.CombatPayload, stonePayload.PrefabValue, 
                stonePayload.Sender, ticketMachine);
            stone.MoveToTarget(stonePayload.TransformValue1);

            actor.Stone.gameObject.SetActive(false);
        }

        private void OnBossApplyAttackCooldown(IBaseEventPayload baseEventPayload)
        {
            if (baseEventPayload is not BossEventPayload payload)
            {
                return;
            }

            Debug.Log("OnBossApplyAttackCooldown :: 쿨타임 적용");

            var type = payload.AttackTypeValue;
            var isCooldownDone = payload.BoolValue;

            // 봉인 적용
            terrapupa.ApplyAttackAvailable(type, isCooldownDone);
            terra.ApplyAttackAvailable(type, isCooldownDone);
            pupa.ApplyAttackAvailable(type, isCooldownDone);
        }

        private void OnStartEarthQuake(IBaseEventPayload earthQuakePayload)
        {
            Debug.Log("OnStartEarthQuake :: 내려찍기 공격 피격 체크");
            var payload = earthQuakePayload as BossEventPayload;

            if (payload == null)
            {
                return;
            }

            var hitEffect = payload.PrefabValue;
            var playerTransform = payload.TransformValue1;
            var manaTransform = payload.TransformValue2;
            var hitBossTransform = payload.TransformValue3;
            var boss = payload.Sender;

            // 1. 플레이어 피격
            if (playerTransform != null)
            {
                // 플레이어 아래 광선을 쏴서 점프 체크
                RaycastHit hit;
                var jumpCheckValue = 1.0f;

                LayerMask groundLayer = 1 << LayerMask.NameToLayer("Ground");
                var isJumping = !Physics.Raycast(playerTransform.position + Vector3.up * 0.1f, -Vector3.up, out hit,
                    jumpCheckValue + 0.1f, groundLayer);

                if (hit.collider != null)
                {
                    Debug.Log($"Raycast distance: {hit.distance}");
                }

                if (!isJumping)
                {
                    HitedPlayer(boss, playerTransform, payload.CombatPayload);
                    ParticleManager.Instance.GetParticle(hitEffect, playerTransform, 0.5f);
                }
            }

            // 2. 마나의 샘 피격
            if (manaTransform != null)
            {
                HitedManaFountaine(boss, manaTransform, hitEffect);
            }

            // 3. 타 보스 피격
            if (hitBossTransform != null)
            {
                hitBossTransform = hitBossTransform.GetComponent<TerrapupaDetection>().MyTerrapupa;
                var hitBossControllers = hitBossTransform.GetComponent<TerrapupaBehaviourController>();
                if (hitBossControllers?.TerrapupaData != null)
                {
                    hitBossControllers.TerrapupaData.hitEarthQuake.Value = true;
                    ParticleManager.Instance.GetParticle(hitEffect, hitBossTransform);
                }
            }
        }

        private void OnBossAtrractedByMagicStone(IBaseEventPayload payload)
        {
            if (payload is not BossEventPayload magicStonePayload)
            {
                return;
            }

            Debug.Log("OnBossAtrractedByMagicStone :: 보스 마법 돌맹이를 추적 시작");

            var magicStone = magicStonePayload.TransformValue1;
            var target = magicStonePayload.TransformValue2;
            if (target)
            {
                var actor = target.GetComponent<TerrapupaBehaviourController>();

                actor.AttractMagicStone(magicStone);
            }
        }

        private void OnBossUnattractedByMagicStone(IBaseEventPayload payload)
        {
            if (payload is not BossEventPayload magicStonePayload)
            {
                return;
            }

            Debug.Log("OnBossUnattractedByMagicStone :: 보스 마법 돌맹이를 추적 종료");

            var target = magicStonePayload.TransformValue2;
            if (target)
            {
                var bossController = target.GetComponent<TerrapupaBehaviourController>();
                bossController.UnattractMagicStone();
            }
        }

        private void OnIntakeMagicStoneByBoss1(IBaseEventPayload bossPayload)
        {
            Debug.Log("OnIntakeMagicStoneByBoss1 :: 보스가 마법 돌맹이를 섭취함");

            var payload = bossPayload as BossEventPayload;

            var actor = payload.Sender.GetComponent<TerrapupaBehaviourController>();
            var _magicStone = payload.TransformValue1;
            var healValue = payload.IntValue;

            actor.EndIntakeMagicStone(healValue);

            if (!actor.IsDead && healValue != 0 && !isFirstTerrapupaHeal)
            {
                isFirstTerrapupaHeal = true;
                TerrapupaDialogChannel.SendMessage(TerrapupaDialogTriggerType.IntakeMagicStoneFirstTime, ticketMachine);
            }

            if (_magicStone != null)
            {
                Destroy(_magicStone.gameObject);
            }
        }

        private void OnBossDeath(IBaseEventPayload bossPayload)
        {
            Debug.Log("OnBossDeath :: 보스가 사망");

            var payload = bossPayload as BossEventPayload;
            Debug.Log(payload.Sender);
            if (payload.Sender != null)
            {
                var bossName = payload.Sender.name;

                if (bossName == "Terrapupa")
                {
                    Debug.Log("테라푸파 사망, 2페이즈");
                    OnStartSecondPhase();
                }
                else if (bossName == "Terra" || bossName == "Pupa")
                {
                    Debug.Log("테라 혹은 푸파 사망, 미니언 2마리 소환");
                    OnStartThirdPhase(payload.Sender);
                }
                else
                {
                    minionDeathCheck--;
                    Debug.Log($"미니언 사망, 남은 미니언 {minionDeathCheck}");

                    if (minionDeathCheck == 2 || minionDeathCheck == 0)
                    {
                        Debug.Log("골렘의 눈 드랍");
                        var position = payload.Sender.position;

                        StoneChannel.DropStone(ticketMachine, position, GOLEM_CORE_INDEX);
                        
                        golemCoreCount++;
                        if (golemCoreCount == 1)
                        {
                            TerrapupaDialogChannel.SendMessage(TerrapupaDialogTriggerType.GetGolemCoreFirstTime,
                                ticketMachine);
                        }
                        else if (golemCoreCount == 2)
                        {
                            TerrapupaDialogChannel.SendMessage(TerrapupaDialogTriggerType.DieAllMinions, ticketMachine);
                        }
                    }
                }

                payload.Sender.gameObject.SetActive(false);
            }
        }

        private void OnHitStone(IBaseEventPayload payload)
        {
            if (payload is not BossEventPayload bossPayload)
            {
                return;
            }

            Debug.Log("OnHitStone :: 보스가 돌에 맞음");

            var target = bossPayload.TransformValue1.GetComponent<TerrapupaBehaviourController>().TerrapupaData;
            target.hitThrowStone.Value = true;
        }

        private void OnBossMeleeAttack(IBaseEventPayload bossPayload)
        {
            Debug.Log("OnMeleeAttack :: 보스의 근접 공격");

            var payload = bossPayload as BossEventPayload;

            if (payload == null)
            {
                return;
            }

            var hitEffect = payload.PrefabValue;
            var playerTransform = payload.TransformValue1;
            var manaTransform = payload.TransformValue2;
            var boss = payload.Sender;
            var attackValue = payload.IntValue;

            if (playerTransform != null)
            {
                HitedPlayer(boss, playerTransform, payload.CombatPayload);
                if (hitEffect != null)
                {
                    ParticleManager.Instance.GetParticle(hitEffect, playerTransform, 0.5f);
                    SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "TerrapupaRollHit", boss.position);
                }
            }

            if (manaTransform != null)
            {
                HitedManaFountaine(boss, manaTransform);
            }
        }

        private void OnBossLowAttack(IBaseEventPayload bossPayload)
        {
            Debug.Log("OnLowAttack");
            var payload = bossPayload as BossEventPayload;

            if (payload == null)
            {
                return;
            }

            var playerTransform = payload.TransformValue1;
            var manaTransform = payload.TransformValue2;
            var boss = payload.Sender;

            var jumpCheckValue = 1.0f;

            if (playerTransform != null)
            {
                // 플레이어 아래 광선을 쏴서 점프 체크
                RaycastHit hit;

                LayerMask groundLayer = 1 << LayerMask.NameToLayer("Ground");
                var isJumping = !Physics.Raycast(playerTransform.position + Vector3.up * 0.1f, -Vector3.up, out hit,
                    jumpCheckValue + 0.1f, groundLayer);

                Debug.Log($"Raycast distance: {hit.distance}");
                if (!isJumping)
                {
                    var bossController = boss.GetComponent<TerrapupaBehaviourController>();
                    HitedPlayer(boss, playerTransform, payload.CombatPayload);
                }
            }

            if (manaTransform != null)
            {
                HitedManaFountaine(payload.Sender, manaTransform);
            }
        }

        private void OnBossMinionAttack(IBaseEventPayload bossPayload)
        {
            Debug.Log("OnMinionAttack :: 보스 미니언의 공격");

            var payload = bossPayload as BossEventPayload;

            if (payload == null)
            {
                return;
            }

            var playerTransform = payload.TransformValue1;
            var minion = payload.Sender;

            Debug.Log(playerTransform);

            if (playerTransform != null)
            {
                HitedPlayer(minion, playerTransform, payload.CombatPayload);
            }

            StartCoroutine(MinionAttackCooldown(payload));
        }

        [Button]
        private void OnDestroyAllManaFountains(IBaseEventPayload payload)
        {
            Debug.Log("OnDestroyAllManaFountains :: 모든 보스 기절");

            switch (currentLevel)
            {
                case 1:
                    Debug.Log("테라푸파 기절");
                    terrapupa.Stun();
                    break;
                case 2:
                    Debug.Log("테라, 푸파 기절");
                    terra.Stun();
                    pupa.Stun();
                    break;
            }

            if (!isFirstDestroyAllManaFountain)
            {
                isFirstDestroyAllManaFountain = true;
                TerrapupaDialogChannel.SendMessage(TerrapupaDialogTriggerType.DestroyAllManaFountains, ticketMachine);
            }
        }

        private void OnApplySingleBossCooldown(IBaseEventPayload cooldownPayload)
        {
            Debug.Log("ApplyBossAttackCooldown :: 쿨타임 적용");
            var payload = cooldownPayload as BossEventPayload;

            var bossController = payload.Sender.GetComponent<TerrapupaBehaviourController>();
            var cooldown = payload.FloatValue;
            var banType = payload.AttackTypeValue;

            bossController.Cooldown(cooldown, banType);
        }

        private void OnStartIntakeMagicStone(IBaseEventPayload bossPayload)
        {
            var payload = bossPayload as BossEventPayload;

            var bossController = payload.Sender.GetComponent<TerrapupaBehaviourController>();
            var magicStone = payload.TransformValue1;

            // 지속시간 체크 정지
            magicStone.GetComponent<MagicStone>().StopCheckDuration();
            bossController.StartIntakeMagicStone();
        }

        #endregion

        #region 3. 코루틴 함수

        private IEnumerator MinionAttackCooldown(BossEventPayload payload)
        {
            var minionController = payload.Sender.GetComponent<TerrapupaMinionBehaviourController>();
            Debug.Log($"{minionController} 공격 봉인");
            minionController.MinionData.canAttack.Value = false;

            yield return new WaitForSeconds(payload.FloatValue);

            Debug.Log($"MinionAttackCooldown :: {minionController} 쿨다운 완료");
            minionController.MinionData.canAttack.Value = true;
        }

        private IEnumerator FallCheck()
        {
            while (true)
            {
                CheckFalling(terrapupa.transform);
                CheckFalling(terra.transform);
                CheckFalling(pupa.transform);
                foreach (var minion in minions)
                {
                    CheckFalling(minion.transform);
                }

                yield return new WaitForSeconds(fallCheckLatency);
            }
        }

        private IEnumerator NullCheck()
        {
            while (true)
            {
                CheckPlayerNull(terrapupa.transform);
                CheckPlayerNull(terra.transform);
                CheckPlayerNull(pupa.transform);
                foreach (var minion in minions)
                {
                    CheckPlayerNull(minion.transform);
                }

                yield return new WaitForSeconds(fallCheckLatency);
            }
        }

        #endregion

        #region 4. 기타 함수

        private void HitedPlayer(Transform attacker, Transform player, CombatPayload payload)
        {
            if (payload == null)
            {
                return;
            }

            Debug.Log($"플레이어 피해 {payload.Damage} 입음");
            payload.Attacker = attacker;
            payload.Defender = player;

            ticketMachine.SendMessage(ChannelType.Combat, payload);
        }

        private void HitedManaFountaine(Transform attacker, Transform manaTransform, GameObject hitEffect = null)
        {
            var manaFountain = manaTransform.GetComponent<ManaFountain>();

            if (manaFountain != null)
            {
                manaFountain.IsBroken = true;

                EventBus.Instance.Publish(EventBusEvents.DestroyedManaByBoss1,
                    new BossEventPayload
                    {
                        PrefabValue = hitEffect,
                        Sender = attacker,
                        TransformValue1 = manaTransform,
                        AttackTypeValue = manaFountain.banBossAttackType
                    });
            }
        }

        private void SpawnTerraAndPupa()
        {
            Debug.Log("SpawnTerraAndPupa :: 테라, 푸파 소환");

            var interval = 1.5f;
            var angle = 90.0f;

            terra.gameObject.SetActive(true);
            pupa.gameObject.SetActive(true);

            terra.StartBattle(player.transform);
            pupa.StartBattle(player.transform);

            terra.transform.position = terrapupa.transform.position + new Vector3(0.0f, 0.0f, -interval);
            pupa.transform.position = terrapupa.transform.position + new Vector3(0.0f, 0.0f, interval);
            terra.transform.rotation = Quaternion.Euler(terrapupa.transform.eulerAngles + new Vector3(0.0f, -angle, 0.0f));
            pupa.transform.rotation = Quaternion.Euler(terrapupa.transform.eulerAngles + new Vector3(0.0f, angle, 0.0f));
        }

        private void SpawnMinions(Vector3 position)
        {
            Debug.Log("SpawnMinions :: 테, 라, 푸, 파 랜덤 2마리 소환");

            for (var i = 0; i < 2; i++)
            {
                var minion = minions[currentMinionSpawnIndex];

                minion.gameObject.SetActive(true);
                minion.transform.position = position;
                minion.MinionData.player.Value = player.transform;

                minion.HealthBar.HideBillboard();

                // 미니언 인덱스 갱신 ( +1 )
                currentMinionSpawnIndex = (currentMinionSpawnIndex + 1) % minions.Count;
            }
        }
        
        private void CheckFalling(Transform target)
        {
            LayerMask groundMask = LayerMask.GetMask("Ground");
            var checkDistance = 30.0f;
            var rayStartOffset = 10.0f;

            RaycastHit hit;

            var rayStart = target.position + Vector3.up * rayStartOffset;

            var hitGround = Physics.Raycast(rayStart, -Vector3.up, out hit, checkDistance, groundMask);

            if (!hitGround)
            {
                Debug.Log("추락 방지, 포지션 초기화");
                target.position = transform.position;
            }
        }

        private void CheckPlayerNull(Transform targetBoss)
        {
            var instance = targetBoss.GetComponent<BehaviourTreeInstance>();
            var targetPlayer = instance.FindBlackboardKey<Transform>("player");

            if (targetPlayer.value != player.transform)
            {
                targetPlayer.Value = player.transform;
            }
        }
        
        #endregion
    }
}
