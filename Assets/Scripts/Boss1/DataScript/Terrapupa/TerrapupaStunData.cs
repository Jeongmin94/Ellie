﻿using Sirenix.OdinInspector;
using TheKiwiCoder;
using UnityEngine;

namespace Boss1.DataScript.Terrapupa
{
    [CreateAssetMenu(fileName = "Stun", menuName = "Terrapupa/Stun")]
    public class TerrapupaStunData : BehaviourTreeData
    {
        [Title("기절 상태")] [InfoBox("기절 지속시간")] public float stunDuration = 10.0f;

        public BlackboardKey<float> duration;

        public TerrapupaStunData()
        {
            dataName = "TerrapupaStun";
        }

        public override void Init(BehaviourTree tree)
        {
            SetBlackboardValue("duration", stunDuration, tree);

            duration = FindBlackboardKey<float>("duration", tree);
        }
    }
}