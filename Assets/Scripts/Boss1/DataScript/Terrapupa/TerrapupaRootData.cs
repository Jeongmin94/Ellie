﻿using Sirenix.OdinInspector;
using TheKiwiCoder;
using UnityEngine;

namespace Boss1.DataScript.Terrapupa
{
    [CreateAssetMenu(fileName = "Root", menuName = "Terrapupa/Root")]
    public class TerrapupaRootData : BehaviourTreeData
    {
        [Title("사운드 설정")] 
        [InfoBox("일어날 때 소리")] public string sound1 = "TerrapupaGripStone";

        [Title("기본 수치")] 
        [InfoBox("보스 이름")] public string bossName;
        [InfoBox("보스의 체력")] public int hp = 20;

        [Title("패턴 사용 여부")] 
        [InfoBox("구르기")] public bool rollUsable = true;
        [InfoBox("돌 던지기")] public bool stoneUsable = true;
        [InfoBox("땅 뒤집기")] public bool earthQuakeUsable = true;
        [InfoBox("하단 공격")] public bool lowAttackUsable = true;

        [Title("공격 감지 범위")] 
        [InfoBox("장거리 공격")] public float LongRangeDetectionDistance = 25.0f;
        [InfoBox("중거리 공격")] public float MidRangeDetectionDistance = 15.0f;
        [InfoBox("단거리 공격")] public float ShortRangeDetectionDistance = 5.0f;

        [Title("피격시 카메라 강도")] 
        [InfoBox("카메라 흔들림 강도")] public float cameraShakeIntensity = 0.1f;
        [InfoBox("카메라 흔들림 지속시간")] public float cameraShakeDuration = 0.1f;
        
        public BlackboardKey<bool> canEarthQuake;
        public BlackboardKey<bool> canLowAttack;
        public BlackboardKey<bool> canRoll;
        public BlackboardKey<bool> canThrowStone;

        public BlackboardKey<int> currentHP;
        public BlackboardKey<bool> hitEarthQuake;

        public BlackboardKey<bool> hitThrowStone;
        public BlackboardKey<bool> isIntake;
        public BlackboardKey<bool> isStart;
        public BlackboardKey<bool> isStuned;
        public BlackboardKey<bool> isTempted;
        public BlackboardKey<Transform> magicStoneTransform;
        public BlackboardKey<Transform> objectTransform;

        public BlackboardKey<Transform> player;

        public TerrapupaRootData()
        {
            dataName = "TerrapupaRoot";
        }

        public override void Init(BehaviourTree tree)
        {
            SetBlackboardValue("sound1", sound1, tree);
            SetBlackboardValue("currentHP", hp, tree);
            SetBlackboardValue("canRoll", rollUsable, tree);
            SetBlackboardValue("canThrowStone", stoneUsable, tree);
            SetBlackboardValue("canEarthQuake", earthQuakeUsable, tree);
            SetBlackboardValue("canLowAttack", lowAttackUsable, tree);
            SetBlackboardValue("longRangeDetectionDistance", LongRangeDetectionDistance, tree);
            SetBlackboardValue("midRangeDetectionDistance", MidRangeDetectionDistance, tree);
            SetBlackboardValue("shortRangeDetectionDistance", ShortRangeDetectionDistance, tree);

            currentHP = FindBlackboardKey<int>("currentHP", tree);

            player = FindBlackboardKey<Transform>("player", tree);
            objectTransform = FindBlackboardKey<Transform>("objectTransform", tree);
            magicStoneTransform = FindBlackboardKey<Transform>("magicStoneTransform", tree);
            currentHP = FindBlackboardKey<int>("currentHP", tree);
            isStart = FindBlackboardKey<bool>("isStart", tree);
            canThrowStone = FindBlackboardKey<bool>("canThrowStone", tree);
            canEarthQuake = FindBlackboardKey<bool>("canEarthQuake", tree);
            canRoll = FindBlackboardKey<bool>("canRoll", tree);
            canLowAttack = FindBlackboardKey<bool>("canLowAttack", tree);
            isTempted = FindBlackboardKey<bool>("isTempted", tree);
            isIntake = FindBlackboardKey<bool>("isIntake", tree);
            isStuned = FindBlackboardKey<bool>("isStuned", tree);

            hitThrowStone = FindBlackboardKey<bool>("hitThrowStone", tree);
            hitEarthQuake = FindBlackboardKey<bool>("hitEarthQuake", tree);
        }
    }
}
