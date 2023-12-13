using Assets.Scripts.StatusEffects;
using Channels.Combat;
using System.Collections;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Terrapupa/MinionAttack")]
public class TerrapupaMinionAttackData : BaseBTData
{
    public TerrapupaMinionAttackData()
    {
        dataName = "TerrapupaMinionAttack";
    }

    [Header("이펙트 설정")]
    [Tooltip("공격 이펙트")] public GameObject attackEffect1;

    [Header("공격 설정")]
    [Tooltip("공격 쿨타임")] public float cooldown = 5.0f;
    [Tooltip("타겟팅 회전 속도")] public float rotationSpeed = 2.0f;
    [Tooltip("공격 사거리")] public float attackDistance = 5.0f;

    [Header("피격 정보")]
    [Tooltip("공격력")] public int attackValue = 2;
    [Tooltip("상태 이상")] public StatusEffectName statusEffect = StatusEffectName.WeakRigidity;
    [Tooltip("상태 이상 지속시간")] public float statusEffectDuration = 0.5f;
    [Tooltip("상태 이상 힘(force)")] public float statusEffectForce = 0.0f;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<GameObject>("effect1", attackEffect1, tree);
        SetBlackboardValue<float>("cooldown", cooldown, tree);
        SetBlackboardValue<float>("rotationSpeed", rotationSpeed, tree);
        SetBlackboardValue<int>("attackValue", attackValue, tree);
        SetBlackboardValue<float>("attackDistance", attackDistance, tree);
        SetBlackboardValue<IBaseEventPayload>("combatPayload", new CombatPayload
        {
            Damage = attackValue,
            StatusEffectName = statusEffect,
            statusEffectduration = statusEffectDuration,
            force = statusEffectForce,
        }, tree);
    }
}