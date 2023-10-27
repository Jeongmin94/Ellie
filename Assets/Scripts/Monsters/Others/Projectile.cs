using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Monsters.Others
{

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
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }

        public void SetProjectileData(float attackValue, float durationTime, string owner)
        {
            this.durationTime = durationTime;
            this.attackValue = attackValue;
            this.owner = owner;
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
            Destroy(gameObject);
            //Send Message To GameCenter
        }
        private void FirePlayerProjectile(Collider other)
        {

        }

    }
}