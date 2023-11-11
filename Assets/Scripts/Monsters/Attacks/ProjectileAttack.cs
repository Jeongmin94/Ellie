using System.Collections;
using Assets.Scripts.Combat;
using Assets.Scripts.Monsters.AbstractClass;
using Assets.Scripts.Monsters.Others;
using Channels.Combat;
using UnityEngine;


namespace Assets.Scripts.Monsters.Attacks
{
    public class ProjectileAttack : AbstractAttack
    {
        private Projectile projectile;
        public ProjectileAttackData attackData;

        private Vector3 offset;

        private void Awake()
        {
            SetTicketMachine();
        }

        public override void InitializeProjectile(ProjectileAttackData data)
        {
            attackData = data;
            InitializedBase(data.attackValue, data.attackDuration, data.attackInterval, data.attackableMinimumDistance);
            offset = data.offset;
            projectile = data.projectilePrefab.GetComponent<Projectile>();
        }

        public override void ActivateAttack()
        {
            Projectile obj = Instantiate(projectile, transform.position + offset, transform.rotation);
            obj.spawner = gameObject.GetComponent<ProjectileAttack>();
            StartCoroutine(StartAttackReadyCount());
        }

        private IEnumerator StartAttackReadyCount()
        {
            IsAttackReady = false;
            yield return new WaitForSeconds(AttackInterval);
            IsAttackReady = true;
        }

        public void ProjectileHitPlayer(Transform otherTransform)
        {
            SetAndAttack(attackData, otherTransform);
        }

        private void SetAndAttack(ProjectileAttackData data, Transform otherTransform)
        {
            CombatPayload payload = new();
            payload.Type = data.combatType;
            Debug.Log("Payload Type : " + payload.Type);
            payload.Attacker = transform;
            payload.Defender = otherTransform;
            payload.AttackDirection = Vector3.zero;
            payload.AttackStartPosition = transform.position;
            payload.AttackPosition = otherTransform.position;
            payload.PlayerStatusEffectName = StatusEffects.StatusEffectName.Burn;
            payload.Damage = (int)data.attackValue;
            Attack(payload);
        }
    }

}