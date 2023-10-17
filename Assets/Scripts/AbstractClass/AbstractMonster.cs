using System.Collections.Generic;
using UnityEngine;

//Check : 공통된 부분은 구조체 만들어서 전달하는건 어떨까마

public abstract class AbstractMonster : MonoBehaviour
{
    //Monster Stat, Type
    protected Structures.MonsterStat monsterStat;
    protected Structures.MonsterType monsterType;

    //PlayerDistance
    protected float playerDistance;

    //Actions
    protected bool isAttacking;

    //Components
    private Rigidbody rigidBody;
    protected AbstractAttack[] skills;
    protected Animator animator;

    //Attack Dictionary
    protected Dictionary<string, AbstractAttack> Attacks = new();


    // >> : Functions
    protected void InitializeStat(Structures.MonsterStat monsterStat)
    {
        this.monsterStat = monsterStat;
    }

    protected void InitializeType(Structures.MonsterType monsterType)
    {
        this.monsterType = monsterType;
    }
    protected AbstractAttack AddSkill(string skillName, Enums.AttackSkill attackSkill)
    {
        AbstractAttack attack = null;
        GameObject newSkill = new GameObject(skillName);
        newSkill.transform.SetParent(transform);
        newSkill.tag = gameObject.tag;
        newSkill.transform.localPosition = Vector3.zero;
        newSkill.transform.localRotation = Quaternion.Euler(Vector3.zero);
        newSkill.transform.localScale = Vector3.one;

        switch (attackSkill)
        {
            case Enums.AttackSkill.ProjectileAttack:
                attack = newSkill.AddComponent<ProjectileAttack>();
                break;
            case Enums.AttackSkill.BoxCollider:
                attack = newSkill.AddComponent<BoxColliderAttack>();
                break;
            case Enums.AttackSkill.SphereCollider:
                attack = newSkill.AddComponent<SphereColliderAttack>();
                break;
            case Enums.AttackSkill.WeaponAttack:
                attack = newSkill.AddComponent<WeaponAttack>();
                break;
            case Enums.AttackSkill.AOEAttack:
                attack = newSkill.AddComponent<AOEPrefabAttack>();
                break;
        }
        if (attack != null)
            Attacks.Add(skillName, attack);

        return attack;
    }

    //Temp -> will be replaced with navmesh
    protected void ChasePlayer()
    {
        Vector3 direction = Player.GetPlayerPosition() - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        lookRotation.x = 0;
        lookRotation.z = 0;
        rigidBody.MoveRotation(Quaternion.Slerp(rigidBody.rotation, lookRotation, monsterStat.rotationSpeed * Time.deltaTime));
        transform.Translate(Vector3.forward * monsterStat.movementSpeed * Time.deltaTime);
    }

}
