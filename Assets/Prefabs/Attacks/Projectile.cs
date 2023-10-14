using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float ProjectileSpeed;
    [SerializeField] private float DestroyTime;
    
    private float AttackValue;
    private string Owner;

    private void Start()
    {
        Destroy(gameObject, DestroyTime);
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * ProjectileSpeed*Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(Owner);
        if (Owner == "Monster")
        {
            if (other.tag == "Player")
            {
                FireEnemyProjectile(other);
            }
        }
        else if (Owner == "Player")
        {

        }
    }
    private void FireEnemyProjectile(Collider other)
    {
   
            other.gameObject.GetComponent<Player>().Damaged(AttackValue);
            Destroy(gameObject);
    }
    private void FirePlayerProjectile(Collider other)
    {

    }

    public void SetProjectileData(float attackValue,string owner)
    {
        AttackValue = attackValue;
        Owner = owner;
        Debug.Log("Set Owner : " + Owner);
    }
}
