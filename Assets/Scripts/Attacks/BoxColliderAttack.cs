using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderAttack : AbstractAttack
{
    [SerializeField] private BoxCollider collider;

    public override void InitializeBoxCollider(float attackValue,
        float duration, float attackInterval, float attackRange, Vector3 size, Vector3 offset)
    {
        InitializedBase(attackValue, duration, attackInterval, attackRange);
        owner = gameObject.tag.ToString();

        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider>();
            collider.isTrigger = true;
        }

        collider.enabled = false;
        collider.size = size;
        gameObject.transform.localPosition = offset;
    }

    public override void ActivateAttack()
    {
        collider.enabled = true;
        //Debug.Log("BoxCollider Attacked");
        StartCoroutine(DisableCollider());
        
    }

    private IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(durationTime);
        collider.enabled = false;
        IsAttackReady = false;
        StartCoroutine(SetAttakingFalse());
    }
    private IEnumerator SetAttakingFalse()
    {
        IsAttackReady = true;
        yield return new WaitForSeconds(AttackInterval);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (owner == "Monster")
        {
            if(other.tag=="Player")
            {
                other.gameObject.GetComponent<Player>().Damaged(attackValue);
            }
        }
    }
}
