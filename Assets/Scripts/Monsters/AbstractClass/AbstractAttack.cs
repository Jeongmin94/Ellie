using Assets.Scripts.Combat;
using Assets.Scripts.Monsters.Utility;
using UnityEngine;

namespace Assets.Scripts.Monsters.AbstractClass
{

    public abstract class AbstractAttack : MonoBehaviour, ICombatant
    {
        protected float attackValue;
        protected float durationTime; 

        public float AttackableDistance { get; private set; }
        public float AttackInterval { get; private set; }

        public bool IsAttackReady { get; protected set; }

        protected string owner;
        protected string prefabName;

        public abstract void ActivateAttack();

        //Initializes
        protected void InitializedBase(float attackValue, float durationTime
            , float attackInterval, float attackRange)
        {
            this.attackValue = attackValue;
            this.durationTime = durationTime;
            AttackInterval = attackInterval;
            AttackableDistance = attackRange;

            IsAttackReady = true;
            owner = transform.parent.gameObject.tag;
        }

        public virtual void InitializeBoxCollider(BoxColliderAttackData data)
        { }

        public virtual void InitializeSphereCollider(SphereColliderAttackData data)
        { }

        public virtual void InitializeProjectile(ProjectileAttackData data)
        { }

        public virtual void InitializeWeapon(WeaponAttackData data)
        { }

        public virtual void InitializeAOE(AOEAttackData data)
        { }

        public virtual void Attack(IBaseEventPayload payload) { }
        //change it to abstract method after done all attack type

        public virtual void ReceiveDamage(IBaseEventPayload payload)
        { }
    }

}