using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

using Assets.Scripts.Monsters.Utility;

[System.Serializable]
public class AddWeaponAttack : ActionNode
{
    public NodeProperty<string> attackName;
    public NodeProperty<string> weaponPrefabName;

    protected override void OnStart() {
        GameObject obj = Functions.FindChildByName(context.gameObject, weaponPrefabName.Value);
        context.controller.weaponAttackData.weapon = obj;

    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        context.controller.AddSkills(attackName.Value, Enums.AttackSkill.WeaponAttack);
        context.controller.Attacks[attackName.Value].InitializeWeapon
            (context.controller.weaponAttackData);
        
        return State.Success;
    }
}
