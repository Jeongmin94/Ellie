using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMonster : MonoBehaviour
{
    //Stat
    [SerializeField] protected float HP;
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected float rotationSpeed;
    [SerializeField] protected float detectPlayerDistance;
    [SerializeField] protected float overtravelDistance;

    //PlayerDistance
    protected float playerDistance;

    //Actions
    [SerializeField] protected bool isAttacking;

    //Type
    [SerializeField] protected Enums.MonsterKind kind;
    [SerializeField] protected Enums.MovementType type;
    [SerializeField] protected Enums.AttackTurnType turnType;

    //Components
    private Rigidbody RB;
    protected AbstractAttack[] skills;
    protected Animator animator;

    //Attack Dictionary
    protected Dictionary<string, AbstractAttack> Attacks = new();

    protected void InitializeStat(float HP, float movementSpeed, float rotationSpeed, float detectPlayerDistance, float overtravelDistance)
    {
        this.HP = HP;
        this.movementSpeed = movementSpeed;
        this.rotationSpeed = rotationSpeed;
        this.detectPlayerDistance = detectPlayerDistance;
        this.overtravelDistance = overtravelDistance;
        if(RB==null)
        {
            RB = GetComponent<Rigidbody>();
        }

        isAttacking = false;
    }

    protected void InitializeType(Enums.MonsterKind kind, Enums.MovementType type, Enums.AttackTurnType turnType)
    {
        this.kind = kind;
        this.type = type;
        this.turnType = turnType;
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
        RB.MoveRotation(Quaternion.Slerp(RB.rotation, lookRotation, rotationSpeed * Time.deltaTime));
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
    }

}
