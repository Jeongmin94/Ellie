using Assets.Scripts.StatusEffects;
using Channels.Combat;
using System.Collections;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "EarthQuake", menuName = "Terrapupa/EarthQuake")]
public class TerrapupaEarthQuakeData : BaseBTData
{
    public TerrapupaEarthQuakeData()
    {
        dataName = "TerrapupaEarthQuake";
    }

    [Header("이펙트 설정")]
    [Tooltip("내려 찍기 이펙트")] public GameObject earthQuakeEffect1;

    [Header("땅 뒤집기 공격")]
    [Tooltip("공격 쿨타임")] public float cooldown = 10.0f;
    [Tooltip("타겟팅 회전 속도")] public float rotationSpeed = 4.0f;
    [Tooltip("공격 이동 거리")] public float moveDistance = 10.0f;
    [Tooltip("점프 중 이동 속도")] public float movementSpeed = 8.0f;
    [Tooltip("공격 범위 중심각")] public float attackAngle = 45.0f;
    [Tooltip("공격 적중 거리")] public float effectiveRadius = 50.0f;

    [Header("피격 정보")]
    [Tooltip("공격력")] public int attackValue = 5;
    [Tooltip("상태 이상")] public StatusEffectName statusEffect = StatusEffectName.KnockedAirborne;
    [Tooltip("상태 이상 지속시간")] public float statusEffectDuration = 1.0f;
    [Tooltip("상태 이상 힘(force)")] public float statusEffectForce = 15.0f;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<GameObject>("effect1", earthQuakeEffect1, tree);
        SetBlackboardValue<float>("cooldown", cooldown, tree);
        SetBlackboardValue<float>("rotationSpeed", rotationSpeed, tree);
        SetBlackboardValue<float>("moveDistance", moveDistance, tree);
        SetBlackboardValue<float>("movementSpeed", movementSpeed, tree);
        SetBlackboardValue<float>("attackAngle", attackAngle, tree);
        SetBlackboardValue<float>("effectiveRadius", effectiveRadius, tree);
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