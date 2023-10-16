using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : AbstractAttack
{
    [SerializeField] private Collider collider;

    public override void InitializeWeapon(float attackValue, float durationTime, float attackInterval, float attackRange, GameObject weapon)
    {
        InitializedBase(attackValue, durationTime, attackInterval, attackRange);
        Owner = Functions.FindHighestParent(gameObject).tag.ToString();

        if (collider == null)
        {
            collider = weapon.GetComponent<Collider>();
            if (collider == null) Debug.Log("[WeaponAttack] CanNotFindCollider");
            collider.isTrigger = true;
        }
        collider.enabled = false;
    }
    private void Update()
    {
        //Debug.Log(Owner);
    }

    public override void ActivateAttack()
    {
        //Debug.Log("[WeaponAttack] ActivateAttack");
        if (!isAttackReady) return;
        if (collider == null)
        {
            collider = gameObject.GetComponent<Collider>();
            collider.enabled = false;
            if (collider == null) Debug.Log("[WeaponAttack] CanNotFindCollider");
        }
        collider.enabled = true;
        StartCoroutine(DisableCollider());
        isAttackReady = false;
    }

    private IEnumerator DisableCollider()
    {
        //Debug.Log("[WeaponAttack] DisableCollider()");
        yield return new WaitForSeconds(DurationTime);
        collider.enabled = false;
        StartCoroutine(SetAttackReady());
    }

    private IEnumerator SetAttackReady()
    {
        yield return new WaitForSeconds(AttackInterval);
        isAttackReady = true;
        
    }

    public void OnWeaponTriggerEnter(Collider other)
    {
        if (Owner == "Monster")
        {
            if (other.tag == "Player")
            {
                other.gameObject.GetComponent<Player>().Damaged(AttackValue);
            }
        }
    }

}
