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
using UnityEngine;

public class TerrapupaMapObjectController : SerializedMonoBehaviour
{
    [Title("�׶�Ǫ�� ������ ������Ʈ ��ü")]
    [SerializeField] private GameObject magicStalactitePrefab;
    [SerializeField] private List<ManaFountain> manaFountains;
    [SerializeField] private List<List<MagicStalactite>> stalactites = new List<List<MagicStalactite>>();
    [SerializeField] private BossRoomDoorKnob leftDoor;
    [SerializeField] private BossRoomDoorKnob rightDoor;

    [Title("���� üũ")]
    [ReadOnly][SerializeField] private int golemCoreCount = 0;
    [ReadOnly][SerializeField] private int manaFountainCount = 4;

    [Title("������")]
    [InfoBox("������ ������ �°� ������ �ƴϿ��� �����ϴ����� ����\n true�� ������ �ƴϿ��� ����")] public bool canBossStun = false;
    [InfoBox("����� ��Ÿ��")] public float regenerateStalactiteTime = 10.0f;
    [InfoBox("���� ����")] public int numberOfSector = 3;
    [InfoBox("���� �� ������ ����")] public int stalactitePerSector = 3;
    [InfoBox("���� ���� ������")] public float fieldRadius = 25.0f;
    [InfoBox("���� ����")] public float fieldHeight = 8.0f;

    [Title("������ ��")]
    [InfoBox("����� ��Ÿ��")] public float respawnManaFountainTime = 10.0f;
    [InfoBox("���� ������ ����� ��Ÿ��")] public float regenerateManaStoneTime = 10.0f;

    [Title("������ ��")]
    [InfoBox("������ �ð�")] public float openSpeedTime = 3.0f;
    [InfoBox("������ ����")] public float openAngle = 120.0f;

    private TicketMachine ticketMachine;

    public TicketMachine TicketMachine
    {
        get { return ticketMachine; }
    }

    private void Awake()
    {
        SubscribeEvents();
        SpawnStalactites();
        InitManaFountains();
        InitTicketMachine();

        manaFountainCount = manaFountains.Count;
    }

