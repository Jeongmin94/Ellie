using Assets.Scripts.StatusEffects;
using Channels.Combat;
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
    [Tooltip("공격 쿨타임")] public float cooldown = 10.0f;
    [Tooltip("타겟팅 회전 속도")] public float rotationSpeed = 4.0f;

    [Header("피격 정보")]
    [Tooltip("공격력")] public int attackValue = 5;
    [Tooltip("상태 이상")] public StatusEffectName statusEffect = StatusEffectName.Down;
    [Tooltip("상태 이상 지속시간")] public float statusEffectDuration = 0.5f;
    [Tooltip("상태 이상 힘(force)")] public float statusEffectForce = 10.0f;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<GameObject>("effect1", lowAttackEffect1, tree);
        SetBlackboardValue<float>("cooldown", cooldown, tree);
        SetBlackboardValue<float>("rotationSpeed", rotationSpeed, tree);
        SetBlackboardValue<int>("attackValue", attackValue, tree);
        SetBlackboardValue<IBaseEventPayload>("combatPayload", new CombatPayload
        {
            Damage = attackValue,
            PlayerStatusEffectName = statusEffect,
            statusEffectduration = statusEffectDuration,
            force = statusEffectForce,
        }, tree);
    }
}