using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(parent ==null)
        {
            GameObject highestParent = Functions.FindHighestParent(gameObject);
            parent = Functions.FindChildByName(highestParent, "WeaponAttack").GetComponent<WeaponAttack>();
        }
        parent.OnWeaponTriggerEnter(other);
    }
}