    #region 1. �ʱ�ȭ �Լ�
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
    }
    private void SubscribeEvents()
    {
        EventBus.Instance.Subscribe<BossEventPayload>(EventBusEvents.HitManaByPlayerStone, OnHitMana);
        EventBus.Instance.Subscribe<BossEventPayload>(EventBusEvents.DestroyedManaByBoss1, OnDestroyedMana);
        EventBus.Instance.Subscribe<BossEventPayload>(EventBusEvents.DropMagicStalactite, OnDropMagicStalactite);
        EventBus.Instance.Subscribe<BossEventPayload>(EventBusEvents.BossRoomDoorOpen, OnBossRoomDoorOpen);
    }
    #endregion

    #region 2. �̺�Ʈ �ڵ鷯
    private void OnHitMana(BossEventPayload manaPayload)
    {
        Debug.Log("OnHitMana :: ������ �� ��Ÿ�� ����");

        ManaFountain mana = manaPayload.TransformValue1.GetComponent<ManaFountain>();
        DropStoneItem(mana.SpawnPosition, mana.MAGICSTONE_INDEX);

        StartCoroutine(ManaCooldown(manaPayload));
    }
    private void OnDestroyedMana(BossEventPayload manaPayload)
    {
        Debug.Log($"OnDestroyedMana :: {manaPayload.AttackTypeValue} ���� Ÿ�� ����");

        // ������ ���� �¾��� ���, �� ����
        if (manaPayload.TransformValue2 != null)
        {
            Destroy(manaPayload.TransformValue2.gameObject);
        }

        // ������ ���� ���� ����
        Transform manaTransform = manaPayload.TransformValue1;
        ManaFountain mana = manaTransform.GetComponent<ManaFountain>();
        TerrapupaBTController actor = manaPayload.Sender.GetComponent<TerrapupaBTController>();
        if (actor == null)
        {
            actor = manaPayload.Sender.GetComponent<TerrapupaStone>().Owner.GetComponent<TerrapupaBTController>();
            manaPayload.Sender = actor.transform;
        }

        // ������ 3�� ����
        for (int i = 0; i < 3; i++)
        {
            DropStoneItem(mana.SpawnPosition, mana.NORMALSTONE_INDEX);
        }

        // ��Ʈ ����Ʈ ����
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

        // ������ ���� ���� ��Ÿ�� ���� ���߰�, �ı� ��Ÿ������ ���� �����Ŵ
        TerrapupaAttackType type = mana.banBossAttackType;
        if (actor.AttackCooldown.ContainsKey(type) && actor.AttackCooldown[type] != null)
        {
            Debug.Log($"{actor}�� ��Ÿ�� �ߺ� ����");
            actor.StopCoroutine(actor.AttackCooldown[type]);
        }

        // �����̸� ���� ���� ���� ��Ÿ�� ����
        manaPayload.BoolValue = false;
        EventBus.Instance.Publish(EventBusEvents.ApplyBossCooldown, manaPayload);

        StartCoroutine(ManaRespawn(manaPayload));
    }
    private void OnDropMagicStalactite(BossEventPayload stalactitePayload)
    {
        Debug.Log($"OnDropMagicStalactite :: ������ ���");

        Transform boss = stalactitePayload.TransformValue2;
        if (boss != null)
        {
            TerrapupaBTController actor = boss.GetComponent<TerrapupaBTController>();
            if (stalactitePayload.TransformValue2 != null)
            {
                Debug.Log("���� Ÿ��");

                // 
                if (canBossStun == true || (canBossStun == false && actor.terrapupaData.isIntake.Value))
                {
                    Debug.Log("����");

                    actor.terrapupaData.isStuned.Value = true;
                    actor.terrapupaData.isTempted.Value = false;
                    actor.terrapupaData.isIntake.Value = false;
                }
            }
        }

        StartCoroutine(RespawnMagicStalactite(stalactitePayload));
    }
    private void OnBossRoomDoorOpen(BossEventPayload payload)
    {
        Debug.Log($"OnBossRoomDoorOpen :: ������ �� ���� üũ");
        GolemCoreStone core = payload.TransformValue1.GetComponent<GolemCoreStone>();
        if (core == null)
        {
            return;
        }
        BossRoomDoorKnob knob = payload.Sender.GetComponent<BossRoomDoorKnob>();

        knob.Init(core.transform);
        golemCoreCount++;

        if (golemCoreCount == 2)
        {
            // �� ����
            OpenDoor();
        }
    }
    #endregion

    #region 3. �ڷ�ƾ �Լ�
    private IEnumerator ManaCooldown(BossEventPayload manaPayload)
    {
        ManaFountain mana = manaPayload.TransformValue1.GetComponent<ManaFountain>();
        mana.IsCooldown = true;

        yield return new WaitForSeconds(mana.coolDownValue);

        Debug.Log($"{mana.name} ��Ÿ�� �Ϸ�");
        mana.IsCooldown = false;
    }
    private IEnumerator RespawnMagicStalactite(BossEventPayload payload)
    {
        float respawnTime = payload.FloatValue;
        Debug.Log($"{respawnTime}�� ���� �����");

        yield return new WaitForSeconds(respawnTime);

        Vector3 position = GenerateRandomPositionInSector(payload.IntValue);
        payload.TransformValue1.position = position;
        payload.TransformValue1.gameObject.SetActive(true);
    }
    private IEnumerator ManaRespawn(BossEventPayload manaPayload)
    {
        manaFountainCount--;
        ManaFountain mana = manaPayload.TransformValue1.GetComponent<ManaFountain>();
        mana.IsBroken = true;
        mana.gameObject.SetActive(false);

        if (manaFountainCount == 0)
        {
            EventBus.Instance.Publish(EventBusEvents.DestroyAllManaFountain);
        }

        yield return new WaitForSeconds(mana.respawnValue);

        Debug.Log($"{mana.name} ������ �Ϸ�");

        manaFountainCount++;
        mana.gameObject.SetActive(true);
        mana.IsBroken = false;
        mana.IsCooldown = false;

        manaPayload.BoolValue = true;
        EventBus.Instance.Publish(EventBusEvents.ApplyBossCooldown, manaPayload);
    }
    #endregion

    #region 4. ��Ÿ �Լ�
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
    private void OpenDoor()
    {
        leftDoor.OpenDoor(-openAngle, openSpeedTime);
        rightDoor.OpenDoor(openAngle, openSpeedTime);
    }
    #endregion
}
