using System.Collections;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "Root", menuName = "Terrapupa/Root")]
public class TerrapupaRootData : BaseBTData
{
    public TerrapupaRootData()
    {
        dataName = "Root";
    }

    [Header("기본 수치")]
    [Tooltip("보스의 체력")] public int hp = 45;

    [Header("패턴 사용 여부")]
    [Tooltip("구르기")] public bool rollUsable = true;
    [Tooltip("돌 던지기")] public bool stoneUsable = true;
    [Tooltip("땅 뒤집기")] public bool earthQuakeUsable = true;
    [Tooltip("하단 공격")] public bool lowAttackQuakeUsable = true;

    [Header("공격 감지 범위")]
    [Tooltip("장거리 공격")] public float LongRangeDetectionDistance = 25.0f;
    [Tooltip("중거리 공격")] public float MidRangeDetectionDistance = 15.0f;
    [Tooltip("단거리 공격")] public float ShortRangeDetectionDistance = 5.0f;

    public BlackboardKey<int> currentHP;
    public BlackboardKey<float> longRangeDetectionDistance;
    public BlackboardKey<float> midRangeDetectionDistance;
    public BlackboardKey<float> shortRangeDetectionDistance;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<int>("currentHP", hp, tree);
        SetBlackboardValue<bool>("canRoll", rollUsable, tree);
        SetBlackboardValue<bool>("canThrowStone", stoneUsable, tree);
        SetBlackboardValue<bool>("canEarthQuake", earthQuakeUsable, tree);
        SetBlackboardValue<bool>("canLowAttack", lowAttackQuakeUsable, tree);
        SetBlackboardValue<float>("longRangeDetectionDistance", LongRangeDetectionDistance, tree);
        SetBlackboardValue<float>("midRangeDetectionDistance", MidRangeDetectionDistance, tree);
        SetBlackboardValue<float>("shortRangeDetectionDistance", ShortRangeDetectionDistance, tree);

        currentHP = FindBlackboardKey<int>("currentHP", tree);
        longRangeDetectionDistance = FindBlackboardKey<float>("longRangeDetectionDistance", tree);
        midRangeDetectionDistance = FindBlackboardKey<float>("midRangeDetectionDistance", tree);
        shortRangeDetectionDistance = FindBlackboardKey<float>("shortRangeDetectionDistance", tree);
    }
}