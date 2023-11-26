using System.Collections;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "LowAttack", menuName = "Terrapupa/LowAttack")]
public class TerrapupaLowAttackData : BaseBTData
{
    public TerrapupaLowAttackData()
    {
        dataName = "TerrapupaLowAttack";
    }

    [Header("이펙트 설정")]
    [Tooltip("공격 이펙트")] public GameObject lowAttackEffect1;

    [Header("하단 공격")]
    [Tooltip("공격 쿨타임")] public float lowAttackCooldown = 10.0f;
    [Tooltip("타겟팅 회전 속도")] public float lowAttackRotationSpeed = 4.0f;
    [Tooltip("공격력")] public int lowAttackAttackValue = 5;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<GameObject>("effect1", lowAttackEffect1, tree);
        SetBlackboardValue<float>("cooldown", lowAttackCooldown, tree);
        SetBlackboardValue<float>("rotationSpeed", lowAttackRotationSpeed, tree);
        SetBlackboardValue<int>("attackValue", lowAttackAttackValue, tree);
    }
}