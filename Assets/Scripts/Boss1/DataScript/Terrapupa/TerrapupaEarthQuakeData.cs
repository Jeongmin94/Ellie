using Assets.Scripts.StatusEffects;
using Channels.Combat;
using Sirenix.OdinInspector;
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

    [Title("사운드 설정")]
    [InfoBox("땅 뒤집기 공격 사운드")] public string sound1 = "TerrapupaEarthQuake";
    [InfoBox("땅 뒤집기 공격 시작 사운드")] public string sound2 = "TerapupaEarthQuakeJump";

    [Title("이펙트 설정")]
    [InfoBox("내려 찍기 이펙트")] public GameObject earthQuakeEffect1;

    [Title("땅 뒤집기 공격")]
    [InfoBox("공격 쿨타임")] public float cooldown = 10.0f;
    [InfoBox("타겟팅 회전 속도")] public float rotationSpeed = 4.0f;
    [InfoBox("공격 이동 거리")] public float moveDistance = 10.0f;
    [InfoBox("점프 중 이동 속도")] public float movementSpeed = 8.0f;
    [InfoBox("공격 범위 중심각")] public float attackAngle = 45.0f;
    [InfoBox("공격 적중 거리")] public float effectiveRadius = 50.0f;

    [Title("피격 정보")]
    [InfoBox("공격력")] public int attackValue = 5;
    [InfoBox("상태 이상")] public StatusEffectName statusEffect = StatusEffectName.KnockedAirborne;
    [InfoBox("상태 이상 지속시간")] public float statusEffectDuration = 1.0f;
    [InfoBox("상태 이상 힘(force)")] public float statusEffectForce = 15.0f;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<string>("sound1", sound1, tree);
        SetBlackboardValue<string>("sound2", sound2, tree);
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
            StatusEffectName = statusEffect,
            statusEffectduration = statusEffectDuration,
            force = statusEffectForce,
        }, tree);
    }
}