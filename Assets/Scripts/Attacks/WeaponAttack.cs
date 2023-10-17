using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : AbstractAttack
{
    [SerializeField] private Collider collider;

    public override void InitializeWeapon(float attackValue, float durationTime, float attackInterval, float attackRange, GameObject weapon)
    {
        InitializedBase(attackValue, durationTime, attackInterval, attackRange);
        owner = Functions.FindHighestParent(gameObject).tag.ToString();

        if (collider == null)
        {
            collider = weapon.GetComponent<Collider>();
            if (collider == null) Debug.Log("[WeaponAttack] CanNotFindCollider");
            collider.isTrigger = true;
        }
        collider.enabled = false;
    }

    public override void ActivateAttack()
    {
        //Debug.Log("[WeaponAttack] ActivateAttack");
        if (!IsAttackReady) return;
        if (collider == null)
        {
            collider = gameObject.GetComponent<Collider>();
            collider.enabled = false;
            if (collider == null) Debug.Log("[WeaponAttack] CanNotFindCollider");
        }
        collider.enabled = true;
        StartCoroutine(DisableCollider());
        IsAttackReady = false;
    }

    private IEnumerator DisableCollider()
    {
        //Debug.Log("[WeaponAttack] DisableCollider()");
        yield return new WaitForSeconds(durationTime);
        collider.enabled = false;
        StartCoroutine(SetAttackReady());
    }

    private IEnumerator SetAttackReady()
    {
        yield return new WaitForSeconds(AttackInterval);
        IsAttackReady = true;
        
    }

    public void OnWeaponTriggerEnter(Collider other)
    {
        if (owner == "Monster")
        {
            if (other.tag == "Player")
            {
                other.gameObject.GetComponent<Player>().Damaged(attackValue);
            }
        }
    }

}
