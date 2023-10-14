using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : AbstractAttack
{
    private string ProjectilePrefabPath = "Prefabs/";
    private Projectile projectile;
    private Vector3 Offset;

    public override void InitializeProjectile(float attackValue, float attackInterval,float attackRange, Vector3 offset, string prefabName)
    {
        InitializedBase(attackValue, 0, attackInterval, attackRange, prefabName);
        Offset = offset;
        Owner = gameObject.tag.ToString();
    }

    public override void ActivateAttack()
    {
        if (projectile == null)
        {
            projectile = Resources.Load<Projectile>(ProjectilePrefabPath + PrefabName);
        }

        Projectile obj = Instantiate(projectile, transform.position+Offset, transform.rotation);
        obj.SetProjectileData(AttackValue, gameObject.tag.ToString());
        Debug.Log("ActivateProjectile");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("ProjectileAttack"))
        {
            ActivateAttack();
        }
    }
}
