using System.Collections;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "Root", menuName = "Terrapupa/MinionRoot")]
public class TerrapupaMinionRootData : BaseBTData
{
    public TerrapupaMinionRootData()
    {
        dataName = "TerrapupaMinionRoot";
    }

    [Header("기본 수치")]
    [Tooltip("보스 이름")] public string bossName;
    [Tooltip("보스의 체력")] public int hp = 5;

    [Header("미니언 특성")]
    [Tooltip("패턴 전환 시간")] public float transferTime = 2.0f;

    [Header("공격 감지 범위")]
    [Tooltip("감지 범위")] public float attackDetectionDistance = 4.0f;

    public BlackboardKey<Transform> player;
    public BlackboardKey<int> currentHP;
    public BlackboardKey<bool> canAttack;
    public BlackboardKey<bool> isHit;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<int>("currentHP", hp, tree);
        SetBlackboardValue<float>("transferTime", transferTime, tree);
        SetBlackboardValue<float>("attackDetectionDistance", attackDetectionDistance, tree);

        SetBlackboardValue<bool>("canAttack", true, tree);
        SetBlackboardValue<bool>("isHit", false, tree);

        currentHP = FindBlackboardKey<int>("currentHP", tree);
        player = FindBlackboardKey<Transform>("player", tree);
        canAttack = FindBlackboardKey<bool>("canAttack", tree);
        isHit = FindBlackboardKey<bool>("isHit", tree);
    }
}