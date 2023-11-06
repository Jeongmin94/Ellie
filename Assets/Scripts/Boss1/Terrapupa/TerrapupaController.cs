using Assets.Scripts.Utils;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

namespace Boss.Terrapupa
{
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

        public TerrapupaWeakPoint WeakPoint
        {
            get { return weakPoint; }
            set { weakPoint = value; }
        }

        public BlackboardKey<Transform> player;
        public BlackboardKey<Transform> objectTransform;
        public BlackboardKey<Transform> magicStoneTransform;
        public BlackboardKey<int> currentHP;
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
            weakPoint.SubscribeCollisionAction(OnCollidedCoreByPlayerStone);
        }

        private void InitStatus()
        {
            behaviourTreeInstance.SetBlackboardValue<Transform>("player", target);

            player = behaviourTreeInstance.FindBlackboardKey<Transform>("player");
            objectTransform = behaviourTreeInstance.FindBlackboardKey<Transform>("objectTransform");
            magicStoneTransform = behaviourTreeInstance.FindBlackboardKey<Transform>("magicStoneTransform");
            currentHP = behaviourTreeInstance.FindBlackboardKey<int>("currentHP");
            canThrowStone = behaviourTreeInstance.FindBlackboardKey<bool>("canThrowStone");
            canEarthQuake = behaviourTreeInstance.FindBlackboardKey<bool>("canEarthQuake");
            canRoll = behaviourTreeInstance.FindBlackboardKey<bool>("canRoll");
            canLowAttack = behaviourTreeInstance.FindBlackboardKey<bool>("canLowAttack");
            isTempted = behaviourTreeInstance.FindBlackboardKey<bool>("isTempted");
            isIntake = behaviourTreeInstance.FindBlackboardKey<bool>("isIntake");
            isStuned = behaviourTreeInstance.FindBlackboardKey<bool>("isStuned");

            pos = behaviourTreeInstance.FindBlackboardKey<Vector3>("pos");
        }

        private void OnCollidedCoreByPlayerStone(IBaseEventPayload payload)
        {
            // 플레이어 총알 -> Combat Channel -> TerrapupaWeakPoint :: ReceiveDamage() -> TerrapupaController
            Debug.Log($"OnCollidedCoreByPlayerStone :: {payload}");

            if(isStuned.value)
            {
                CombatPayload combatPayload = payload as CombatPayload;
                int damage = combatPayload.Damage;

                currentHP.Value -= damage;
                Debug.Log($"기절 상태, 데미지 입음 {currentHP.Value}");
                
            }
            else 
            {
                Debug.Log("기절 상태가 아님");
            }
        }
    }
}