using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Monsters.Attacks;
using Assets.Scripts.Monsters.Utility;
using UnityEngine;

namespace Assets.Scripts.Monsters.Others
{
    public class Weapon : MonoBehaviour
    {
        private WeaponAttack parent;

        private void Start()
        {
            GameObject highestParent = Functions.FindHighestParent(gameObject);
            parent = Functions.FindChildByName(highestParent, "WeaponAttack").GetComponent<WeaponAttack>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (parent == null)
            {
                GameObject highestParent = Functions.FindHighestParent(gameObject);
                parent = Functions.FindChildByName(highestParent, "WeaponAttack").GetComponent<WeaponAttack>();
            }
            parent.OnWeaponTriggerEnter(other);
        }
    }

}