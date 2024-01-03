using System.Collections;
using System.Collections.Generic;
using Boss1.Terrapupa;
using Boss1.TerrapupaMinion;
using Channels.Boss;
using Channels.Components;
using Channels.Stone;
using Channels.Type;
using Managers.Event;
using Player;
using Sirenix.OdinInspector;
using TheKiwiCoder;
using UnityEngine;
using Utils;

namespace Controller.Boss1
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

        //
        private const int GOLEM_CORE_INDEX = 4021;
        private bool isFirstDestroyAllManaFountain;
        private int golemCoreCount = 0;
        //
        
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
            EventBus.Instance.Subscribe(EventBusEvents.BossDeath, OnBossDeath);
            EventBus.Instance.Subscribe(EventBusEvents.ApplyBossCooldown, OnBossApplyAttackCooldown);
            EventBus.Instance.Subscribe(EventBusEvents.DestroyAllManaFountain, OnDestroyAllManaFountains);
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
    }
}
