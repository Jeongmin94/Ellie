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
using Assets.Scripts.Item.Stone;
using System.Collections.Generic;

namespace Centers.Boss
{
    public class TerrapupaCenter : BaseCenter
    {
        [SerializeField] private StoneHatchery hatchery;
        [SerializeField] private TerrapupaController terrapupa;
        [SerializeField] private TerrapupaController terra;
        [SerializeField] private TerrapupaController pupa;
        [SerializeField] private List<TerrapupaMinionController> minions;
        [SerializeField] private PlayerController player;
        [SerializeField] private TerrapupaMapObjectController terrapupaMapObjects;

        public float fallCheckLatency = 5.0f;
        private int bossDeathCheck = 0;
        public bool isActiveTerrapupa = true;
        public bool isActiveTerra = false;
        public bool isActivePupa = false;
        public bool isActiveMinions = false;

        private void Awake()
        {
            base.Init();
            SubscribeEvents();
        }

        protected override void Start()
        {
            base.Start();

            CheckTickets();
            SetBossTarget();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("테라푸파 사망");
                terrapupa.terrapupaData.currentHP.Value = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("테라, 푸파 사망");
                terra.terrapupaData.currentHP.Value = 0;
                pupa.terrapupaData.currentHP.Value = 0;
            }
        }

        /// <summary>
        /// Init Boss Center
        /// </summary>

        private void SetBossTarget()
        {
            terrapupa.terrapupaData.player.Value = player.transform;
            terra.terrapupaData.player.Value = player.transform;
            pupa.terrapupaData.player.Value = player.transform;
            foreach (var minion in minions)
            {
                Debug.Log("확인");
                minion.minionData.player.Value = player.transform;
            }

            terrapupa.gameObject.SetActive(isActiveTerrapupa);
            terra.gameObject.SetActive(isActiveTerra);
            pupa.gameObject.SetActive(isActivePupa);
            foreach (var minion in minions)
            {
                minion.gameObject.SetActive(isActiveMinions);
            }
        }

        private void CheckTickets()
        {
            CheckTicket(hatchery.gameObject);
            CheckTicket(terrapupa.gameObject);
            CheckTicket(terra.gameObject);
            CheckTicket(pupa.gameObject);
            CheckTicket(terrapupaMapObjects.gameObject);
            foreach (var minion in minions)
            {
                CheckTicket(minion.gameObject);
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
            EventBus.Instance.Subscribe<IBaseEventPayload>(EventBusEvents.BossMeleeAttack, OnMeleeAttack);
            EventBus.Instance.Subscribe<IBaseEventPayload>(EventBusEvents.BossLowAttack, OnLowAttack);
            EventBus.Instance.Subscribe<IBaseEventPayload>(EventBusEvents.BossMinionAttack, OnMinionAttack);
        }

        /// <summary>
        /// Boss Event Handler
        /// </summary>

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
            stone.GetComponent<TerrapupaStone>().Init(actor.Stone.position, actor.transform.localScale, stonePayload.FloatValue, stonePayload.IntValue, stonePayload.Sender, actor.TicketMachine);
            stone.GetComponent<TerrapupaStone>().MoveToTarget(stonePayload.TransformValue1);

            actor.Stone.gameObject.SetActive(false);
        }

