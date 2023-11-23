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
    [Tooltip("보스의 체력")] public int hp = 20;

    public BlackboardKey<Transform> player;
    public BlackboardKey<int> currentHP;
    public BlackboardKey<bool> canAttack;
    public BlackboardKey<bool> isHit;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<int>("currentHP", hp, tree);
        SetBlackboardValue<bool>("canAttack", true, tree);
        SetBlackboardValue<bool>("isHit", false, tree);

        currentHP = FindBlackboardKey<int>("currentHP", tree);
        player = FindBlackboardKey<Transform>("player", tree);
        canAttack = FindBlackboardKey<bool>("canAttack", tree);
        isHit = FindBlackboardKey<bool>("isHit", tree);
    }
}