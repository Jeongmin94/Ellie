using Assets.Scripts.StatusEffects;
using Channels.Combat;
using Sirenix.OdinInspector;
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

    [Title("사운드 설정")]
    [InfoBox("하단공격 공격 사운드")] public string sound1 = "TerrapupaAttackHit";
    [InfoBox("하단공격 공격 사운드")] public string sound2 = "TerrapupaRollStart";

    [Title("이펙트 설정")]
    [InfoBox("공격 이펙트")] public GameObject lowAttackEffect1;

    [Title("하단 공격")]
    [InfoBox("공격 쿨타임")] public float cooldown = 10.0f;
    [InfoBox("타겟팅 회전 속도")] public float rotationSpeed = 4.0f;

    [Title("피격 정보")]
    [InfoBox("공격력")] public int attackValue = 5;
    [InfoBox("상태 이상")] public StatusEffectName statusEffect = StatusEffectName.Down;
    [InfoBox("상태 이상 지속시간")] public float statusEffectDuration = 0.5f;
    [InfoBox("상태 이상 힘(force)")] public float statusEffectForce = 10.0f;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<string>("sound1", sound1, tree);
        SetBlackboardValue<string>("sound2", sound2, tree);
        SetBlackboardValue<GameObject>("effect1", lowAttackEffect1, tree);
        SetBlackboardValue<float>("cooldown", cooldown, tree);
        SetBlackboardValue<float>("rotationSpeed", rotationSpeed, tree);
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