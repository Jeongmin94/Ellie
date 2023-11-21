using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Monsters.AbstractClass;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using Assets.Scripts.Utils;
using UnityEngine;
using Assets.Scripts.Player;
using Channels.Combat;
using Assets.Scripts.Combat;

namespace Assets.Scripts.Monsters.Attacks
{
    public class BoxColliderAttack : AbstractAttack
    {
        private BoxCollider collider;
        private BoxColliderAttackData attackData;
        private ParticleSystem particle;

        private void Awake()
        {
            SetTicketMachine();   
        }

        public override void InitializeBoxCollider(BoxColliderAttackData data)
        {
            attackData = data;
            InitializedBase(data.attackValue, data.attackDuration, data.attackInterval, data.attackableDistance);

            if (collider == null)
            {
                collider = gameObject.AddComponent<BoxCollider>();
                collider.isTrigger = true;
            }

            collider.size = data.size;
            gameObject.transform.localPosition = data.offset;
            collider.enabled = false;

            if (audioController == null)
                audioController = transform.parent.GetComponent<MonsterAudioController>();
            if (particleController == null)
                particleController = transform.parent.GetComponent<MonsterParticleController>();
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

        private void OnTriggerEnter(Collider other)
        {
            if (owner == "Monster")
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
        }

        private void SetAndAttack(BoxColliderAttackData data, Transform otherTransform)
        {
            CombatPayload payload = new();
            payload.Type = data.combatType;
            payload.Attacker = transform;
            payload.Defender = otherTransform;
            payload.AttackDirection = Vector3.zero;
            payload.AttackStartPosition = transform.position;
            payload.AttackPosition = otherTransform.position;
            payload.PlayerStatusEffectName = StatusEffects.StatusEffectName.WeakRigidity;
            payload.Damage = (int)data.attackValue;
            Attack(payload);
        }


    }
}
