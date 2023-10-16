using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : AbstractAttack
{
    
    private string projectilePrefabPath = "Assets/Prefabs/Attacks/";
    [SerializeField] public Projectile projectile;
    private Vector3 offset;
    private Vector3 direction;
    //ToDo : Add Direction to Projectile Attack! 

    public override void InitializeProjectile(float attackValue, float durationTime, float attackInterval, float attackRange, Vector3 offset, GameObject prefabObject)
    {
        InitializedBase(attackValue, durationTime, attackInterval, attackRange);
        this.offset = offset;
        projectile = prefabObject.GetComponent<Projectile>();
        owner = gameObject.tag.ToString();
    }

    public override void ActivateAttack()
    {
        Projectile obj = Instantiate(projectile, transform.position+offset, transform.rotation);
        obj.SetProjectileData(attackValue,durationTime, gameObject.tag.ToString());
        StartCoroutine(StartAttackReadyCount());
    }

    private IEnumerator StartAttackReadyCount()
    {
        isAttackReady = false;
        yield return new WaitForSeconds(AttackInterval);
        isAttackReady = true;
    }

}
