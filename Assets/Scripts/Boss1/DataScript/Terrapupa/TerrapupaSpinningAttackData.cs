using Channels.Combat;
using Player.StatusEffects;
using Sirenix.OdinInspector;
using TheKiwiCoder;
using UnityEngine;

namespace Boss1.DataScript.Terrapupa
{
    [CreateAssetMenu(fileName = "SpinningAttack", menuName = "Terrapupa/SpinningAttack")]
    public class TerrapupaSpinningAttackData : BaseBTData
    {
        [Title("사운드 설정")] [InfoBox("회전공격 공격 사운드")]
        public string sound1 = "TerrapupaAttackHit";

        [InfoBox("회전공격 공격시작 사운드")] public string sound2 = "TerrapupaRollStart";

        [Title("이펙트 설정")] [InfoBox("회전 공격 시 이펙트")]
        public GameObject spinningAttackEffect1;

        [Title("회전 공격")] [InfoBox("플레이어에게 타겟팅 회전 속도")]
        public float rotationSpeed = 2.0f;

        [InfoBox("회전 공격 중 이동 속도")] public float movementSpeed = 1.0f;
        [InfoBox("회전 공격 공격 적중 거리")] public float effectiveRadius = 7.0f;

        [Title("피격 정보")] [InfoBox("공격력")] public int attackValue = 5;

        [InfoBox("상태 이상")] public StatusEffectName statusEffect = StatusEffectName.WeakRigidity;
        [InfoBox("상태 이상 지속시간")] public float statusEffectDuration = 0.2f;
        [InfoBox("상태 이상 힘(force)")] public float statusEffectForce;

        public TerrapupaSpinningAttackData()
        {
            dataName = "TerrapupaSpinningAttack";
        }

        public override void Init(BehaviourTree tree)
        {
            SetBlackboardValue("sound1", sound1, tree);
            SetBlackboardValue("sound2", sound2, tree);
            SetBlackboardValue("effect1", spinningAttackEffect1, tree);
            SetBlackboardValue("rotationSpeed", rotationSpeed, tree);
            SetBlackboardValue("movementSpeed", movementSpeed, tree);
            SetBlackboardValue("effectiveRadius", effectiveRadius, tree);
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
}