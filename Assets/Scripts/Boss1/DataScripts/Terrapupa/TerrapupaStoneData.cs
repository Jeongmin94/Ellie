using Assets.Scripts.StatusEffects;
using Channels.Combat;
using System.Collections;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "Stone", menuName = "Terrapupa/Stone")]
public class TerrapupaStoneData : BaseBTData
{
    public TerrapupaStoneData()
    {
        dataName = "TerrapupaStone";
    }

    [Header("이펙트 설정")]
    [Tooltip("돌 줍는 상황 이펙트")] public GameObject stoneEffect1;
    [Tooltip("돌맹이 피격 시 이펙트")] public GameObject stoneEffect2;

    [Header("돌 던지기 공격")]
    [Tooltip("공격 쿨타임")] public float cooldown = 10.0f;
    [Tooltip("타겟팅 회전 속도")] public float rotationSpeed = 1.0f;
    [Tooltip("돌의 이동 속도")] public float movementSpeed = 15.0f;

    [Header("피격 정보")]
    [Tooltip("공격력")] public int attackValue = 5;
    [Tooltip("상태 이상")] public StatusEffectName statusEffect = StatusEffectName.Down;
    [Tooltip("상태 이상 지속시간")] public float statusEffectDuration = 0.5f;
    [Tooltip("상태 이상 힘(force)")] public float statusEffectForce = 10.0f;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<GameObject>("effect1", stoneEffect1, tree);
        SetBlackboardValue<GameObject>("effect2", stoneEffect2, tree);
        SetBlackboardValue<float>("cooldown", cooldown, tree);
        SetBlackboardValue<float>("rotationSpeed", rotationSpeed, tree);
        SetBlackboardValue<float>("movementSpeed", movementSpeed, tree);
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