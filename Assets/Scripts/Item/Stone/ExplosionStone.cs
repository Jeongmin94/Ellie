using Assets.Scripts.Managers;
using Boss1.Terrapupa;
using Channels.Stone;
using Combat;
using Managers.Particle;
using Managers.Pool;
using Managers.Sound;
using UnityEngine;

namespace Item.Stone
{
    public class ExplosionStone : BaseStoneEffect
    {
        public float damageDistance = 3.0f;
        public LayerMask targetLayer;

        private bool isExplosion;
        private MeshCollider meshCollider;

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            meshCollider = GetComponent<MeshCollider>();

            var bossLayer = LayerMask.NameToLayer("Boss");
            var monsterLayer = LayerMask.NameToLayer("Monster");
            var playerLayer = LayerMask.NameToLayer("Ignore Raycast");
            var exceptGroundLayer = LayerMask.NameToLayer("ExceptGround");

            targetLayer = (1 << bossLayer) | (1 << monsterLayer) | (1 << playerLayer) | (1 << exceptGroundLayer);
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

            if (isExplosion && gameObject.activeSelf)
            {
                PoolManager.Instance.Push(GetComponent<Poolable>());
            }
        }

        public void Explosion()
        {
            // 콜라이더 제거 + 중력 제거
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            meshCollider.enabled = false;

            ParticleManager.Instance.GetParticle(data.skillEffectParticle, transform);
            //TerrapupaAttackHit, Stone_Sound_1
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "TerrapupaAttackHit", transform.position);

            CheckTargets();
        }

        private void CheckTargets()
        {
            var hitColliders = Physics.OverlapSphere(transform.position, damageDistance, targetLayer);
            foreach (var hitCollider in hitColliders)
            {
                var enemy = hitCollider.GetComponentInChildren<ICombatant>();

                if (enemy != null && !hitCollider.gameObject.CompareTag("WeakPoint"))
                {
                    OccurEffect(hitCollider.transform);
                }
                else if (hitCollider.GetComponent<TerrapupaWeakPoint>() != null)
                {
                    OccurEffect(hitCollider.transform);
                }
            }
        }
    }
}