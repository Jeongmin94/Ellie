﻿using Channels.Combat;
using Player.StatusEffects;
using Sirenix.OdinInspector;
using TheKiwiCoder;
using UnityEngine;

namespace Boss1.DataScript.Terrapupa
{
    [CreateAssetMenu(fileName = "Roll", menuName = "Terrapupa/Roll")]
    public class TerrapupaRollData : BehaviourTreeData
    {
        [Title("사운드 설정")] [InfoBox("돌진 공격 사운드")]
        public string sound1 = "TerrapupaRoll";

        [InfoBox("돌진 공격 히트 사운드")] public string sound2 = "TerrapupaRollHit";
        [InfoBox("돌진 공격 시작 사운드")] public string sound3 = "TerrapupaRollStart";

        [Title("이펙트 설정")] [InfoBox("돌진 시작 시 이펙트")]
        public GameObject rollEffect1;

        [InfoBox("돌진 중 충돌 시 이펙트")] public GameObject rollEffect2;

        [Title("구르기 공격")] [InfoBox("공격 쿨타임")] public float cooldown = 10.0f;

        [InfoBox("타겟팅 회전 속도")] public float rotationSpeed = 5.0f;

        [InfoBox("구르기 종료 벽 인식 거리(Raycast 길이)")]
        public float rayCastLength = 7.0f;

        [InfoBox("구르기 이동속도")] public float movementSpeed = 20.0f;
        [InfoBox("구르기 가속도")] public float acceleration = 0.3f;
        [InfoBox("구르기 감속도")] public float dcceleration = 0.3f;

        [Title("피격 정보")] [InfoBox("공격력")] public int attackValue = 5;

        [InfoBox("상태 이상")] public StatusEffectName statusEffect = StatusEffectName.KnockedAirborne;
        [InfoBox("상태 이상 지속시간")] public float statusEffectDuration = 1.0f;
        [InfoBox("상태 이상 힘(force)")] public float statusEffectForce = 15.0f;

        public TerrapupaRollData()
        {
            dataName = "TerrapupaRoll";
        }

        public override void Init(BehaviourTree tree)
        {
            SetBlackboardValue("sound1", sound1, tree);
            SetBlackboardValue("sound2", sound2, tree);
            SetBlackboardValue("sound3", sound3, tree);
            SetBlackboardValue("effect1", rollEffect1, tree);
            SetBlackboardValue("effect2", rollEffect2, tree);
            SetBlackboardValue("cooldown", cooldown, tree);
            SetBlackboardValue("rotationSpeed", rotationSpeed, tree);
            SetBlackboardValue("rayCastLength", rayCastLength, tree);
            SetBlackboardValue("movementSpeed", movementSpeed, tree);
            SetBlackboardValue("acceleration", acceleration, tree);
            SetBlackboardValue("dcceleration", dcceleration, tree);
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