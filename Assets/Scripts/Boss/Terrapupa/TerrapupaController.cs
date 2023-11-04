using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

namespace Assets.Scripts.Boss.Terrapupa
{
    public enum TerrapupaAttackType
    {
        None,
        ThrowStone,
        EarthQuake,
        Roll,
        LowAttack,
    }

    public class TerrapupaController : BehaviourTreeController
    {
        [SerializeField] private Transform target;
        [SerializeField] private Transform stone;
        [SerializeField] private TerrapupaWeakPoint weakPoint;

        public Transform Target
        {
            get { return target; }
            set { target = value; }
        }

        public Transform Stone
        {
            get { return stone; }
            set { stone = value; }
        }

        public BlackboardKey<Transform> player;
        public BlackboardKey<Transform> objectTransform;
        public BlackboardKey<Transform> magicStoneTransform;
        public BlackboardKey<bool> canThrowStone;
        public BlackboardKey<bool> canEarthQuake;
        public BlackboardKey<bool> canRoll;
        public BlackboardKey<bool> canLowAttack;
        public BlackboardKey<bool> isTempted;
        public BlackboardKey<bool> isIntake;
        public BlackboardKey<bool> isStuned;

        public BlackboardKey<Vector3> pos;

        private void Start()
        {
            InitStatus();
            weakPoint.collisionAction += OnCollidedCoreByPlayerStone;
        }

        private void InitStatus()
        {
            behaviourTreeInstance.SetBlackboardValue<Transform>("player", target);

            player = behaviourTreeInstance.FindBlackboardKey<Transform>("player");
            objectTransform = behaviourTreeInstance.FindBlackboardKey<Transform>("objectTransform");
            magicStoneTransform = behaviourTreeInstance.FindBlackboardKey<Transform>("magicStoneTransform");
            canThrowStone = behaviourTreeInstance.FindBlackboardKey<bool>("canThrowStone");
            canEarthQuake = behaviourTreeInstance.FindBlackboardKey<bool>("canEarthQuake");
            canRoll = behaviourTreeInstance.FindBlackboardKey<bool>("canRoll");
            canLowAttack = behaviourTreeInstance.FindBlackboardKey<bool>("canLowAttack");
            isTempted = behaviourTreeInstance.FindBlackboardKey<bool>("isTempted");
            isIntake = behaviourTreeInstance.FindBlackboardKey<bool>("isIntake");
            isStuned = behaviourTreeInstance.FindBlackboardKey<bool>("isStuned");

            pos = behaviourTreeInstance.FindBlackboardKey<Vector3>("pos");
        }

        private void OnCollidedCoreByPlayerStone()
        {
            Debug.Log("충돌 확인");
            if(isStuned.value)
            {
                Debug.Log("기절 상태, 데미지 입음");
            }
        }
    }
}