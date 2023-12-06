using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Combat;
using Assets.Scripts.Monsters.Attacks;
using Assets.Scripts.Monsters.Utility;
using UnityEngine;

namespace Assets.Scripts.Monsters.Others
{

    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float projectileSpeed;
        public ProjectileAttack spawner;

        private void Start()
        {
            Destroy(gameObject, spawner.attackData.attackDuration);
        }

        private void FixedUpdate()
        {
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.gameObject.GetComponent<ICombatant>() != null)
                {
                    spawner.ProjectileHitPlayer(other.transform);
                }
                Destroy(gameObject);
            }
        }
    }
}