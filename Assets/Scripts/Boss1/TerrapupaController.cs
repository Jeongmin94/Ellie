﻿using Boss.Objects;
using Boss.Terrapupa;
using Assets.Scripts.Player;
using Channels.Boss;
using System.Collections;
using UnityEngine;
using Assets.Scripts.Managers;
using Channels.Type;
using Channels.Combat;
using System.Collections.Generic;
using Assets.Scripts.Particle;
using Channels.Components;
using Assets.Scripts.Item.Stone;
using Sirenix.OdinInspector;
using Assets.Scripts.Utils;
using Assets.Scripts.Channels.Item;
using System;

public class TerrapupaController : SerializedMonoBehaviour
{
    [Title("테라푸파 보스 객체")]
    [SerializeField] private TerrapupaBTController terrapupa;
    [SerializeField] private TerrapupaBTController terra;
    [SerializeField] private TerrapupaBTController pupa;
    [SerializeField] private List<TerrapupaMinionBTController> minions;
    [SerializeField] private PlayerController player;

    [Title("현재 페이즈 상태 체크용")]
    [ReadOnly][SerializeField] private int currentLevel = 1;        // 1페이즈, 2페이즈 체크용
    [ReadOnly][SerializeField] private int minionDeathCheck = 4;    // 3페이즈 미니언 4마리 체크
    [ReadOnly][SerializeField] private int currentMinionSpawnIndex = 0;
    [ReadOnly][SerializeField] private float fallCheckLatency = 5.0f;

    [Title("보스몬스터 생성 여부")]
    [InfoBox("박스 체크 시 해당 몬스터가 활성화 됩니다")]
    public bool isActiveTerrapupa = true;
    public bool isActiveTerra = false;
    public bool isActivePupa = false;
    public bool isActiveMinions = false;

    private TicketMachine ticketMachine;

    private const int GOLEM_CORE_INDEX = 4021;

    #region 0. 치트키
    [Title("치트키")]
    [Button("1페이즈 스킵", ButtonSizes.Large)]
    public void KillTerrapupa()
    {
        Debug.Log("테라푸파 사망 치트");
        Vector3 pos = terrapupa.transform.position;

        terrapupa.transform.position = new Vector3(pos.x, -1.0f, pos.z);
        terrapupa.terrapupaData.currentHP.Value = 0;
    }
    [Button("2페이즈 스킵", ButtonSizes.Large)]
    public void KillTerraAndPupa()
    {
        Debug.Log("테라, 푸파 사망 치트");
        Vector3 pos1 = terra.transform.position;
        Vector3 pos2 = pupa.transform.position;

        terra.transform.position = new Vector3(pos1.x, -1.0f, pos1.z);
        pupa.transform.position = new Vector3(pos2.x, -1.0f, pos2.z);
        terra.terrapupaData.currentHP.Value = 0;
        pupa.terrapupaData.currentHP.Value = 0;
    }
    [Button("3페이즈 스킵", ButtonSizes.Large)]
    public void KillMinions()
    {
        Debug.Log("테, 라, 푸, 파 사망 치트");

        foreach (var minion in minions)
        {
            minion.minionData.currentHP.Value = 0;
        }
    }
    #endregion

    private void Awake()
    {
        SubscribeEvents();
        InitTicketMachine();
    }
    private void Start()
    {
        ShuffleMinions();
        SetBossInfo();
        StartCoroutine(FallCheck());
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
    public void InitTicketMachine()
    {
        ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
        ticketMachine.AddTickets(ChannelType.Stone, ChannelType.Combat);

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
        EventBus.Instance.Subscribe<IBaseEventPayload>(EventBusEvents.StartIntakeMagicStone, OnStartIntakeMagicStone);
    }
    private void ShuffleMinions()
    {
        // 미니언 리스트 랜덤 셔플
        for (int i = 0; i < minions.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, minions.Count);
            TerrapupaMinionBTController temp = minions[i];
            minions[i] = minions[randomIndex];
            minions[randomIndex] = temp;
        }
    }
    #endregion

