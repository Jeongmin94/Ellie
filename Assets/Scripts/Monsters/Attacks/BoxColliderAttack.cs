using System.Collections;
using Channels.Combat;
using Combat;
using Data.Monster;
using Monsters.AbstractClass;
using Player.StatusEffects;
using UnityEngine;

namespace Monsters.Attacks
{
    public class BoxColliderAttack : AbstractAttack
    {
        private MonsterAttackData attackData;
        private BoxCollider collider;
        private ParticleSystem particle;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.gameObject.GetComponent<ICombatant>() != null)
                {
                    audioController.PlayAudio(MonsterAudioType.MeleeAttackHit);
                    if (particle == null)
                    {
                        particle = particleController.GetParticle(MonsterParticleType.MeleeHit);
                    }

                    particle.transform.position = other.transform.position;
                    particle.Play();
                    SetAndAttack(attackData, other.transform);
                }
            }
        }

        public override void InitializeBoxCollider(MonsterAttackData data)
        {
            attackData = data;
            InitializedBase(data);

            if (collider == null)
            {
                collider = gameObject.AddComponent<BoxCollider>();
                collider.isTrigger = true;
            }

            collider.size = data.size;
            gameObject.transform.localPosition = data.offset;
            collider.enabled = false;

            if (audioController == null)
            {
                audioController = transform.parent.GetComponent<MonsterAudioController>();
            }

            if (particleController == null)
            {
                particleController = transform.parent.GetComponent<MonsterParticleController>();
            }
        }

        public override void ActivateAttack()
        {
            collider.enabled = true;
            StartCoroutine(DisableCollider());
        }

        private IEnumerator DisableCollider()
        {
            yield return new WaitForSeconds(durationTime);
            collider.enabled = false;
        }

        private void SetAndAttack(MonsterAttackData data, Transform otherTransform)
        {
            CombatPayload payload = new();
            payload.Type = data.combatType;
            payload.Attacker = transform;
            payload.Defender = otherTransform;
            payload.AttackDirection = Vector3.zero;
            payload.AttackStartPosition = transform.position;
            payload.AttackPosition = otherTransform.position;
            payload.StatusEffectName = StatusEffectName.WeakRigidity;
            payload.statusEffectduration = 0.3f;
            payload.Damage = data.attackValue;
            Attack(payload);
        }
    }
}