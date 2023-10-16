using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : AbstractAttack
{
    private string ProjectilePrefabPath = "Assets/Prefabs/Attacks/";
    [SerializeField] public Projectile projectile;
    private Vector3 Offset;

    public override void InitializeProjectile(float attackValue, float attackInterval,float attackRange, Vector3 offset, GameObject projectilePrefab)
    {
        InitializedBase(attackValue, 0, attackInterval, attackRange);
        Offset = offset;
        projectile = projectilePrefab.GetComponent<Projectile>();
        Owner = gameObject.tag.ToString();
    }

    public override void ActivateAttack()
    {
        Projectile obj = Instantiate(projectile, transform.position+Offset, transform.rotation);
        obj.SetProjectileData(AttackValue, gameObject.tag.ToString());
        StartCoroutine(StartAttackReadyCount());
    }

    private IEnumerator StartAttackReadyCount()
    {
        isAttackReady = false;
        yield return new WaitForSeconds(AttackInterval);
        isAttackReady = true;
    }

}
