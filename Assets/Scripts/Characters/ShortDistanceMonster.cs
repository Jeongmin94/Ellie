using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortDistanceMonster : AbstractMonster
{
    //For Test
    public GameObject Player;

    private enum SkillName {ShortDistanceAttack, End}
    private AbstractAttack[] Skills;
    private Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        InitializeStat(10,2,10, 30, 50);

        Skills = new AbstractAttack[(int)SkillName.End];
        Skills[(int)SkillName.ShortDistanceAttack] =AddSkill(SkillName.ShortDistanceAttack.ToString(), Enums.AttackSkill.BoxCollider);
        Skills[(int)SkillName.ShortDistanceAttack].InitializeBoxCollider
            (2, 0.5f, 2.0f, 2.0f, Vector3.one, Vector3.forward);
       
    }
    private void FixedUpdate()
    {
        PlayerDistance = Vector3.Distance(Player.transform.position, transform.position);
        if (!isAttacking)
        {
            StartCoroutine(UpdateAttackable());
        }
    }
    private IEnumerator UpdateAttackable()
    {
        //Debug.Log("[ShortDistanceMonster] UpdateAttackDistance()");
        for (int i = 0; i < (int)SkillName.End; i++)
        {
            if (PlayerDistance < Skills[i].AttackRange && Skills[i].isAttackReady)
            {
                isAttacking = true;
                animator.SetTrigger("ShortDistanceAttackAnimation");
                yield return new WaitForSeconds(1.5f);
                Skills[i].ActivateAttack();
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
