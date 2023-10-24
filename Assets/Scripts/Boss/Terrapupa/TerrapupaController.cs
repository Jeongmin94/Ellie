using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TheKiwiCoder;
using UnityEngine;

namespace Assets.Scripts.Boss.Terrapupa
{
    public class TerrapupaController : MonoBehaviour
    {
        [SerializeField] private List<BehaviourTree> treeList = new List<BehaviourTree>();
        [SerializeField] private BehaviourTreeInstance behaviourTreeInstance;
        [SerializeField] private TerrapupaDataInfo data;

        public Transform target;

        public BlackboardKey<Vector3> targetPosition;
        public BlackboardKey<int> currentHP;
        public BlackboardKey<float> moveSpeed;
        public BlackboardKey<bool> canThrowStone;
        public BlackboardKey<bool> canEarthQuake;
        public BlackboardKey<bool> canRoll;
        public BlackboardKey<bool> canLowAttack;

        private void Start()
        {
            InitStatus();
        }

        private void Update()
        {
            targetPosition.value = target.position;
        }

        public class TestEvent
        {
            public string Message { get; set; }
        }

        private void InitStatus()
        {
            behaviourTreeInstance.SetBlackboardValue<Vector3>("targetPosition", target.position);
            behaviourTreeInstance.SetBlackboardValue<int>("currentHP", data.hp);
            behaviourTreeInstance.SetBlackboardValue<float>("moveSpeed", data.movementSpeed);
            behaviourTreeInstance.SetBlackboardValue<bool>("canThrowStone", true);
            behaviourTreeInstance.SetBlackboardValue<bool>("canEarthQuake", true);
            behaviourTreeInstance.SetBlackboardValue<bool>("canRoll", true);
            behaviourTreeInstance.SetBlackboardValue<bool>("canLowAttack", true);

            targetPosition = behaviourTreeInstance.FindBlackboardKey<Vector3>("targetPosition");
            currentHP = behaviourTreeInstance.FindBlackboardKey<int>("currentHP");
            moveSpeed = behaviourTreeInstance.FindBlackboardKey<float>("moveSpeed");
            canThrowStone = behaviourTreeInstance.FindBlackboardKey<bool>("canThrowStone");
            canEarthQuake = behaviourTreeInstance.FindBlackboardKey<bool>("canEarthQuake");
            canRoll = behaviourTreeInstance.FindBlackboardKey<bool>("canRoll");
            canLowAttack = behaviourTreeInstance.FindBlackboardKey<bool>("canLowAttack");
        }
    }
}