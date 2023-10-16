using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMonster : AbstractMonster
{
    //For Test
    public GameObject player;

    //초기화 쉽게 하기 위해 구조체 새로 생
    Structures.MonsterStat stat = new()
    {
        HP = 10.0f,
        movementSpeed = 2.0f,
        rotationSpeed = 3.0f,
        detectPlayerDistance = 10.0f,
        overtravelDistance = 15.0f
    };

    [SerializeField] GameObject projectile;
    private enum SkillName {ProjectileAttack, Melee, End}

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        //InitializeStat(10, 2, 10, 50, 50); //몬스터에 대한 초기화
        InitializeStat(stat);

        skills = new AbstractAttack[(int)SkillName.End];
        skills[(int)SkillName.ProjectileAttack] = AddSkill(SkillName.ProjectileAttack.ToString(), Enums.AttackSkill.ProjectileAttack);
        skills[(int)SkillName.ProjectileAttack].InitializeProjectile //공격에 대한 초기화
            (2.0f, 5.0f, 5.0f,5.0f, Vector3.up * 1.0f, projectile);
    }

    private void FixedUpdate()
    {
        playerDistance = Vector3.Distance(player.transform.position, transform.position);
        if (!isAttacking)
        {
            StartCoroutine(UpdateAttackDistance());
        }
    }

    private IEnumerator UpdateAttackDistance()
    {
        for(int i=0; i<(int)SkillName.End;i++)
        {
            if (playerDistance < skills[i].AttackRange && skills[i].isAttackReady)
            {
                isAttacking = true;
                animator.SetTrigger("ProjectileAttackAnimation");
                yield return new WaitForSeconds(1.5f);
                skills[i].ActivateAttack();
                break;
            }
            //To Do : 개별 공격 시간 계산하여 attacking false 체크 필요.
        }
        StartCoroutine(SetAttackingFalse());
    }

    private IEnumerator SetAttackingFalse()
    {
        isAttacking = false;
        yield return new WaitForSeconds(1.5f);
    }
}
