using Assets.Scripts.StatusEffects;
using Channels.Combat;
using Sirenix.OdinInspector;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "LowAttack", menuName = "Terrapupa/LowAttack")]
public class TerrapupaLowAttackData : BaseBTData
{
    [Title("사운드 설정")] [InfoBox("하단공격 공격 사운드")]
    public string sound1 = "TerrapupaAttackHit";

    [InfoBox("하단공격 공격 사운드")] public string sound2 = "TerrapupaRollStart";

    [Title("이펙트 설정")] [InfoBox("공격 이펙트")] public GameObject lowAttackEffect1;

    [Title("하단 공격")] [InfoBox("공격 쿨타임")] public float cooldown = 10.0f;

    [InfoBox("타겟팅 회전 속도")] public float rotationSpeed = 4.0f;

    [Title("피격 정보")] [InfoBox("공격력")] public int attackValue = 5;

    [InfoBox("상태 이상")] public StatusEffectName statusEffect = StatusEffectName.Down;
    [InfoBox("상태 이상 지속시간")] public float statusEffectDuration = 0.5f;
    [InfoBox("상태 이상 힘(force)")] public float statusEffectForce = 10.0f;

    public TerrapupaLowAttackData()
    {
        dataName = "TerrapupaLowAttack";
    }

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue("sound1", sound1, tree);
        SetBlackboardValue("sound2", sound2, tree);
        SetBlackboardValue("effect1", lowAttackEffect1, tree);
        SetBlackboardValue("cooldown", cooldown, tree);
        SetBlackboardValue("rotationSpeed", rotationSpeed, tree);
        SetBlackboardValue("attackValue", attackValue, tree);
        SetBlackboardValue<IBaseEventPayload>("combatPayload", new CombatPayload
        {
            Damage = attackValue,
            StatusEffectName = statusEffect,
            statusEffectduration = statusEffectDuration,
            force = statusEffectForce
        }, tree);
    }
}