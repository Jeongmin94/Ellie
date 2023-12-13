using Assets.Scripts.StatusEffects;
using Channels.Combat;
using System.Collections;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "SelfDestruct", menuName = "Terrapupa/MinionSelfDestruct")]
public class TerrapupaMinionSelfDestructData : BaseBTData
{
    public TerrapupaMinionSelfDestructData()
    {
        dataName = "TerrapupaMinionSelfDestruct";
    }

    [Header("이펙트 설정")]
    [Tooltip("공격 이펙트")] public GameObject attackEffect1;

    [Header("공격 설정")]
    [Tooltip("타겟팅 회전 속도")] public float rotationSpeed = 2.0f;
    [Tooltip("돌진 점프 높이")] public float jumpPower = 5.0f;
    [Tooltip("돌진 속도")] public float rushSpeed = 6.0f;

    [Header("피격 정보")]
    [Tooltip("공격력")] public int attackValue = 8;
    [Tooltip("상태 이상")] public StatusEffectName statusEffect = StatusEffectName.KnockedAirborne;
    [Tooltip("상태 이상 지속시간")] public float statusEffectDuration = 1.0f;
    [Tooltip("상태 이상 힘(force)")] public float statusEffectForce = 15.0f;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<GameObject>("effect1", attackEffect1, tree);
        SetBlackboardValue<float>("rotationSpeed", rotationSpeed, tree);
        SetBlackboardValue<float>("jumpPower", jumpPower, tree);
        SetBlackboardValue<float>("rushSpeed", rushSpeed, tree);
        SetBlackboardValue<int>("attackValue", attackValue, tree);
        SetBlackboardValue<IBaseEventPayload>("combatPayload", new CombatPayload
        {
            Damage = attackValue,
            StatusEffectName = statusEffect,
            statusEffectduration = statusEffectDuration,
            force = statusEffectForce,
        }, tree);
    }
}