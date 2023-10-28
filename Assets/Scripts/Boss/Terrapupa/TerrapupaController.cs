using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TheKiwiCoder;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Boss.Terrapupa
{
    public class PositionEventPayload : IBaseEventPayload
    {
        private Vector3 position;

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
    }

    public class TerrapupaController : MonoBehaviour
    {
        [SerializeField] private List<BehaviourTree> treeList = new List<BehaviourTree>();
        [SerializeField] private BehaviourTreeInstance behaviourTreeInstance;
        [SerializeField] private TerrapupaDataInfo data;

        [SerializeField] private Transform target;
        [SerializeField] private Transform rightHand;

        public Transform Target
        {
            get { return target; }
            set { target = value; }
        }

        public Transform RightHand
        {
            get { return rightHand; }
            set { rightHand = value; }
        }

        public BlackboardKey<Vector3> targetPosition;
        public BlackboardKey<int> currentHP;
        public BlackboardKey<float> moveSpeed;
        public BlackboardKey<bool> canThrowStone;
        public BlackboardKey<bool> canEarthQuake;
        public BlackboardKey<bool> canRoll;
        public BlackboardKey<bool> canLowAttack;
        public BlackboardKey<IBaseEventPayload> throwStonePayload;

        private void Start()
        {
            InitStatus();
        }

        private void Update()
        {
            targetPosition.value = target.position;
        }

        private void InitStatus()
        {
            PositionEventPayload payload = new PositionEventPayload();
            payload.Position = rightHand.position;

            behaviourTreeInstance.SetBlackboardValue<Vector3>("targetPosition", target.position);
            behaviourTreeInstance.SetBlackboardValue<int>("currentHP", data.hp);
            behaviourTreeInstance.SetBlackboardValue<float>("moveSpeed", data.movementSpeed);
            behaviourTreeInstance.SetBlackboardValue<bool>("canThrowStone", true);
            behaviourTreeInstance.SetBlackboardValue<bool>("canEarthQuake", true);
            behaviourTreeInstance.SetBlackboardValue<bool>("canRoll", true);
            behaviourTreeInstance.SetBlackboardValue<bool>("canLowAttack", true);
            behaviourTreeInstance.SetBlackboardValue<IBaseEventPayload>("throwStonePayload", payload);

            targetPosition = behaviourTreeInstance.FindBlackboardKey<Vector3>("targetPosition");
            currentHP = behaviourTreeInstance.FindBlackboardKey<int>("currentHP");
            moveSpeed = behaviourTreeInstance.FindBlackboardKey<float>("moveSpeed");
            canThrowStone = behaviourTreeInstance.FindBlackboardKey<bool>("canThrowStone");
            canEarthQuake = behaviourTreeInstance.FindBlackboardKey<bool>("canEarthQuake");
            canRoll = behaviourTreeInstance.FindBlackboardKey<bool>("canRoll");
            canLowAttack = behaviourTreeInstance.FindBlackboardKey<bool>("canLowAttack");
            throwStonePayload = behaviourTreeInstance.FindBlackboardKey<IBaseEventPayload>("throwStonePayload");
        }
    }
}