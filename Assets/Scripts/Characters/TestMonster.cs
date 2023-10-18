using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonster : AbstractMonster
{
    private enum SkillName { BoxAttack, ProjectileAttack, SphereAttack,WeaponAttack }
    [SerializeField] GameObject projectile;

    private void Start()
    {
        AddSkill(SkillName.ProjectileAttack.ToString(), Enums.AttackSkill.ProjectileAttack);
        AddSkill(SkillName.BoxAttack.ToString(), Enums.AttackSkill.BoxCollider);
        AddSkill(SkillName.SphereAttack.ToString(), Enums.AttackSkill.SphereCollider);

        AbstractAttack attack;
        if (Attacks.TryGetValue(SkillName.ProjectileAttack.ToString(), out attack))
        {
            attack.InitializeProjectile(10.0f,3.0f,10.0f,5.0f,Vector3.zero, projectile);
        }
        if (Attacks.TryGetValue(SkillName.BoxAttack.ToString(), out attack))
        {
            Debug.Log("Collider seted");
            attack.InitializeBoxCollider(4.0f, 1.0f,1.5f,1.0f, new Vector3(1, 1, 1), Vector3.forward * 1.0f);
        }
        if (Attacks.TryGetValue(SkillName.SphereAttack.ToString(), out attack))
        {
            Debug.Log("SphereCollider Seted");
            attack.InitializeSphereCollider(3.0f, 2.0f,5.0f,10.0f, 5.0f, Vector3.forward * 5.0f);
        }

    }
}
