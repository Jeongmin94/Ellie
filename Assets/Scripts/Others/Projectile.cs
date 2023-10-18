using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    private float durationTime;
    
    private float attackValue;
    private string owner;

    private void Start()
    {
        Destroy(gameObject, durationTime);
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * projectileSpeed*Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (owner == "Monster")
        {
            if (other.tag == "Player")
            {
                FireEnemyProjectile(other);
            }
        }
        else if (owner == "Player")
        {

        }
    }
    private void FireEnemyProjectile(Collider other)
    {
   
            other.gameObject.GetComponent<TestPlayer>().Damaged(attackValue);
            Destroy(gameObject);
    }
    private void FirePlayerProjectile(Collider other)
    {

    }

    public void SetProjectileData(float attackValue,float durationTime, string owner)
    {
        this.durationTime = durationTime;
        this.attackValue = attackValue;
        this.owner = owner;
    }
}
