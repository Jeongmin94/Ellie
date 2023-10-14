using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderAttack : AbstractAttack
{
    [SerializeField] private BoxCollider collider;

    public override void InitializeBoxCollider(float attackValue,
        float duration, float attackInterval, float attackRange, Vector3 size, Vector3 offset, string prefabName = "")
    {
        InitializedBase(attackValue, duration, attackInterval, attackRange, prefabName);
        Owner = gameObject.tag.ToString();

        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider>();
            collider.isTrigger = true;
        }
        gameObject.SetActive(false);
        collider.size = size;
        gameObject.transform.localPosition = offset;
    }

    public override void ActivateAttack()
    {
        gameObject.SetActive(true);
        Debug.Log("BoxCollider Attacked");
        StartCoroutine("DisableCollider");
        AttackReady = false;
        StartCoroutine("SetAttackReady");
    }

    private IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(DurationTime);
        gameObject.SetActive(false);
    }
    private IEnumerator SetAttackReady()
    {
        yield return new WaitForSeconds(AttackInterval);
        AttackReady = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (Owner == "Monster")
        {
            if(other.tag=="Player")
            {
                other.gameObject.GetComponent<Player>().Damaged(AttackValue);
                Debug.Log("CubeGiveDamage");
            }
        }
    }
    private void AttackPlayer()
    {

    }

    private void OnGUI()
    {
        //if (GUILayout.Button("BoxCollider"))
        //    ActivateAttack();
    }
}
