using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMonster : AbstractMonster
{
    private enum SkillName {ProjectileAttack, End}
    private AbstractAttack[] Skills;
    private Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        InitializeStat(10,2,10, 50, 50);

        Skills = new AbstractAttack[(int)SkillName.End];
        Skills[(int)SkillName.ProjectileAttack]=AddSkill(SkillName.ProjectileAttack.ToString(), AttackSkill.ProjectileAttack);
        Skills[(int)SkillName.ProjectileAttack].InitializeProjectile
            (2.0f, 5.0f, 5.0f,Vector3.up*1.0f, "Projectile");
       
    }
    private void FixedUpdate()
    {
        PlayerDistance = Vector3.Distance(Player.GetPlayerPosition(), transform.position);
        if (!Attacking)
        {
            StartCoroutine(UpdateAttackDistance());
        }

        if (PlayerDistance < DetectPlayerDistance)
        {
            ChasePlayer();
            animator.SetTrigger("WalkingAnimation");
        }
        else
        {
            animator.SetTrigger("IdleAnimation");
        }
    }
    private IEnumerator UpdateAttackDistance()
    {
        for(int i=0; i<(int)SkillName.End;i++)
        {
            AnimatorStateInfo animatorInfo;
            if (PlayerDistance < Skills[i].AttackRange)
            {
                Attacking = true;
                animator.SetTrigger("ProjectileAttackAnimation");
                animatorInfo = animator.GetCurrentAnimatorStateInfo(0);
                yield return new WaitForSeconds(animatorInfo.length * 0.9f);
                Skills[i].ActivateAttack();
            }
            //To Do : 개별 공격 시간 계산하여 attacking false 체크 필요.
        }
        if (Attacking)
            StartCoroutine("SetAttackingFalse");
    }
    private IEnumerator SetAttackingFalse()
    {
        yield return new WaitForSeconds(3.0f);
        Attacking = false;
    }
}
