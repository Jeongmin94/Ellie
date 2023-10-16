using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMonster : AbstractMonster
{
    //For Test
    public GameObject player;

    private enum SkillName {WeaponAttack, End}
    [SerializeField] private GameObject weapon;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        InitializeStat(10, 2, 10, 30, 40);

        skills = new AbstractAttack[(int)SkillName.End];
        skills[(int)SkillName.WeaponAttack] = AddSkill(SkillName.WeaponAttack.ToString(), Enums.AttackSkill.WeaponAttack);
        skills[(int)SkillName.WeaponAttack].InitializeWeapon(2.2f, 1.0f, 3.0f, 3.0f, weapon);
    }   
    private void FixedUpdate()
    {
        playerDistance = Vector3.Distance(player.transform.position, transform.position);
        if(!isAttacking)
        {
            StartCoroutine(UpdateAttackable());
        }
    }
    private IEnumerator UpdateAttackable()
    {
        for(int i=0; i<(int)SkillName.End;i++)
        {
            if (playerDistance < skills[i].AttackRange && skills[i].isAttackReady)
            {
                isAttacking = true;
                animator.SetTrigger("WeaponAttackAnimation");
                yield return new WaitForSeconds(1.5f);
                skills[i].ActivateAttack();
            }
        }
        if (isAttacking)
            StartCoroutine(SetAttackingFalse());
    }
    private IEnumerator SetAttackingFalse()
    {
        isAttacking = false;
        yield return new WaitForSeconds(1.5f);
    }

}
