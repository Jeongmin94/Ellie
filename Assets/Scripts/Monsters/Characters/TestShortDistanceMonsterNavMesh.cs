using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Monsters.AbstractClass;
using Assets.Scripts.Monsters.Others;
using Assets.Scripts.Monsters.Utility;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Monsters.Characters
{
    public class TestShortDistanceMonsterNavMesh : AbstractMonster
    {
        //For Test
        public GameObject player;

        private DistanceDetectedAI detectPlayer;
        private DistanceDetectedAI chasePlayer;

        private NavMeshAgent agent;
        private Vector3 spawnPosition;
        private bool isReturningToSpawnPosition;

        Structures.MonsterStat stat = new()
        {
            HP = 10.0f,
            movementSpeed = 2.0f,
            rotationSpeed = 180.0f,
            detectPlayerDistance = 20.0f,
            overtravelDistance = 15.0f
        };
        private float chasePlayerDistacne = 10.0f; //Add in to MonsterStat Structure;

        private float wanderRadius = 5.0f;
        private float wanderInterval = 3.0f;
        private float wanderAccumulateTime;

        private enum SkillName { ShortDistanceAttack, End }

        private void Awake()
        {
            InitializeStat(stat);

            //Movement Initialize
            spawnPosition = new Vector3(-3.0f, 0.0f, -3.0f);
            chasePlayer = Functions.FindChildByName(gameObject, "ChasePlayer").GetComponent<DistanceDetectedAI>();
            detectPlayer = Functions.FindChildByName(gameObject, "DetectPlayer").GetComponent<DistanceDetectedAI>();

            detectPlayer.SetDetectDistance(stat.detectPlayerDistance);
            chasePlayer.SetDetectDistance(chasePlayerDistacne);

            isReturningToSpawnPosition = false;

            animator = gameObject.GetComponent<Animator>();

            agent = GetComponent<NavMeshAgent>();
            agent.speed = monsterStat.movementSpeed;
            agent.angularSpeed = monsterStat.rotationSpeed;
            agent.stoppingDistance = 1.0f; //ToDo: 스킬마다 다른거 어떻게 적용할지 고민 필요

            skills = new AbstractAttack[(int)SkillName.End];
            skills[(int)SkillName.ShortDistanceAttack] = AddSkill(SkillName.ShortDistanceAttack.ToString(), Enums.AttackSkill.BoxCollider);
            skills[(int)SkillName.ShortDistanceAttack].InitializeBoxCollider
                (2, 0.5f, 2.0f, 2.0f, Vector3.one, Vector3.forward);

        }
        private void FixedUpdate()
        {
            if (isAttacking)
            {
                Debug.Log("IsAttacking");
                return;
            }

            if (isReturningToSpawnPosition) //Returnning to spawn Pos and Returned
            {
                agent.destination = spawnPosition;
                if (Vector3.Distance(transform.position, spawnPosition) < 0.5f)
                {
                    isReturningToSpawnPosition = false;
                    animator.SetTrigger("IdleAnimation");
                }
            }


            if (!isReturningToSpawnPosition)
            {
                travelDistance = Vector3.Distance(spawnPosition, transform.position);
                if (travelDistance > stat.overtravelDistance)
                {
                    isReturningToSpawnPosition = true;
                }
                else if (chasePlayer.IsDetected)
                {
                    animator.SetTrigger("WalkingAnimation");
                    StartCoroutine(UpdateAttackable());
                    agent.destination = player.transform.position;
                }
                else if (detectPlayer.IsDetected)
                {
                    Debug.Log("MonsterWandering");
                    MonsterWandering();
                }
                else if (!detectPlayer.IsDetected)
                {
                    agent.destination = spawnPosition;
                    isReturningToSpawnPosition = true;
                }
            }


        }

        private IEnumerator UpdateAttackable()
        {
            playerDistance = Vector3.Distance(player.transform.position, transform.position);
            for (int i = 0; i < (int)SkillName.End; i++)
            {
                if (playerDistance < skills[i].AttackRange && skills[i].IsAttackReady)
                {
                    isAttacking = true;
                    animator.SetTrigger("ShortDistanceAttackAnimation");
                    yield return new WaitForSeconds(1.5f);
                    skills[i].ActivateAttack();
                    StartCoroutine(SetAttackingFalse());
                }
                //To Do : 개별 공격 시간 계산하여 attacking false 체크 필요.
            }
        }

        private IEnumerator SetAttackingFalse()
        {
            yield return new WaitForSeconds(1.5f);
            isAttacking = false;
        }

        private void MonsterWandering()
        {
            wanderAccumulateTime += Time.deltaTime;
            if (wanderAccumulateTime >= wanderInterval)
            {
                Vector3 wanderPosition = Random.insideUnitSphere * wanderRadius;
                wanderPosition += spawnPosition;
                agent.destination = wanderPosition;
                wanderAccumulateTime = 0;
            }
        }
    }

}