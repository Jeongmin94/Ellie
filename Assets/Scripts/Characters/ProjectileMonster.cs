using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMonster : AbstractMonster
{
    //For Test
    public GameObject Player;

    [SerializeField] GameObject projectile;
    private enum SkillName {ProjectileAttack, End}
    private AbstractAttack[] Skills;
    private Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        InitializeStat(10, 2, 10, 50, 50); //몬스터에 대한 초기화

        Skills = new AbstractAttack[(int)SkillName.End];
        Skills[(int)SkillName.ProjectileAttack] = AddSkill(SkillName.ProjectileAttack.ToString(), Enums.AttackSkill.ProjectileAttack);
        Skills[(int)SkillName.ProjectileAttack].InitializeProjectile //공격에 대한 초기화
            (2.0f, 5.0f, 5.0f, Vector3.up * 1.0f, projectile);
    }

    private void FixedUpdate()
    {
        PlayerDistance = Vector3.Distance(Player.transform.position, transform.position);
        if (!isAttacking)
        {
            StartCoroutine(UpdateAttackDistance());
        }
    }

    private IEnumerator UpdateAttackDistance()
    {
        for(int i=0; i<(int)SkillName.End;i++)
        {
            if (PlayerDistance < Skills[i].AttackRange && Skills[i].isAttackReady)
            {
                isAttacking = true;
                animator.SetTrigger("ProjectileAttackAnimation");
                yield return new WaitForSeconds(1.5f);
                Skills[i].ActivateAttack();
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
