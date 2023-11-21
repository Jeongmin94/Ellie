using Assets.Scripts.Particle;
using Assets.Scripts.Utils;
using Boss.Objects;
using Boss.Terrapupa;
using Channels.Boss;
using Channels.Components;
using Channels.Type;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrapupaMapObjectController : MonoBehaviour
{
    [SerializeField] private GameObject magicStalactitePrefab;
    [SerializeField] private List<ManaFountain> manaFountains;
    [SerializeField] private List<List<MagicStalactite>> stalactites = new List<List<MagicStalactite>>();

    [Header("종마석")]
    [Tooltip("재생성 쿨타임")] public float regenerateStalactiteTime = 10.0f;
    [Tooltip("구역 갯수")] public int numberOfSector = 3;
    [Tooltip("구역 당 종마석 갯수")] public int stalactitePerSector = 3;
    [Tooltip("생성 구역 반지름")] public float fieldRadius = 25.0f;
    [Tooltip("생성 높이")] public float fieldHeight = 8.0f;

    [Header("마나의 샘")]
    [Tooltip("재생성 쿨타임")] public float respawnManaFountainTime = 10.0f;
    [Tooltip("마법 돌맹이 재생성 쿨타임")] public float regenerateManaStoneTime = 10.0f;

    private TicketMachine ticketMachine;

    private int manaFountainCount;

    private void Awake()
    {
        SubscribeEvents();
        SpawnStalactites();
        InitManaFountains();
        InitTicketMachine();

        manaFountainCount = manaFountains.Count;
    }

    /// <summary>
    /// Init BossRoom Objects
    /// </summary>

    private void SubscribeEvents()
    {
        EventBus.Instance.Subscribe<BossEventPayload>(EventBusEvents.HitManaByPlayerStone, OnHitMana);
        EventBus.Instance.Subscribe<BossEventPayload>(EventBusEvents.DestroyedManaByBoss1, OnDestroyedMana);
        EventBus.Instance.Subscribe<BossEventPayload>(EventBusEvents.DropMagicStalactite, OnDropMagicStalactite);
    }

    private void InitTicketMachine()
    {
        ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
        ticketMachine.AddTickets(ChannelType.Stone);

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
                GameObject stalactite = Instantiate(magicStalactitePrefab, position, Quaternion.identity, transform);
                MagicStalactite instantStalactite = stalactite.GetComponent<MagicStalactite>();
                instantStalactite.MyIndex = i;
                sectorList.Add(instantStalactite);
                instantStalactite.respawnValue = regenerateStalactiteTime;
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

    public void InitManaFountains()
    {
        foreach (var mana in manaFountains)
        {
            mana.coolDownValue = regenerateManaStoneTime;
            mana.respawnValue = respawnManaFountainTime;
        }
    }

    /// <summary>
    /// BossRoom Objects Event Handler
    /// </summary>

    private void OnHitMana(BossEventPayload manaPayload)
    {
        Debug.Log("OnHitMana :: 마나의 샘 쿨타임 적용");

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
        manaPayload.BoolValue = false;

        GameObject hitEffect = manaPayload.PrefabValue;
        Transform manaTransform = manaPayload.TransformValue1;

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

        if(hitEffect != null)
        {
            ParticleManager.Instance.GetParticle(hitEffect, new ParticlePayload
            {
                Position = manaTransform.position,
                Rotation = manaTransform.rotation,
                Scale = new Vector3(0.7f, 0.7f, 0.7f),
                Offset = new Vector3(0.0f, 1.0f, 0.0f),
            });
        }

        EventBus.Instance.Publish(EventBusEvents.ApplyBossCooldown, manaPayload);

        StartCoroutine(ManaRespawn(manaPayload));
    }

    private IEnumerator ManaRespawn(BossEventPayload manaPayload)
    {
        manaFountainCount--;
        ManaFountain mana = manaPayload.TransformValue1.GetComponent<ManaFountain>();
        mana.IsBroken = true;
        mana.gameObject.SetActive(false);

        if(manaFountainCount == 0)
        {
            EventBus.Instance.Publish(EventBusEvents.DestroyAllManaFountain);
        }

        yield return new WaitForSeconds(mana.respawnValue);

        Debug.Log($"{mana.name} 리스폰 완료");
        manaPayload.BoolValue = true;

        manaFountainCount++;
        mana.gameObject.SetActive(true);
        mana.IsBroken = false;
        mana.IsCooldown = false;

        EventBus.Instance.Publish(EventBusEvents.ApplyBossCooldown, manaPayload);
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
}
