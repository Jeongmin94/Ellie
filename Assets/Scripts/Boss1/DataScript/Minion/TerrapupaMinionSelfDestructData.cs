using Channels.Combat;
using Player.StatusEffects;
using Sirenix.OdinInspector;
using TheKiwiCoder;
using UnityEngine;

namespace Boss1.DataScript.Minion
{
    [CreateAssetMenu(fileName = "SelfDestruct", menuName = "Terrapupa/MinionSelfDestruct")]
    public class TerrapupaMinionSelfDestructData : BehaviourTreeData
    {
        [Title("사운드 설정")] [InfoBox("땅 뒤집기 공격 사운드")]
        public string sound1 = "MinionSelfDestruct";

        [Header("이펙트 설정")] [Tooltip("공격 이펙트")] public GameObject attackEffect1;

        [Header("공격 설정")] [Tooltip("타겟팅 회전 속도")]
        public float rotationSpeed = 2.0f;

        [Tooltip("돌진 점프 높이")] public float jumpPower = 5.0f;
        [Tooltip("돌진 속도")] public float rushSpeed = 6.0f;

        [Header("피격 정보")] [Tooltip("공격력")] public int attackValue = 8;

        [Tooltip("상태 이상")] public StatusEffectName statusEffect = StatusEffectName.KnockedAirborne;
        [Tooltip("상태 이상 지속시간")] public float statusEffectDuration = 1.0f;
        [Tooltip("상태 이상 힘(force)")] public float statusEffectForce = 15.0f;

        public TerrapupaMinionSelfDestructData()
        {
            dataName = "TerrapupaMinionSelfDestruct";
        }

        public override void Init(BehaviourTree tree)
        {
            SetBlackboardValue("sound1", sound1, tree);
            SetBlackboardValue("effect1", attackEffect1, tree);
            SetBlackboardValue("rotationSpeed", rotationSpeed, tree);
            SetBlackboardValue("jumpPower", jumpPower, tree);
            SetBlackboardValue("rushSpeed", rushSpeed, tree);
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