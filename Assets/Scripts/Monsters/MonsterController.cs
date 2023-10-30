
using UnityEngine;
using System.Collections;

using TheKiwiCoder;
using Assets.Scripts.Monsters.Utility;
using Assets.Scripts.Monsters.Others;
using Assets.Scripts.Monsters.AbstractClass;


using UnityEngine.AI;
using System.Collections.Generic;

using Assets.Scripts.Data;

namespace Assets.Scripts.Monsters
{


    public class MonsterController : AbstractMonster
    {
        //Temp
        public GameObject player;


        [SerializeField] public SkeletonMonsterData monsterData;
        [SerializeField] public RunToPlayerAttackData runToPlayerData;

        [SerializeField] public BoxColliderAttackData meleeAttackData;
        [SerializeField] public WeaponAttackData weaponAttackData;
        [SerializeField] public ProjectileAttackData projectileAttackData;
        [SerializeField] public FleeSkillData fleeSkilldata;

        public PatrolPoints patrolPoints;

        public enum SkillName { MeleeAttack, End }

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
            behaviourTreeInstance.SetBlackboardValue<float>("MovementSpeed", monsterData.movementSpeed);

            if (meleeAttackData != null)
            {
                behaviourTreeInstance.SetBlackboardValue<float>("MeleeAnimationHold", meleeAttackData.animationHold);
                behaviourTreeInstance.SetBlackboardValue<float>("MeleeAttackableDistance", meleeAttackData.attackableDistance);
                behaviourTreeInstance.SetBlackboardValue<float>("MeleeAttackInterval", meleeAttackData.attackInterval);
            }
            if (weaponAttackData != null)
            {
                behaviourTreeInstance.SetBlackboardValue<float>("WeaponAnimationHold", weaponAttackData.animationHold);
                behaviourTreeInstance.SetBlackboardValue<float>("WeaponAttackableDistance", weaponAttackData.attackableDistance);
                behaviourTreeInstance.SetBlackboardValue<float>("WeaponAttackInterval", weaponAttackData.attackInterval);
            }
            if (projectileAttackData != null)
            {
                behaviourTreeInstance.SetBlackboardValue<float>("ProjectimeAnimationHold", projectileAttackData.animationHold);
                behaviourTreeInstance.SetBlackboardValue<float>("projectileAttackableDistance", projectileAttackData.attackableMinimumDistance);
                behaviourTreeInstance.SetBlackboardValue<float>("ProjectileAttackInterval", projectileAttackData.attackInterval);
            }
            if (runToPlayerData != null)
            {
                behaviourTreeInstance.SetBlackboardValue<float>("RunAttackMinimumDistance", runToPlayerData.activateMinimumDistance);
                behaviourTreeInstance.SetBlackboardValue<float>("RunAttackInterval", runToPlayerData.attackInterval);
            }
            if(fleeSkilldata!=null)
            {
                behaviourTreeInstance.SetBlackboardValue<float>("ActivatableFleeDistance", fleeSkilldata.activatableDistance);
                behaviourTreeInstance.SetBlackboardValue<float>("ActivateFleeInterval", fleeSkilldata.activateInterval);
                behaviourTreeInstance.SetBlackboardValue<float>("FleeDistance", fleeSkilldata.fleeDistance);
                behaviourTreeInstance.SetBlackboardValue<float>("FleeSpeed", fleeSkilldata.fleeSpeed);
            }
        }

        private void SetNavMesh()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.speed = monsterData.movementSpeed;
            agent.angularSpeed = monsterData.rotationSpeed;
            agent.stoppingDistance = monsterData.stopDistance;
            agent.baseOffset = -0.1f;
        }

        private void AddSkills()
        {

        }
    }
}

