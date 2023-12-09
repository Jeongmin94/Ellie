using Assets.Scripts.Channels;
using Assets.Scripts.Channels.Item;
using Assets.Scripts.Combat;
using Assets.Scripts.Managers;
using Assets.Scripts.Particle;
using Channels.Type;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Item.Stone
{
    public class ExplosionStone : BaseStoneEffect
    {
        public float damageDistance = 3.0f;
        public LayerMask targetLayer;

        private Rigidbody rb;
        private MeshCollider meshCollider;

        private bool isExplosion = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            meshCollider = GetComponent<MeshCollider>();

            int bossLayer = LayerMask.NameToLayer("Boss");
            int monsterLayer = LayerMask.NameToLayer("Monster");
            int playerLayer = LayerMask.NameToLayer("Ignore Raycast");

            targetLayer = (1 << bossLayer) | (1 << monsterLayer) | (1 << playerLayer);
        }

        private void OnDisable()
        {
            rb.isKinematic = false;
            meshCollider.enabled = true;
            isExplosion = false;
        }

        protected override void OnCollisionEnter(Collision collision)
        {
            if (!isExplosion && Type == StoneEventType.ShootStone)
            {
                isExplosion = true;
                Explosion();
            }
        }

        public void Explosion()
        {
            Debug.Log("폭발 돌맹이 폭발");

            // 콜라이더 제거 + 중력 제거
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            meshCollider.enabled = false;

            ParticleManager.Instance.GetParticle(data.skillEffectParticle, transform, 1.0f);
            //TerrapupaAttackHit, Stone_Sound_1
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "TerrapupaAttackHit", transform.position);

            CheckTargets();

            PoolManager.Instance.Push(this.GetComponent<Poolable>());
        }

        private void CheckTargets()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageDistance, targetLayer);
            foreach (var hitCollider in hitColliders)
            {
                ICombatant enemy = hitCollider.GetComponentInChildren<ICombatant>();

                if (enemy != null)
                {
                    OccurEffect(hitCollider.transform);
                }
            }
        }
    }
}
