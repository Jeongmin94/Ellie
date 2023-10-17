using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEPrefabAttack : AbstractAttack
{
    [SerializeField] private AOE prefabObject;
    private Vector3 position;
    private Vector3 offset;
    private float damageInterval;

    public override void InitializeAOE
        (float attackValue, float durationTime, float attackInterval, float attackRange, float damageInterval,GameObject prefabObject)
    {
        InitializedBase(attackValue, durationTime, attackInterval, attackRange);
        Debug.Log("ParameterOBJ : " + prefabObject);
        this.damageInterval = damageInterval;
        this.prefabObject = prefabObject.GetComponent<AOE>();
        if (prefabObject == null)
            Debug.Log("[AOEPrefabAttack] PrefabObject is Null");
    }

    public override void ActivateAttack()
    {
        AOE obj = Instantiate(prefabObject, position + offset, transform.rotation);
        obj.SetPrefabData(attackValue, durationTime, damageInterval, gameObject.tag.ToString());
        StartCoroutine(StartAttackReadyCount());
    }
    private IEnumerator StartAttackReadyCount()
    {
        IsAttackReady = false;
        yield return new WaitForSeconds(AttackInterval);
        IsAttackReady = true;
    }

    public void SetPrefabPosition(Vector3 position,Vector3 offset)
    {
        this.position = position;
        this.offset = offset;
    }
}
