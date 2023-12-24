using Channels.Combat;
using Player.StatusEffects;
using Sirenix.OdinInspector;
using TheKiwiCoder;
using UnityEngine;

namespace Boss1.DataScript.Terrapupa
{
    [CreateAssetMenu(fileName = "Stone", menuName = "Terrapupa/Stone")]
    public class TerrapupaStoneData : BaseBTData
    {
        [Title("사운드 설정")] [InfoBox("돌 주울때 사운드")]
        public string sound1 = "TerrapupaGripStone";

        [InfoBox("돌 히트 사운드")] public string sound2 = "TerrapupaAttackHit";
        [InfoBox("돌 던질때 사운드")] public string sound3 = "TerrapupaThrowStone";

        [Title("이펙트 설정")] [InfoBox("돌 줍는 상황 이펙트")]
        public GameObject stoneEffect1;

        [InfoBox("돌맹이 피격 시 이펙트")] public GameObject stoneEffect2;

        [Title("돌 던지기 공격")] [InfoBox("공격 쿨타임")]
        public float cooldown = 10.0f;

        [InfoBox("타겟팅 회전 속도")] public float rotationSpeed = 1.0f;
        [InfoBox("돌의 이동 속도")] public float movementSpeed = 15.0f;

        [Title("피격 정보")] [InfoBox("공격력")] public int attackValue = 5;

        [InfoBox("상태 이상")] public StatusEffectName statusEffect = StatusEffectName.Down;
        [InfoBox("상태 이상 지속시간")] public float statusEffectDuration = 0.5f;
        [InfoBox("상태 이상 힘(force)")] public float statusEffectForce = 10.0f;

        public TerrapupaStoneData()
        {
            dataName = "TerrapupaStone";
        }

        public override void Init(BehaviourTree tree)
        {
            SetBlackboardValue("sound1", sound1, tree);
            SetBlackboardValue("sound2", sound2, tree);
            SetBlackboardValue("sound3", sound3, tree);
            SetBlackboardValue("effect1", stoneEffect1, tree);
            SetBlackboardValue("effect2", stoneEffect2, tree);
            SetBlackboardValue("cooldown", cooldown, tree);
            SetBlackboardValue("rotationSpeed", rotationSpeed, tree);
            SetBlackboardValue("movementSpeed", movementSpeed, tree);
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