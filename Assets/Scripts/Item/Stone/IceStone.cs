using Assets.Scripts.Managers;
using Assets.Scripts.Particle;
using Boss1.Terrapupa;
using Channels.Stone;
using Combat;
using UnityEngine;

namespace Item.Stone
{
    public class IceStone : BaseStoneEffect
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
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "Stone_Sound_1", transform.position);

            CheckTargets();

            PoolManager.Instance.Push(GetComponent<Poolable>());
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