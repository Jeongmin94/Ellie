using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShortDistanceMonsterNavMesh : AbstractMonster
{
    //For Test
    public GameObject player;

    private NavMeshAgent agent;
    private Vector3 spawnPosition;
    private bool isReturningToSpawnPosition;

    Structures.MonsterStat stat = new()
    {
        HP = 10.0f,
        movementSpeed = 2.0f,
        rotationSpeed = 60.0f,
        detectPlayerDistance = 10.0f,
        overtravelDistance = 15.0f
    };

    private enum SkillName {ShortDistanceAttack, End}

    private void Awake()
    {
        InitializeStat(stat);

        animator = gameObject.GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = monsterStat.movementSpeed;
        Debug.Log(monsterStat.movementSpeed);
        agent.angularSpeed = monsterStat.rotationSpeed;
        agent.stoppingDistance = 1.0f; //ToDo: 스킬마다 다른거 어떻게 적용할지 고민 필요
        //player.transform.position = navMesh.destination;

        //InitializeStat(10,2,10, 30, 50);

        skills = new AbstractAttack[(int)SkillName.End];
        skills[(int)SkillName.ShortDistanceAttack] =AddSkill(SkillName.ShortDistanceAttack.ToString(), Enums.AttackSkill.BoxCollider);
        skills[(int)SkillName.ShortDistanceAttack].InitializeBoxCollider
            (2, 0.5f, 2.0f, 2.0f, Vector3.one, Vector3.forward);
       
    }
    private void FixedUpdate()
    {
        playerDistance = Vector3.Distance(player.transform.position, transform.position);
        if (!isAttacking)
        {
            StartCoroutine(UpdateAttackable());
        }

        //movement
        if (isReturningToSpawnPosition)
        {
            if (Vector3.Distance(transform.position, spawnPosition) < 2.0f)
                isReturningToSpawnPosition = false;
        }
        if (playerDistance < monsterStat.detectPlayerDistance)
        {
            agent.destination = player.transform.position;
        }
        else if(playerDistance>monsterStat.overtravelDistance)
        {
            agent.destination = spawnPosition;
            isReturningToSpawnPosition = true;
        }
        Debug.Log(player.transform.position);
    }
    private IEnumerator UpdateAttackable()
    {
        //Debug.Log("[ShortDistanceMonster] UpdateAttackDistance()");
        for (int i = 0; i < (int)SkillName.End; i++)
        {
            if (playerDistance < skills[i].AttackRange && skills[i].IsAttackReady)
            {
                isAttacking = true;
                animator.SetTrigger("ShortDistanceAttackAnimation");
                yield return new WaitForSeconds(1.5f);
                skills[i].ActivateAttack();
            }
            //To Do : 개별 공격 시간 계산하여 attacking false 체크 필요.
        }
        StartCoroutine(SetAttackingFalse());
    }

    private IEnumerator SetAttackingFalse()
    {
        yield return new WaitForSeconds(1.0f);
        isAttacking = false;
    }
}
