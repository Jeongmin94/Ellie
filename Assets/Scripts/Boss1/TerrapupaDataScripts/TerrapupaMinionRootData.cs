using System.Collections;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "Root", menuName = "Terrapupa/MinionRoot")]
public class TerrapupaMinionRootData : BaseBTData
{
    public TerrapupaMinionRootData()
    {
        dataName = "TerrapupaRoot";
    }

    [Header("기본 수치")]
    [Tooltip("보스 이름")] public string bossName;
    [Tooltip("보스의 체력")] public int hp = 20;

    public BlackboardKey<int> currentHP;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<int>("currentHP", hp, tree);

        currentHP = FindBlackboardKey<int>("currentHP", tree);
    }
}