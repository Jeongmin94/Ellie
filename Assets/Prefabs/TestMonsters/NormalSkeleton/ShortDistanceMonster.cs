using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortDistanceMonster : AbstractMonster
{
    private enum SkillName {ShortDistanceAttack, End}
    private AbstractAttack[] Skills;
    private Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        InitializeStat(10,2,10, 30, 50);

        Skills = new AbstractAttack[(int)SkillName.End];
        Skills[(int)SkillName.ShortDistanceAttack] =AddSkill(SkillName.ShortDistanceAttack.ToString(), AttackSkill.BoxCollider);
        Skills[(int)SkillName.ShortDistanceAttack].InitializeBoxCollider
            (2, 0.5f, 0.5f, 1.0f, Vector3.one, Vector3.forward);
       
    }
    private void FixedUpdate()
    {
        PlayerDistance= Vector3.Distance(Player.GetPlayerPosition(), transform.position);
        if (!Attacking)
        {
            StartCoroutine(UpdateAttackDistance());
            if (PlayerDistance < Skills[(int)SkillName.ShortDistanceAttack].AttackRange)
            {
                animator.SetTrigger("WalkingAnimation");
            }
            else if(PlayerDistance<DetectPlayerDistance)
            {
                ChasePlayer();
                animator.SetTrigger("WalkingAnimation");
            }
            else
            {
                animator.SetTrigger("IdleAnimation");
            }
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
                animator.SetTrigger("ShortDistanceAttackAnimation");
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
