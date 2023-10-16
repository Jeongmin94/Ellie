using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    WeaponAttack Parent;

    private void Start()
    {
        GameObject highestParent = Functions.FindHighestParent(gameObject);
        Parent = Functions.FindChildByName(highestParent, "WeaponAttack").GetComponent<WeaponAttack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(Parent ==null)
        {
            GameObject highestParent = Functions.FindHighestParent(gameObject);
            Parent = Functions.FindChildByName(highestParent, "WeaponAttack").GetComponent<WeaponAttack>();
        }
        Parent.OnWeaponTriggerEnter(other);
    }
}
