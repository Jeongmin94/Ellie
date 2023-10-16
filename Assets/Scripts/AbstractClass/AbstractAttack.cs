using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAttack : MonoBehaviour
{
    [SerializeField] protected float AttackValue; //공격력
    [SerializeField] public float AttackRange { get; private set; } //공격 발동 범위
    [SerializeField] protected float DurationTime; //공격 지속 시간
    [SerializeField] public float AttackInterval { get; private set; } //공격 쿨타임(간격)
    [SerializeField] public bool isAttackReady{ get; protected set; } //공격 가능 여부 

    protected string Owner;
    protected string PrefabName;

    public abstract void ActivateAttack();

    //Initializes
    protected void InitializedBase(float attackValue, float durationTime
        , float attackInterval, float attackRange)
    {
        AttackValue = attackValue;
        DurationTime = durationTime;
        AttackInterval = attackInterval;
        AttackRange = attackRange;

        isAttackReady = true;
        //Debug.Log("[AbstractAttack]Initialized base");
    }

    public virtual void InitializeBoxCollider(float attackValue,
        float duration, float attackInterval, float attackRange, Vector3 size, Vector3 offset)
    { }

    public virtual void InitializeSphereCollider(float attackValue, float duration,
        float attackInterval, float attackRange, float attackRadius, Vector3 offset)
    { }

    public virtual void InitializeProjectile(float attackValue, float attackInterval, float attackRange, Vector3 offset, GameObject prefabName)
    { }
    //attackvalue and duration should be in AttackObject or ProjectilePrefab?

    public virtual void InitializeWeapon(float attackValue, float durationTime, float attackInterval, float attackRange, GameObject weapon)
    { }
}