        private void OnBossApplyAttackCooldown(BossEventPayload payload)
        {
            Debug.Log("OnBossApplyAttackCooldown :: 쿨타임 적용");

            switch (payload.AttackTypeValue)
            {
                case TerrapupaAttackType.ThrowStone:
                    if (terrapupa.terrapupaData.stoneUsable) terrapupa.terrapupaData.canThrowStone.Value = payload.BoolValue;
                    if (terra.terrapupaData.stoneUsable) terra.terrapupaData.canThrowStone.Value = payload.BoolValue;
                    if (pupa.terrapupaData.stoneUsable) pupa.terrapupaData.canThrowStone.Value = payload.BoolValue;
                    break;
                case TerrapupaAttackType.EarthQuake:
                    if (terrapupa.terrapupaData.earthQuakeUsable) terrapupa.terrapupaData.canEarthQuake.Value = payload.BoolValue;
                    if (terra.terrapupaData.earthQuakeUsable) terra.terrapupaData.canEarthQuake.Value = payload.BoolValue;
                    if (pupa.terrapupaData.earthQuakeUsable) pupa.terrapupaData.canEarthQuake.Value = payload.BoolValue;
                    break;
                case TerrapupaAttackType.Roll:
                    if (terrapupa.terrapupaData.rollUsable) terrapupa.terrapupaData.canRoll.Value = payload.BoolValue;
                    if (terra.terrapupaData.rollUsable) terra.terrapupaData.canRoll.Value = payload.BoolValue;
                    if (pupa.terrapupaData.rollUsable) pupa.terrapupaData.canRoll.Value = payload.BoolValue;
                    break;
                case TerrapupaAttackType.LowAttack:
                    if (terrapupa.terrapupaData.lowAttackUsable) terrapupa.terrapupaData.canLowAttack.Value = payload.BoolValue;
                    if (terra.terrapupaData.lowAttackUsable) terra.terrapupaData.canLowAttack.Value = payload.BoolValue;
                    if (pupa.terrapupaData.lowAttackUsable) pupa.terrapupaData.canLowAttack.Value = payload.BoolValue;
                    break;
                default:
                    break;
            }
        }

