using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShortDistanceMonster : AbstractMonster
{
    //For Test
    public GameObject player;

    Structures.MonsterStat stat = new()
    {
        HP = 10.0f,
        movementSpeed = 2.0f,
        rotationSpeed = 3.0f,
        detectPlayerDistance = 10.0f,
        overtravelDistance = 15.0f
    };

    private enum SkillName {ShortDistanceAttack, End}

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        //InitializeStat(10,2,10, 30, 50);
        InitializeStat(stat);

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