    #region 2. 이벤트 핸들러
    private void OnSpawnStone(IBaseEventPayload payload)
    {
        Debug.Log("OnSpawnStone :: 보스의 돌맹이 줍기");
        BossEventPayload stonePayload = payload as BossEventPayload;
        TerrapupaBTController actor = stonePayload.Sender.GetComponent<TerrapupaBTController>();

        actor.Stone.gameObject.SetActive(true);
    }
    private void OnThrowStone(IBaseEventPayload payload)
    {
        Debug.Log("OnThrowStone :: 보스의 돌맹이 던지기");

        BossEventPayload stonePayload = payload as BossEventPayload;
        TerrapupaBTController actor = stonePayload.Sender.GetComponent<TerrapupaBTController>();

        Poolable stone = PoolManager.Instance.Pop(actor.Stone.gameObject, transform);
        stone.GetComponent<TerrapupaStone>().Init(actor.Stone.position, actor.transform.localScale, stonePayload.FloatValue, stonePayload.CombatPayload, stonePayload.PrefabValue, stonePayload.Sender, ticketMachine);
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

        if (payload == null)
        {
            return;
        }

        GameObject hitEffect = payload.PrefabValue;
        Transform playerTransform = payload.TransformValue1;
        Transform manaTransform = payload.TransformValue2;
        Transform hitBossTransform = payload.TransformValue3;
        Transform boss = payload.Sender;

        // 1. 플레이어 피격
        if (playerTransform != null)
        {
            // 플레이어 아래 광선을 쏴서 점프 체크
            RaycastHit hit;
            float jumpCheckValue = 1.0f;

            LayerMask groundLayer = 1 << LayerMask.NameToLayer("Ground");
            bool isJumping = !Physics.Raycast(playerTransform.position + Vector3.up * 0.1f, -Vector3.up, out hit, jumpCheckValue + 0.1f, groundLayer);

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
            TerrapupaBTController hitBossControllers = hitBossTransform.GetComponent<TerrapupaBTController>();
            if (hitBossControllers?.terrapupaData != null)
            {
                hitBossControllers.terrapupaData.hitEarthQuake.Value = true;
                ParticleManager.Instance.GetParticle(hitEffect, hitBossTransform, 1.0f);
            }
        }
    }
    private void OnBossAtrractedByMagicStone(BossEventPayload magicStonePayload)
    {
        Debug.Log($"OnBossAtrractedByMagicStone :: 보스 마법 돌맹이를 추적 시작");

        Transform magicStone = magicStonePayload.TransformValue1;
        Transform target = magicStonePayload.TransformValue2;
        if (target)
        {
            TerrapupaBTController actor = target.GetComponent<TerrapupaBTController>();
            actor.terrapupaData.isTempted.Value = true;
            actor.terrapupaData.isIntake.Value = false;
            actor.terrapupaData.magicStoneTransform.Value = magicStone;
        }
    }
    private void OnBossUnattractedByMagicStone(BossEventPayload magicStonePayload)
    {
        Debug.Log($"OnBossUnattractedByMagicStone :: 보스 마법 돌맹이를 추적 종료");

        Transform target = magicStonePayload.TransformValue2;
        if (target)
        {
            TerrapupaBTController actor = target.GetComponent<TerrapupaBTController>();
            actor.terrapupaData.isTempted.Value = false;
            actor.terrapupaData.isIntake.Value = false;
            actor.terrapupaData.magicStoneTransform.Value = null;
        }
    }
    private void OnIntakeMagicStoneByBoss1(IBaseEventPayload bossPayload)
    {
        Debug.Log($"OnIntakeMagicStoneByBoss1 :: 보스가 마법 돌맹이를 섭취함");

        BossEventPayload payload = bossPayload as BossEventPayload;

        Transform _magicStone = payload.TransformValue1;
        int healValue = payload.IntValue;

        TerrapupaBTController actor = payload.Sender.GetComponent<TerrapupaBTController>();
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
        if (payload.Sender != null)
        {
            string bossName = payload.Sender.name;

            if (bossName == "Terrapupa")
            {
                Debug.Log("테라푸파 사망, 2페이즈");
                SpawnTerraAndPupa();
            }
            else if (bossName == "Terra" || bossName == "Pupa")
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

        if (minionDeathCheck == 2 || minionDeathCheck == 0)
        {
            Debug.Log("골렘의 눈 드랍");
            Vector3 position = payload.Sender.position;

            DropStoneItem(position, GOLEM_CORE_INDEX);
        }
    }
    private void OnHitStone(BossEventPayload bossPayload)
    {
        Debug.Log($"OnHitStone :: 보스가 돌에 맞음");

        TerrapupaRootData target = bossPayload.TransformValue1.GetComponent<TerrapupaBTController>().terrapupaData;
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
            HitedPlayer(boss, playerTransform, payload.CombatPayload);
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
                TerrapupaBTController bossController = boss.GetComponent<TerrapupaBTController>();
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
        Debug.Log($"OnMinionAttack :: 보스 미니언의 공격");

        BossEventPayload payload = bossPayload as BossEventPayload;

        if (payload == null)
        {
            return;
        }

        Transform playerTransform = payload.TransformValue1;
        Transform minion = payload.Sender;

        Debug.Log(playerTransform);

        if (playerTransform != null)
        {
            HitedPlayer(minion, playerTransform, payload.CombatPayload);
        }

        StartCoroutine(MinionAttackCooldown(payload));
    }
    private void OnDestroyAllManaFountains()
    {
        Debug.Log($"OnDestroyAllManaFountains :: 모든 보스 기절");

        switch (currentLevel)
        {
            case 1:
                Debug.Log("테라푸파 기절");
                StunBoss(terrapupa);
                break;
            case 2:
                Debug.Log("테라, 푸파 기절");
                StunBoss(terra);
                StunBoss(pupa);
                break;
        }
    }
    private void OnApplySingleBossCooldown(IBaseEventPayload cooldownPayload)
    {
        Debug.Log("ApplyBossAttackCooldown :: 쿨타임 적용");
        BossEventPayload payload = cooldownPayload as BossEventPayload;

        TerrapupaBTController bossController = payload.Sender.GetComponent<TerrapupaBTController>();
        float cooldown = payload.FloatValue;
        TerrapupaAttackType banType = payload.AttackTypeValue;

        bossController.Cooldown(cooldown, banType);
    }
    private void OnStartIntakeMagicStone(IBaseEventPayload bossPayload)
    {
        BossEventPayload payload = bossPayload as BossEventPayload;

        Transform magicStone = payload.TransformValue1;

        // 지속시간 체크 정지
        magicStone.GetComponent<MagicStone>().StopCheckDuration();
    }
    #endregion

    #region 3. 코루틴 함수
    private IEnumerator MinionAttackCooldown(BossEventPayload payload)
    {
        TerrapupaMinionBTController minionController = payload.Sender.GetComponent<TerrapupaMinionBTController>();
        Debug.Log($"{minionController} 공격 봉인");
        minionController.minionData.canAttack.Value = false;

        yield return new WaitForSeconds(payload.FloatValue);

        Debug.Log($"MinionAttackCooldown :: {minionController} 쿨다운 완료");
        minionController.minionData.canAttack.Value = true;
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
    #endregion

    #region 4. 기타 함수
    private void HitedPlayer(Transform attacker, Transform player, CombatPayload payload)
    {
        if (payload == null)
            return;

        Debug.Log($"플레이어 피해 {payload.Damage} 입음");
        payload.Attacker = attacker;
        payload.Defender = player;

        ticketMachine.SendMessage(ChannelType.Combat, payload);
    }
    private void HitedManaFountaine(Transform attacker, Transform manaTransform, GameObject hitEffect = null)
    {
        ManaFountain manaFountain = manaTransform.GetComponent<ManaFountain>();

        if(manaFountain != null)
        {
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
    }
    private void StunBoss(TerrapupaBTController target)
    {
        Debug.Log($"{target.name} 보스 기절");

        target.terrapupaData.isStuned.Value = true;
        target.terrapupaData.isTempted.Value = false;
        target.terrapupaData.isIntake.Value = false;
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
            TerrapupaMinionBTController minion = minions[currentMinionSpawnIndex];

            minion.gameObject.SetActive(true);
            minion.transform.position = position;
            minion.minionData.player.Value = player.transform;

            // 미니언 인덱스 갱신 ( +1 )
            currentMinionSpawnIndex = (currentMinionSpawnIndex + 1) % minions.Count;
        }
    }
    public void DropStoneItem(Vector3 position, int index)
    {
        ticketMachine.SendMessage(ChannelType.Stone, new StoneEventPayload
        {
            Type = StoneEventType.MineStone,
            StoneSpawnPos = position,
            StoneForce = GetRandVector(),
            StoneIdx = index,
        });
    }
    private void CheckFalling(Transform target)
    {
        LayerMask groundMask = LayerMask.GetMask("Ground");
        float checkDistance = 30.0f;
        float rayStartOffset = 10.0f;

        RaycastHit hit;

        Vector3 rayStart = transform.position + Vector3.up * rayStartOffset;

        bool hitGround = Physics.Raycast(rayStart, -Vector3.up, out hit, checkDistance, groundMask);

        if (!hitGround)
        {
            Debug.Log("추락 방지, 포지션 초기화");
            target.position = transform.position;
        }
    }
    private Vector3 GetRandVector()
    {
        Vector3 vec = new(UnityEngine.Random.Range(-1.0f, 1.0f), 0.5f, 0);
        return vec.normalized;
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