        private void OnStartEarthQuake(IBaseEventPayload earthQuakePayload)
        {
            Debug.Log($"OnStartEarthQuake");
            BossEventPayload payload = earthQuakePayload as BossEventPayload;

            if(payload == null)
            {
                return;
            }

            Transform playerTransform = payload.TransformValue1;
            Transform manaTransform = payload.TransformValue2;
            Transform bossTransform = payload.TransformValue3;
            Transform boss = payload.Sender;
            int attack = payload.IntValue;

            Debug.Log(playerTransform);
            Debug.Log(manaTransform);
            Debug.Log(bossTransform);

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
                    Debug.Log($"플레이어 피해 {attack} 입음");
                    
                    TerrapupaController bossController = boss.GetComponent<TerrapupaController>();
                    bossController.TicketMachine.SendMessage(ChannelType.Combat, new CombatPayload
                    {
                        Attacker = boss,
                        Defender = playerTransform,
                        Damage = payload.IntValue,
                        PlayerStatusEffectName = StatusEffectName.KnockedAirborne,
                        statusEffectduration = 1.0f,
                        force = 15.0f,
                    });
                }
            }
            if (manaTransform != null)
            {
                // 해당 마나의 샘 쿨타임 적용, 삭제
                ManaFountain manaFountain = manaTransform.GetComponent<ManaFountain>();
                manaFountain.IsBroken = true;

                EventBus.Instance.Publish(EventBusEvents.DestroyedManaByBoss1,
                    new BossEventPayload
                    {
                        Sender = payload.Sender,
                        TransformValue1 = manaTransform,
                        AttackTypeValue = manaFountain.banBossAttackType,
                    });
            }
            if(bossTransform != null)
            {
                // 보스 체크
                TerrapupaRootData target = bossTransform.GetComponent<TerrapupaController>().terrapupaData;
                target.hitEarthQuake.Value = true;
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

            //magicStone = null;
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
                    bossDeathCheck++;
                    Debug.Log($"미니언 소환, 남은 미니언 {4 - bossDeathCheck}");
                }

                payload.Sender.gameObject.SetActive(false);
            }

            if(bossDeathCheck == 4)
            {
                Debug.Log("보스 클리어");
            }
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

            for(int i = 0; i < 2; i++)
            {
                int index = Random.Range(0, minions.Count);
                TerrapupaMinionController minion = minions[index];

                minion.gameObject.SetActive(true);

                minion.transform.position = position;
                minion.minionData.player.Value = player.transform;

                minions.RemoveAt(index);
            }
        }

        private void OnHitStone(BossEventPayload bossPayload)
        {
            Debug.Log($"OnHitStone :: 보스가 돌에 맞음");

            TerrapupaRootData target = bossPayload.TransformValue1.GetComponent<TerrapupaController>().terrapupaData;
            target.hitThrowStone.Value = true;
        }

        private void OnMeleeAttack(IBaseEventPayload bossPayload)
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
            int attack = payload.IntValue;

            Debug.Log(playerTransform);
            Debug.Log(manaTransform);

            if (playerTransform != null)
            {
                Debug.Log($"플레이어 피해 {attack} 입음");

                TerrapupaController bossController = boss.GetComponent<TerrapupaController>();
                bossController.TicketMachine.SendMessage(ChannelType.Combat, new CombatPayload
                {
                    Attacker = boss,
                    Defender = playerTransform,
                    Damage = payload.IntValue,
                    PlayerStatusEffectName = StatusEffectName.WeakRigidity,
                    statusEffectduration = 0.05f,
                });
            }
            if (manaTransform != null)
            {
                // 해당 마나의 샘 쿨타임 적용, 삭제
                ManaFountain manaFountain = manaTransform.GetComponent<ManaFountain>();
                manaFountain.IsBroken = true;

                EventBus.Instance.Publish(EventBusEvents.DestroyedManaByBoss1,
                    new BossEventPayload
                    {
                        Sender = payload.Sender,
                        TransformValue1 = manaTransform,
                        AttackTypeValue = manaFountain.banBossAttackType,
                    });
            }
        }

        private void OnLowAttack(IBaseEventPayload bossPayload)
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
            int attack = payload.IntValue;

            Debug.Log(playerTransform);
            Debug.Log(manaTransform);

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
                    Debug.Log($"플레이어 피해 {attack} 입음");

                    TerrapupaController bossController = boss.GetComponent<TerrapupaController>();
                    bossController.TicketMachine.SendMessage(ChannelType.Combat, new CombatPayload
                    {
                        Attacker = boss,
                        Defender = playerTransform,
                        Damage = payload.IntValue,
                        PlayerStatusEffectName = StatusEffectName.Down,
                        statusEffectduration = 0.5f,
                        force = 10.0f,
                    });
                }
            }
            if (manaTransform != null)
            {
                // 해당 마나의 샘 쿨타임 적용, 삭제
                ManaFountain manaFountain = manaTransform.GetComponent<ManaFountain>();
                manaFountain.IsBroken = true;

                EventBus.Instance.Publish(EventBusEvents.DestroyedManaByBoss1,
                    new BossEventPayload
                    {
                        Sender = payload.Sender,
                        TransformValue1 = manaTransform,
                        AttackTypeValue = manaFountain.banBossAttackType,
                    });
            }
        }

        private void OnMinionAttack(IBaseEventPayload bossPayload)
        {
            Debug.Log($"OnMinionAttack :: 보스 미니언의 공격");

            BossEventPayload payload = bossPayload as BossEventPayload;

            if (payload == null)
            {
                return;
            }

            Transform playerTransform = payload.TransformValue1;
            Transform minion = payload.Sender;
            int attack = payload.IntValue;

            Debug.Log(playerTransform);

            if (playerTransform != null)
            {
                Debug.Log($"플레이어 피해 {attack} 입음");

                TerrapupaMinionController minionController = minion.GetComponent<TerrapupaMinionController>();
                minionController.TicketMachine.SendMessage(ChannelType.Combat, new CombatPayload
                {
                    Attacker = minion,
                    Defender = playerTransform,
                    Damage = payload.IntValue,
                    PlayerStatusEffectName = StatusEffectName.WeakRigidity,
                    statusEffectduration = 0.05f,
                });

            }

            StartCoroutine(MinionAttackCooldown(payload));
        }

        private IEnumerator MinionAttackCooldown(BossEventPayload payload)
        {
            TerrapupaMinionController minionController = payload.Sender.GetComponent<TerrapupaMinionController>();
            Debug.Log($"{minionController} 공격 봉인");
            minionController.minionData.canAttack.Value = false;

            yield return new WaitForSeconds(5.0f);

            Debug.Log($"MinionAttackCooldown :: {minionController} 쿨다운 완료");
            minionController.minionData.canAttack.Value = true;
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