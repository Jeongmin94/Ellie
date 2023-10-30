using System.Collections;
using Assets.Scripts.Monsters.AbstractClass;
using Assets.Scripts.Monsters.Others;
using UnityEngine;


namespace Assets.Scripts.Monsters.Attacks
{
    public class ProjectileAttack : AbstractAttack
    {
        private Projectile projectile;
        private Vector3 offset;

        public override void InitializeProjectile(ProjectileAttackData data)
        {
            InitializedBase(data.attackValue, data.attackDuration, data.attackInterval, data.attackableMinimumDistance);
            offset = data.offset;
            projectile = data.projectilePrefab.GetComponent<Projectile>();
        }

        public override void ActivateAttack()
        {
            Projectile obj = Instantiate(projectile, transform.position + offset, transform.rotation);
            obj.SetProjectileData(attackValue, durationTime, gameObject.tag.ToString());
            StartCoroutine(StartAttackReadyCount());
        }

        private IEnumerator StartAttackReadyCount()
        {
            IsAttackReady = false;
            yield return new WaitForSeconds(AttackInterval);
            IsAttackReady = true;
        }

    }

}