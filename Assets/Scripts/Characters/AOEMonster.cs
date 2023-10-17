using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEMonster : AbstractMonster
{
    //For Test
    public GameObject player;

    Structures.MonsterStat stat = new()
    {
        HP = 10.0f,
        movementSpeed = 2.0f,
        rotationSpeed = 3.0f,
        detectPlayerDistance = 10.0f,
        overtravelDistance=15.0f
    };

    private enum SkillName { AOEAttack, End}

    [SerializeField] private GameObject AOEPrefab;
    private Vector3 AOEPosition;

    private void Start()
    {
        animator = GetComponent<Animator>();
        //InitializeStat(10, 2, 1, 1, 1);
        InitializeStat(stat);

        skills = new AbstractAttack[(int)SkillName.End];
        skills[(int)SkillName.AOEAttack] = AddSkill(SkillName.AOEAttack.ToString(), Enums.AttackSkill.AOEAttack);
        skills[(int)SkillName.AOEAttack].InitializeAOE
            (3.3f, 3.0f, 7.0f, 10.0f, 1.0f, AOEPrefab);
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(player.transform.position, transform.position);
        if(!isAttacking)
        {
            AOEPosition = player.transform.position;
            AOEPosition.y = 0;
            skills[(int)SkillName.AOEAttack].GetComponent<AOEPrefabAttack>().SetPrefabPosition(AOEPosition,Vector3.zero);
            StartCoroutine(UpdateAttackDistance());
        }
    }

    private IEnumerator UpdateAttackDistance()
    {
        for (int i = 0; i < (int)SkillName.End; i++)
        {
            if (playerDistance < skills[i].AttackRange && skills[i].IsAttackReady)
            {
                isAttacking = true;
                animator.SetTrigger("AOEAttackAnimation");
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
