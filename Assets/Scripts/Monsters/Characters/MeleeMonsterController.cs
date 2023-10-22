
using UnityEngine;
using System.Collections;

using TheKiwiCoder;
using Assets.Scripts.Monsters.Utility;
using Assets.Scripts.Monsters.Others;
using Assets.Scripts.Monsters.AbstractClass;
using UnityEngine.AI;

namespace Assets.Scripts.Monsters.Characters
{

    public class MeleeMonsterController : AbstractMonster
    {
        //Temp
        public GameObject player;

        private enum SkillName { MeleeAttack, End }
           
        private void Start()
        {
            monsterData = new()
            {
                HP = 10.0f,
                movementSpeed = 2.0f,
                rotationSpeed = 180.0f,
                detectPlayerDistance = 15.0f,
                chasePlayerDistance = 10.0f,
                overtravelDistance = 20.0f,
            };            

            SetNavMesh();
            AddSkills();

            SetBehaviourTreeInstance();
        }

        private void SetBehaviourTreeInstance()
        {
            behaviourTreeInstance = GetComponent<BehaviourTreeInstance>();

            //Temp
            behaviourTreeInstance.SetBlackboardValue<GameObject>("Player", player);
            //<<

            monsterData.spawnPosition = transform.position;
            behaviourTreeInstance.SetBlackboardValue<float>("DetectPlayerDistance", monsterData.detectPlayerDistance);
            behaviourTreeInstance.SetBlackboardValue<Vector3>("SpawnPosition", monsterData.spawnPosition);
            behaviourTreeInstance.SetBlackboardValue<float>("DetectChaseDistance", monsterData.chasePlayerDistance);
            behaviourTreeInstance.SetBlackboardValue<float>("OvertravelDistance", monsterData.overtravelDistance);

            GameObject obj = Functions.FindChildByName(gameObject, "PlayerDetect");
            behaviourTreeInstance.SetBlackboardValue<GameObject>("DetectPlayerAI", obj);
            obj.GetComponent<DistanceDetectedAI>().SetDetectDistance(monsterData.detectPlayerDistance);

            obj = Functions.FindChildByName(gameObject, "ChasePlayer");
            behaviourTreeInstance.SetBlackboardValue<GameObject>("DetectChaseAI", obj);
            obj.GetComponent<DistanceDetectedAI>().SetDetectDistance(monsterData.chasePlayerDistance);

            obj = Functions.FindChildByName(gameObject, SkillName.MeleeAttack.ToString());
            behaviourTreeInstance.SetBlackboardValue<GameObject>("MeleeAttack", obj);
            obj.GetComponent<AbstractAttack>().InitializeBoxCollider(2.0f, 0.5f, 3.0f, 1.0f, new Vector3(1, 1, 1), Vector3.forward * 1.0f);
        }

        private void SetNavMesh()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.speed = monsterData.movementSpeed;
            agent.angularSpeed = monsterData.rotationSpeed;
            agent.stoppingDistance = 1.0f;
            agent.baseOffset = -0.1f;
        }

        private void AddSkills()
        {
            //monsterData랑 Abstract클래스랑 분류할것
            monsterData.skills = new AbstractAttack[(int)SkillName.End];
            monsterData.skills[(int)SkillName.MeleeAttack] = AddSkill(SkillName.MeleeAttack.ToString(), Enums.AttackSkill.BoxCollider);
            monsterData.skills[(int)SkillName.MeleeAttack].InitializeBoxCollider
                (2.0f, 0.5f, 2.0f, 2.0f, Vector3.one, Vector3.forward);
        }
    }
}

