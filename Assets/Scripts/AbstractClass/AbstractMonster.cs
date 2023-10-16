using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMonster : MonoBehaviour
{
    //Stat
    [SerializeField] protected float HP;
    [SerializeField] protected float MovementSpeed;
    [SerializeField] protected float RotationSpeed;
    [SerializeField] protected float DetectPlayerDistance;
    [SerializeField] protected float OvertravelDistance;

    //PlayerDistance
    protected float PlayerDistance;

    //Actions
    [SerializeField] protected bool isAttacking;

    //Typle
    [SerializeField] protected Enums.MonsterKind Kind;
    [SerializeField] protected Enums.MovementType Type;
    [SerializeField] protected Enums.AttackTurnType TurnType;

    //Components
    private Rigidbody RB;

    //Attack Dictionary
    protected Dictionary<string, AbstractAttack> Attacks = new();

    protected void InitializeStat(float HP, float movementSpeed, float rotationSpeed, float detectPlayerDistance, float overtravelDistance)
    {
        this.HP = HP;
        MovementSpeed = movementSpeed;
        RotationSpeed = rotationSpeed;
        DetectPlayerDistance = detectPlayerDistance;
        OvertravelDistance = overtravelDistance;
        if(RB==null)
        {
            RB = GetComponent<Rigidbody>();
        }

        isAttacking = false;
    }

    protected void InitializeType(Enums.MonsterKind kind, Enums.MovementType type, Enums.AttackTurnType turnType)
    {
        Kind = kind;
        Type = type;
        TurnType = turnType;
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
        RB.MoveRotation(Quaternion.Slerp(RB.rotation, lookRotation, RotationSpeed * Time.deltaTime));
        transform.Translate(Vector3.forward * MovementSpeed * Time.deltaTime);
        //RB.MovePosition(Vector3.forward * MovementSpeed * Time.deltaTime);
    }

}
