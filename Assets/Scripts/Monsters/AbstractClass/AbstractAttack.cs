using Assets.Scripts.Monsters.Utility;
using UnityEngine;

namespace Assets.Scripts.Monsters.AbstractClass
{

    public abstract class AbstractAttack : MonoBehaviour
    {
        [SerializeField] protected float attackValue; //공격력
        [SerializeField] protected float durationTime; //공격 지속 시간

        public float AttackRange { get; private set; } //공격 발동 범위
        public float AttackInterval { get; private set; } //공격 쿨타임(간격)
        public bool IsAttackReady { get; protected set; } //공격 가능 여부 

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
            AttackRange = attackRange;

            IsAttackReady = true;
            owner = Functions.FindHighestParent(gameObject).tag.ToString();
            //Debug.Log("[AbstractAttack]Initialized base");
        }

        public virtual void InitializeBoxCollider(BoxColliderAttackData data)
        { }

        public virtual void InitializeSphereCollider
            (float attackValue, float duration, float attackInterval, float attackRange, float attackRadius, Vector3 offset)
        { }

        public virtual void InitializeProjectile(ProjectileAttackData data)
        { }
        //public virtual void InitializeProjectile
        //    (float attackValue, float durationTime, float attackInterval, float attackRange, Vector3 offset, GameObject prefabObject)
        //{ }

        public virtual void InitializeWeapon(WeaponAttackData data)
        { }

        public virtual void InitializeAOE
            (float attackValue, float durationTime, float attackInterval, float attackRange, float damageInterval, GameObject prefabObject)
        { }
    }

}