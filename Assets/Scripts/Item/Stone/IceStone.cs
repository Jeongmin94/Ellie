using Assets.Scripts.Channels.Item;
using Assets.Scripts.Combat;
using Assets.Scripts.Managers;
using Assets.Scripts.Particle;
using Boss.Terrapupa;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Item.Stone
{
    public class IceStone : BaseStoneEffect
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
            int exceptGroundLayer = LayerMask.NameToLayer("ExceptGround");

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

            ParticleManager.Instance.GetParticle(data.skillEffectParticle, transform, 1.0f);
            //TerrapupaAttackHit, Stone_Sound_1
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "Stone_Sound_1", transform.position);

            CheckTargets();

            PoolManager.Instance.Push(this.GetComponent<Poolable>());
        }

        private void CheckTargets()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageDistance, targetLayer);
            foreach (var hitCollider in hitColliders)
            {
                ICombatant enemy = hitCollider.GetComponentInChildren<ICombatant>();

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