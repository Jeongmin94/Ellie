using Assets.Scripts.Channels.Item;
using Assets.Scripts.Item.Stone;
using Assets.Scripts.Particle;
using Assets.Scripts.Utils;
using Boss.Objects;
using Boss.Terrapupa;
using Channels.Boss;
using Channels.Components;
using Channels.Type;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Controller;
using UnityEngine;

public class TerrapupaMapObjectController : BaseController
{
    [Title("테라푸파 보스전 오브젝트 객체")]
    [SerializeField] private GameObject magicStalactitePrefab;
    [SerializeField] private List<ManaFountain> manaFountains;
    [SerializeField] private List<List<MagicStalactite>> stalactites = new List<List<MagicStalactite>>();

    [Title("상태 체크")]
    [SerializeField][ReadOnly] private int manaFountainCount = 4;

    [Title("종마석")]
    [InfoBox("보스가 종마석 맞고 섭취중 아니여도 기절하는지의 여부\n true면 섭취중 아니여도 기절")]
    public bool canBossStun = false;

    [InfoBox("재생성 쿨타임")] public float regenerateStalactiteTime = 10.0f;
    [InfoBox("구역 갯수")] public int numberOfSector = 3;
    [InfoBox("구역 당 종마석 갯수")] public int stalactitePerSector = 3;
    [InfoBox("생성 구역 반지름")] public float fieldRadius = 25.0f;
    [InfoBox("생성 높이")] public float fieldHeight = 8.0f;

    [Title("마나의 샘")][InfoBox("재생성 쿨타임")] public float respawnManaFountainTime = 10.0f;
    [InfoBox("마법 돌맹이 재생성 쿨타임")] public float regenerateManaStoneTime = 10.0f;

    private bool isFirstBrokenManaFountain = false;
    private bool isFirstHitManaFountain = false;

    private TicketMachine ticketMachine;

    public override void InitController()
    {
        Debug.Log($"{name} InitController");

        SubscribeEvents();
        SpawnStalactites();
        InitManaFountains();
        InitTicketMachine();
    }

    #region 1. 초기화 함수

    private void InitTicketMachine()
    {
        ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
        ticketMachine.AddTickets(ChannelType.Stone, ChannelType.Dialog, ChannelType.BossBattle, ChannelType.BossDialog);
        ticketMachine.RegisterObserver(ChannelType.BossBattle, OnNotifyBossBattle);

        foreach (var mana in manaFountains)
        {
            mana.InitTicketMachine(ticketMachine);
        }

        foreach (var sector in stalactites)
        {
            foreach (var stalactite in sector)
            {
                stalactite.InitTicketMachine(ticketMachine);
            }
        }
    }

    private void SpawnStalactites()
    {
        for (int i = 0; i < numberOfSector; i++)
        {
            List<MagicStalactite> sectorList = new List<MagicStalactite>();
            for (int j = 0; j < stalactitePerSector; j++)
            {
                Vector3 position = GenerateRandomPositionInSector(i);
                GameObject stalactite = Instantiate(magicStalactitePrefab, Vector3.zero, Quaternion.identity, transform);
                stalactite.transform.localPosition = position;
                MagicStalactite instantStalactite = stalactite.GetComponent<MagicStalactite>();
                instantStalactite.SetLineRendererPosition();
                instantStalactite.MyIndex = i;
                sectorList.Add(instantStalactite);
                instantStalactite.respawnValue = regenerateStalactiteTime;
            }

            stalactites.Add(sectorList);
        }
    }

    public void InitManaFountains()
    {
        foreach (var mana in manaFountains)
        {
            mana.coolDownValue = regenerateManaStoneTime;
            mana.respawnValue = respawnManaFountainTime;
        }
        manaFountainCount = manaFountains.Count;
    }

    private void SubscribeEvents()
    {
        EventBus.Instance.Subscribe(EventBusEvents.HitManaByPlayerStone, OnHitMana);
        EventBus.Instance.Subscribe(EventBusEvents.DestroyedManaByBoss1, OnDestroyedMana);
        EventBus.Instance.Subscribe(EventBusEvents.DropMagicStalactite, OnDropMagicStalactite);
        EventBus.Instance.Subscribe(EventBusEvents.ActivateMagicStone, OnActivateMagicStone);
    }
    #endregion

    #region 2. 이벤트 핸들러
    private void OnNotifyBossBattle(IBaseEventPayload payload)
    {
        if (payload is not BossBattlePayload bPayload)
            return;

        switch (bPayload.SituationType)
        {
            case BossSituationType.EnterBossRoom:
                break;
            default:
                break;
        }
    }

    private void OnHitMana(IBaseEventPayload payload)
    {
        if (payload is not BossEventPayload manaPayload)
            return;

        Debug.Log("OnHitMana :: 마나의 샘 쿨타임 적용");

        var mana = manaPayload.TransformValue1.GetComponent<ManaFountain>();
        DropStoneItem(mana.SpawnPosition, mana.MAGICSTONE_INDEX);

        if(!isFirstHitManaFountain)
        {
            isFirstHitManaFountain = true;
            BossDialogChannel.SendMessageBossDialog(BossDialogTriggerType.GetMagicStoneFirstTime, ticketMachine);
        }

        StartCoroutine(ManaCooldown(manaPayload));
    }

