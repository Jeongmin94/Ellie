using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAttack : MonoBehaviour
{
    [SerializeField] protected float AttackValue;
    [SerializeField] public float AttackRange { get; private set; }
    [SerializeField] protected float DurationTime;
    [SerializeField] public float AttackInterval { get; private set; }
    [SerializeField] protected bool AttackReady;

    protected string Owner;
    protected string PrefabName;

    public abstract void ActivateAttack();

    protected void InitializedBase(float attackValue, float durationTime
        , float attackInterval, float attackRange, string prefabName = "")
    {
        AttackValue = attackValue;
        DurationTime = durationTime;
        PrefabName = prefabName;
        AttackInterval = attackInterval;
        AttackRange = attackRange;
    }

    public virtual void InitializeBoxCollider(float attackValue,
        float duration, float attackInterval, float attackRange, Vector3 size, Vector3 offset, string prefabName = "")
    { }

    public virtual void InitializeSphereCollider(float attackValue, float duration,
        float attackInterval, float attackRange, float attackRadius, Vector3 offset)
    { }

    public virtual void InitializeProjectile(float attackValue, float attackInterval, float attackRange, Vector3 offset, string prefabName) { }
}
