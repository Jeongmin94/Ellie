using System.Collections;
using Assets.Scripts.Monsters.AbstractClass;
using Assets.Scripts.Monsters.Others;
using UnityEngine;


namespace Assets.Scripts.Monsters.Attacks
{
    public class ProjectileAttack : AbstractAttack
    {

        private string projectilePrefabPath = "Assets/Prefabs/Attacks/";
        private Projectile projectile;
        private Vector3 offset;
        private Vector3 direction;
        //ToDo : Add Direction to Projectile Attack! 

        //public override void InitializeProjectile(float attackValue, float durationTime, float attackInterval, float attackRange, Vector3 offset, GameObject prefabObject)
        //{
        //    InitializedBase(attackValue, durationTime, attackInterval, attackRange);
        //    this.offset = offset;
        //    projectile = prefabObject.GetComponent<Projectile>();
        //    owner = gameObject.tag.ToString();
        //}
        public override void InitializeProjectile(ProjectileAttackData data)
        {
            InitializedBase(data.attackValue, data.attackDuration, data.attackInterval, data.attackableDistance);
            this.offset = data.offset;
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