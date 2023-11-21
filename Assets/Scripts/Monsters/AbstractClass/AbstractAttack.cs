using Assets.Scripts.Combat;
using Assets.Scripts.Monsters.Utility;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Type;
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

        private TicketMachine ticketMachine;

        [SerializeField] protected MonsterAudioController audioController;
        [SerializeField] protected MonsterParticleController particleController;

        public abstract void ActivateAttack();

        protected void InitializedBase(float attackValue, float durationTime
            , float attackInterval, float attackRange)
        {
            this.attackValue = attackValue;
            this.durationTime = durationTime;
            AttackInterval = attackInterval;
            AttackableDistance = attackRange;

            IsAttackReady = true;
            owner = transform.parent.gameObject.tag;
            audioController = transform.parent.GetComponent<MonsterAudioController>();
            particleController = transform.parent.GetComponent<MonsterParticleController>();
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

        public virtual void InitializeFanShape(FanShapeAttackData data)
        { }

        public virtual void ReceiveDamage(IBaseEventPayload payload)
        { }

        protected void SetTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.Combat);
        }

        public void Attack(IBaseEventPayload payload)
        {
            Debug.Log(ticketMachine);
            ticketMachine.SendMessage(ChannelType.Combat, payload);
        }
    }

}