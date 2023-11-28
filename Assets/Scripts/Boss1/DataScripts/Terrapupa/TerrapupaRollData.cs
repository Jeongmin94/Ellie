using Assets.Scripts.StatusEffects;
using Channels.Combat;
using System.Collections;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "Roll", menuName = "Terrapupa/Roll")]
public class TerrapupaRollData : BaseBTData
{
    public TerrapupaRollData()
    {
        dataName = "TerrapupaRoll";
    }

    [Header("이펙트 설정")]
    [Tooltip("돌진 시작 시 이펙트")] public GameObject rollEffect1;
    [Tooltip("돌진 중 충돌 시 이펙트")] public GameObject rollEffect2;

    [Header("구르기 공격")]
    [Tooltip("공격 쿨타임")] public float cooldown = 10.0f;
    [Tooltip("타겟팅 회전 속도")] public float rotationSpeed = 5.0f;
    [Tooltip("구르기 종료 벽 인식 거리(Raycast 길이)")] public float rayCastLength = 7.0f;
    [Tooltip("구르기 이동속도")] public float movementSpeed = 20.0f;
    [Tooltip("구르기 가속도")] public float acceleration = 0.3f;
    [Tooltip("구르기 감속도")] public float dcceleration = 0.3f;

    [Header("피격 정보")]
    [Tooltip("공격력")] public int attackValue = 5;
    [Tooltip("상태 이상")] public StatusEffectName statusEffect = StatusEffectName.KnockedAirborne;
    [Tooltip("상태 이상 지속시간")] public float statusEffectDuration = 1.0f;
    [Tooltip("상태 이상 힘(force)")] public float statusEffectForce = 15.0f;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<GameObject>("effect1", rollEffect1, tree);
        SetBlackboardValue<GameObject>("effect2", rollEffect2, tree);
        SetBlackboardValue<float>("cooldown", cooldown, tree);
        SetBlackboardValue<float>("rotationSpeed", rotationSpeed, tree);
        SetBlackboardValue<float>("rayCastLength", rayCastLength, tree);
        SetBlackboardValue<float>("movementSpeed", movementSpeed, tree);
        SetBlackboardValue<float>("acceleration", acceleration, tree);
        SetBlackboardValue<float>("dcceleration", dcceleration, tree);
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