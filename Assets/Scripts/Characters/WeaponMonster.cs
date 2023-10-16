using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMonster : AbstractMonster
{
    //For Test
    public GameObject Player;

    private enum SkillName {WeaponAttack, End}
    private AbstractAttack[] Skills;
    private Animator animator;
    [SerializeField] private GameObject Weapon;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        InitializeStat(10, 2, 10, 30, 40);

        Skills = new AbstractAttack[(int)SkillName.End];
        Skills[(int)SkillName.WeaponAttack] = AddSkill(SkillName.WeaponAttack.ToString(), Enums.AttackSkill.WeaponAttack);
        Skills[(int)SkillName.WeaponAttack].InitializeWeapon(2.2f, 1.0f, 3.0f, 3.0f, Weapon);
    }
    private void FixedUpdate()
    {
        PlayerDistance = Vector3.Distance(Player.transform.position, transform.position);
        if(!isAttacking)
        {
            StartCoroutine(UpdateAttackable());
        }
    }
    private IEnumerator UpdateAttackable()
    {
        for(int i=0; i<(int)SkillName.End;i++)
        {
            if (PlayerDistance < Skills[i].AttackRange && Skills[i].isAttackReady)
            {
                isAttacking = true;
                animator.SetTrigger("WeaponAttackAnimation");
                yield return new WaitForSeconds(1.5f);
                Skills[i].ActivateAttack();
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