    private void OnDestroyedMana(IBaseEventPayload payload)
    {
        if (payload is not BossEventPayload manaPayload)
            return;

        Debug.Log($"OnDestroyedMana :: {manaPayload.AttackTypeValue} 공격 타입 봉인");

        // 보스의 돌에 맞았을 경우, 돌 삭제
        if (manaPayload.TransformValue2 != null)
        {
            Destroy(manaPayload.TransformValue2.gameObject);
        }

        // 공격한 보스 정보 갱신
        var manaTransform = manaPayload.TransformValue1;
        var mana = manaTransform.GetComponent<ManaFountain>();
        var actor = manaPayload.Sender.GetComponent<TerrapupaBTController>();
        if (actor == null)
        {
            actor = manaPayload.Sender.GetComponent<TerrapupaStone>().Owner.GetComponent<TerrapupaBTController>();
            manaPayload.Sender = actor.transform;
        }

        // 돌맹이 3개 생성
        for (int i = 0; i < 3; i++)
        {
            DropStoneItem(mana.SpawnPosition, mana.NORMALSTONE_INDEX);
        }

        // 히트 이펙트 생성
        GameObject hitEffect = manaPayload.PrefabValue;
        if (hitEffect != null)
        {
            ParticleManager.Instance.GetParticle(hitEffect, new ParticlePayload
            {
                Position = manaTransform.position,
                Rotation = manaTransform.rotation,
                Scale = new Vector3(0.7f, 0.7f, 0.7f),
                Offset = new Vector3(0.0f, 1.0f, 0.0f),
            });
        }

        // 보스의 개별 공격 쿨타임 적용 멈추고, 파괴 쿨타임으로 새로 적용시킴
        TerrapupaAttackType type = mana.banBossAttackType;
        if (actor.AttackCooldown.ContainsKey(type) && actor.AttackCooldown[type] != null)
        {
            Debug.Log($"{actor}의 쿨타임 중복 적용");
            actor.StopCoroutine(actor.AttackCooldown[type]);
        }

        // 돌맹이를 날린 보스 공격 쿨타임 적용
        manaPayload.BoolValue = false;
        EventBus.Instance.Publish(EventBusEvents.ApplyBossCooldown, manaPayload);

        // 최초 마나의 샘 파괴 여부 다이얼로그 전송
        if (!isFirstBrokenManaFountain)
        {
            isFirstBrokenManaFountain = true;
            BossDialogChannel.SendMessageBossDialog(BossDialogTriggerType.DestroyManaFountainFirstTime, ticketMachine);
        }

        // 마나의 샘 재생성 쿨타임
        StartCoroutine(ManaRespawn(manaPayload));
    }

    private void OnDropMagicStalactite(IBaseEventPayload payload)
    {
        if (payload is not BossEventPayload stalactitePayload)
            return;

        Debug.Log($"OnDropMagicStalactite :: 종마석 드랍");

        Transform boss = stalactitePayload.TransformValue2;
        if (boss != null)
        {
            var actor = boss.GetComponent<TerrapupaBTController>();
            if (stalactitePayload.TransformValue2 != null)
            {
                Debug.Log("보스 타격");

                if (canBossStun == true || (canBossStun == false && actor.terrapupaData.isIntake.Value))
                {
                    Debug.Log("기절");
                    actor.Stun();
                }
            }
        }

        StartCoroutine(RespawnMagicStalactite(stalactitePayload));
    }

    private void OnActivateMagicStone(IBaseEventPayload basePayload)
    {
        Debug.Log($"OnActivateMagicStone :: 마법 돌맹이 개수 1개 제한");
        BossEventPayload payload = basePayload as BossEventPayload;

        var magicStone = payload.Sender.GetComponent<MagicStone>();

        if (!MagicStone.isActivateRange)
        {
            magicStone.ActivateRange();
        }
    }

    #endregion

    #region 3. 코루틴 함수

    private IEnumerator ManaCooldown(BossEventPayload manaPayload)
    {
        ManaFountain mana = manaPayload.TransformValue1.GetComponent<ManaFountain>();
        if (mana.isActiveAndEnabled)
        {
            mana.SetLightIntensity(0.0f, mana.changeLightTime);
        }
        mana.IsCooldown = true;

        yield return new WaitForSeconds(mana.coolDownValue);

        Debug.Log($"{mana.name} 쿨타임 완료");
        if (mana.isActiveAndEnabled)
        {
            mana.SetLightIntensity(mana.lightIntensity, mana.changeLightTime);
        }
        mana.IsCooldown = false;
    }

    private IEnumerator RespawnMagicStalactite(BossEventPayload payload)
    {
        float respawnTime = payload.FloatValue;
        Debug.Log($"{respawnTime}초 이후 재생성");

        yield return new WaitForSeconds(respawnTime);

        Vector3 position = GenerateRandomPositionInSector(payload.IntValue);
        payload.TransformValue1.localPosition = position;
        payload.TransformValue1.gameObject.SetActive(true);
    }

    private IEnumerator ManaRespawn(BossEventPayload manaPayload)
    {
        ManaFountain mana = manaPayload.TransformValue1.GetComponent<ManaFountain>();

        manaFountainCount--;
        mana.DestroyManaFountain();
        mana.gameObject.SetActive(false);

        if (manaFountainCount == 0)
        {
            EventBus.Instance.Publish(EventBusEvents.DestroyAllManaFountain, new BossEventPayload());
        }

        yield return new WaitForSeconds(mana.respawnValue);

        Debug.Log($"{mana.name} 리스폰 완료");

        manaFountainCount++;
        mana.gameObject.SetActive(true);
        mana.RegenerateManaFountain();

        manaPayload.BoolValue = true;
        EventBus.Instance.Publish(EventBusEvents.ApplyBossCooldown, manaPayload);
    }

    #endregion

    #region 4. 기타 함수

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

    private Vector3 GetRandVector()
    {
        Vector3 vec = new(Random.Range(-1.0f, 1.0f), 0.5f, 0);
        return vec.normalized;
    }
    #endregion
}
