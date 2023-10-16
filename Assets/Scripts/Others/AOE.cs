using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE : MonoBehaviour
{
    private float durationTime;
    private float attackValue;
    private float damageInterval;
    private string owner;

    private float accumulatedTime;

    private void Start()
    {
        Destroy(gameObject, durationTime);
    }
    public void SetPrefabData(float attackValue, float durationTime, float damageInterval, string owner)
    {
        this.attackValue = attackValue;
        this.durationTime = durationTime;
        this.damageInterval = damageInterval;
        this.owner = owner;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(owner=="Monster")
        {
            if(other.tag=="Player")
            {
                accumulatedTime = 0.0f;
                other.GetComponent<Player>().Damaged(attackValue);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (accumulatedTime > 1.0f)
        {
            if (owner == "Monster")
            {
                if (other.tag == "Player")
                {
                    accumulatedTime = 0.0f;
                    other.GetComponent<Player>().Damaged(attackValue);
                }
            }
            accumulatedTime = 0.0f;
        }
        accumulatedTime += Time.deltaTime;
    }
}
