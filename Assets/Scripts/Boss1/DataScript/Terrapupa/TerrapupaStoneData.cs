using Assets.Scripts.StatusEffects;
using Channels.Combat;
using Sirenix.OdinInspector;
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

    [Title("사운드 설정")]
    [InfoBox("돌 주울때 사운드")] public string sound1 = "TerrapupaGripStone";
    [InfoBox("돌 히트 사운드")] public string sound2 = "TerrapupaAttackHit";
    [InfoBox("돌 던질때 사운드")] public string sound3 = "TerrapupaThrowStone";

    [Title("이펙트 설정")]
    [InfoBox("돌 줍는 상황 이펙트")] public GameObject stoneEffect1;
    [InfoBox("돌맹이 피격 시 이펙트")] public GameObject stoneEffect2;

    [Title("돌 던지기 공격")]
    [InfoBox("공격 쿨타임")] public float cooldown = 10.0f;
    [InfoBox("타겟팅 회전 속도")] public float rotationSpeed = 1.0f;
    [InfoBox("돌의 이동 속도")] public float movementSpeed = 15.0f;

    [Title("피격 정보")]
    [InfoBox("공격력")] public int attackValue = 5;
    [InfoBox("상태 이상")] public StatusEffectName statusEffect = StatusEffectName.Down;
    [InfoBox("상태 이상 지속시간")] public float statusEffectDuration = 0.5f;
    [InfoBox("상태 이상 힘(force)")] public float statusEffectForce = 10.0f;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<string>("sound1", sound1, tree);
        SetBlackboardValue<string>("sound2", sound2, tree);
        SetBlackboardValue<string>("sound3", sound3, tree);
        SetBlackboardValue<GameObject>("effect1", stoneEffect1, tree);
        SetBlackboardValue<GameObject>("effect2", stoneEffect2, tree);
        SetBlackboardValue<float>("cooldown", cooldown, tree);
        SetBlackboardValue<float>("rotationSpeed", rotationSpeed, tree);
        SetBlackboardValue<float>("movementSpeed", movementSpeed, tree);
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