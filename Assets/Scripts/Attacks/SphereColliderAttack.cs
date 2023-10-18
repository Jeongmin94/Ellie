using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereColliderAttack : AbstractAttack
{
    [SerializeField] private SphereCollider collider;

    public override void InitializeSphereCollider(float attackValue, float duration,
        float attackInterval, float attackRange, float attackRadius, Vector3 offset)
    {
        InitializedBase(attackValue, duration, attackInterval, attackRange);
        owner = gameObject.tag.ToString();

        if (collider == null)
        {
            collider = gameObject.AddComponent<SphereCollider>();
            collider.isTrigger = true;
            collider.enabled = false;
        }
        gameObject.transform.localPosition = offset;
        collider.radius = attackRadius;
    }

    public override void ActivateAttack()
    {
        collider.enabled = true;
        Debug.Log("SphereColliderAttacked");
        StartCoroutine("DisableCollider");
    }
    private IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(durationTime);
        collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(owner=="Monster")
        {
            if(other.tag=="Player")
            {
                other.gameObject.GetComponent<TestPlayer>().Damaged(attackValue);
                Debug.Log("ShpereGiveDamage");
            }
        }
    }

}
