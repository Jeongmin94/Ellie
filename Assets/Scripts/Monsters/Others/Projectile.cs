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
        private float projectileSpeed;
        public ProjectileAttack spawner;
        public Transform player;
        public bool isChasing = false;
        private float rotationSpeed = 10.0f;
        private float accumTime = 0.0f;

        private void Start()
        {
            Destroy(gameObject, spawner.attackData.attackDuration);
        }

        private void FixedUpdate()
        {
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
            if(isChasing)
            {
                ChasePlayer();
            }
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
            else if (other.CompareTag("Wall"))
                Destroy(gameObject);
        }
        private void ChasePlayer()
        {
            Vector3 directionToTarget = player.position - transform.position;
            if (directionToTarget.sqrMagnitude < 1.0f)
            {
                accumTime += Time.deltaTime;
            }
            if(accumTime>1.0f)
                Destroy(gameObject);

            Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, rotationSpeed * Time.deltaTime);
        }
        public void ChasePlayer(Transform player)
        {
            this.player = player;
            isChasing = true;
        }
        public void SetSpeed(float speed)
        {
            projectileSpeed = speed;
        }

    }
}