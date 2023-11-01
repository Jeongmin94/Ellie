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

    public class TerrapupaController : MonoBehaviour
    {
        [SerializeField] private BehaviourTreeInstance behaviourTreeInstance;
        [SerializeField] private TerrapupaDataInfo data;

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

        public BlackboardKey<Transform> targetTransform;
        public BlackboardKey<Transform> objectTransform;
        public BlackboardKey<Transform> magicStoneTransform;
        public BlackboardKey<Vector3> targetPosition;
        public BlackboardKey<int> currentHP;
        public BlackboardKey<float> moveSpeed;
        public BlackboardKey<bool> canThrowStone;
        public BlackboardKey<bool> canEarthQuake;
        public BlackboardKey<bool> canRoll;
        public BlackboardKey<bool> canLowAttack;
        public BlackboardKey<bool> isTempted;
        public BlackboardKey<bool> isIntake;
        public BlackboardKey<bool> isStuned;
        public BlackboardKey<IBaseEventPayload> throwStonePayload;
        public BlackboardKey<IBaseEventPayload> occurEarthQuakePayload;

        private void Start()
        {
            InitStatus();

            weakPoint.collisionAction += OnCollidedCoreByPlayerStone;
        }

        private void Update()
        {
            targetPosition.value = target.position;
        }

        private void InitStatus()
        {
            behaviourTreeInstance.SetBlackboardValue<Transform>("targetTransform", target);
            behaviourTreeInstance.SetBlackboardValue<Vector3>("targetPosition", target.position);
            behaviourTreeInstance.SetBlackboardValue<int>("currentHP", data.hp);
            behaviourTreeInstance.SetBlackboardValue<bool>("canThrowStone", true);
            behaviourTreeInstance.SetBlackboardValue<bool>("canEarthQuake", true);
            behaviourTreeInstance.SetBlackboardValue<bool>("canRoll", true);
            behaviourTreeInstance.SetBlackboardValue<bool>("canLowAttack", true);
            behaviourTreeInstance.SetBlackboardValue<bool>("isTempted", false);
            behaviourTreeInstance.SetBlackboardValue<bool>("isIntake", false);
            behaviourTreeInstance.SetBlackboardValue<bool>("isStuned", false);

            behaviourTreeInstance.SetBlackboardValue<IBaseEventPayload>("throwStonePayload",
                new BossEventPayload { TransformValue1 = stone, TransformValue2 = target });
            behaviourTreeInstance.SetBlackboardValue<IBaseEventPayload>("occurEarthQuakePayload",
                new BossEventPayload { });

            targetTransform = behaviourTreeInstance.FindBlackboardKey<Transform>("targetTransform");
            objectTransform = behaviourTreeInstance.FindBlackboardKey<Transform>("objectTransform");
            magicStoneTransform = behaviourTreeInstance.FindBlackboardKey<Transform>("magicStoneTransform");
            targetPosition = behaviourTreeInstance.FindBlackboardKey<Vector3>("targetPosition");
            currentHP = behaviourTreeInstance.FindBlackboardKey<int>("currentHP");
            canThrowStone = behaviourTreeInstance.FindBlackboardKey<bool>("canThrowStone");
            canEarthQuake = behaviourTreeInstance.FindBlackboardKey<bool>("canEarthQuake");
            canRoll = behaviourTreeInstance.FindBlackboardKey<bool>("canRoll");
            canLowAttack = behaviourTreeInstance.FindBlackboardKey<bool>("canLowAttack");
            isTempted = behaviourTreeInstance.FindBlackboardKey<bool>("isTempted");
            isIntake = behaviourTreeInstance.FindBlackboardKey<bool>("isIntake");
            isStuned = behaviourTreeInstance.FindBlackboardKey<bool>("isStuned");
            
            throwStonePayload = behaviourTreeInstance.FindBlackboardKey<IBaseEventPayload>("throwStonePayload");
            occurEarthQuakePayload = behaviourTreeInstance.FindBlackboardKey<IBaseEventPayload>("occurEarthQuakePayload");
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