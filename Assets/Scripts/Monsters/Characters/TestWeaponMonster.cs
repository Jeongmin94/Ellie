using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Monsters.AbstractClass;
using Assets.Scripts.Monsters.Utility;
using UnityEngine;

namespace Assets.Scripts.Monsters.Characters
{
    public class TestWeaponMonster : AbstractMonster
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

        private enum SkillName { WeaponAttack, End }
        [SerializeField] private GameObject weapon;

        private void OnEnable()
        {
            animator = GetComponent<Animator>();
            //InitializeStat(10, 2, 10, 30, 40);
            InitializeStat(stat);

            skills = new AbstractAttack[(int)SkillName.End];
            skills[(int)SkillName.WeaponAttack] = AddSkill(SkillName.WeaponAttack.ToString(), Enums.AttackSkill.WeaponAttack);
            skills[(int)SkillName.WeaponAttack].InitializeWeapon(2.2f, 1.0f, 3.0f, 3.0f, weapon);
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
            for (int i = 0; i < (int)SkillName.End; i++)
            {
                if (playerDistance < skills[i].AttackRange && skills[i].IsAttackReady)
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

}