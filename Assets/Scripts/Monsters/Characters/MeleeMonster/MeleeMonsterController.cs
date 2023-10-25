
using UnityEngine;
using System.Collections;

using TheKiwiCoder;
using Assets.Scripts.Monsters.Utility;
using Assets.Scripts.Monsters.Others;
using Assets.Scripts.Monsters.AbstractClass;


using UnityEngine.AI;
using System.Collections.Generic;

using Assets.Scripts.Data;

namespace Assets.Scripts.Monsters.Characters.MeleeMonster
{

    public class MeleeMonsterController : AbstractMonster
    {
        //Temp
        public GameObject player;


        [SerializeField] public SkeletonMonsterData monsterData;
        [SerializeField] public MeleeAttackData meleeAttackData;
        [SerializeField] public RunToPlayerAttackData runToPlayerData;

        public enum SkillName { MeleeAttack, End }
        public List<GameObject> patrolPoints;        

        private void Awake()
        {
            behaviourTreeInstance = GetComponent<BehaviourTreeInstance>();
        }

        private void Start()
        {
            SetNavMesh();
            AddSkills();

            SetBehaviourTreeInstance();
        }

        private void SetBehaviourTreeInstance()
        {
            //Temp
            behaviourTreeInstance.SetBlackboardValue<GameObject>("Player", player);
            //<<

            GameObject obj = Functions.FindChildByName(gameObject, "ChasePlayer");
            behaviourTreeInstance.SetBlackboardValue<GameObject>("DetectChaseAI", obj);
            obj.GetComponent<DistanceDetectedAI>().SetDetectDistance(monsterData.chasePlayerDistance);

            obj = Functions.FindChildByName(gameObject, "PlayerDetect");
            behaviourTreeInstance.SetBlackboardValue<GameObject>("DetectPlayerAI", obj);
            obj.GetComponent<DistanceDetectedAI>().SetDetectDistance(monsterData.detectPlayerDistance);
                        
            obj = Functions.FindChildByName(gameObject, "PatrolPoints");
            behaviourTreeInstance.SetBlackboardValue<GameObject>("PatrolPoints", obj);

            behaviourTreeInstance.SetBlackboardValue<Vector3>("SpawnPosition", transform.position);

            behaviourTreeInstance.SetBlackboardValue<float>("MeleeAnimationHold", meleeAttackData.animationHold);
            behaviourTreeInstance.SetBlackboardValue<float>("MeleeAttackableDistance", meleeAttackData.attackableDistance);
            behaviourTreeInstance.SetBlackboardValue<float>("MeleeAttackInterval", meleeAttackData.attackInterval);

            behaviourTreeInstance.SetBlackboardValue<float>("RunAttackMinimumDistance", runToPlayerData.activateMinimumDistance);
            behaviourTreeInstance.SetBlackboardValue<float>("RunAttackInterval", runToPlayerData.attackInterval);
            behaviourTreeInstance.SetBlackboardValue<float>("RunReadyAnimation", runToPlayerData.readyHold);
        }

        private void SetNavMesh()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.speed =monsterData.movementSpeed;
            agent.angularSpeed =monsterData.rotationSpeed;
            agent.stoppingDistance = monsterData.stopDistance ;
            agent.baseOffset = -0.1f;
        }

        private void AddSkills()
        {
            //monsterData랑 Abstract클래스랑 분류할것
            //monsterData.skills = new AbstractAttack[(int)SkillName.End];
            //monsterData.skills[(int)SkillName.MeleeAttack] = AddSkill(SkillName.MeleeAttack.ToString(), Enums.AttackSkill.BoxCollider);
            //monsterData.skills[(int)SkillName.MeleeAttack].InitializeBoxCollider
            //    (2.0f, 0.5f, 2.0f, 2.0f, Vector3.one, Vector3.forward);
        }
    }
}

