using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Combat;
using Assets.Scripts.Monsters.Attacks;
using Assets.Scripts.Monsters.Utility;
using UnityEngine;

namespace Assets.Scripts.Monsters.Others
{
    public class Weapon : MonoBehaviour
    {
        public MonsterController monster;
        private WeaponAttack weaponAttack;

        private void Start()
        {
            weaponAttack = Functions.FindChildByName(monster.gameObject, monster.weaponAttackData.attackName).GetComponent<WeaponAttack>();
        }

        private void OnTriggerEnter(Collider other)
        {
            weaponAttack.OnWeaponTriggerEnter(other);
        }
    }

}