using System.Collections;
using Channels.Combat;
using Data.Monster;
using Monsters.AbstractClass;
using Player.StatusEffects;
using UnityEditor;
using UnityEngine;

namespace Monsters.Attacks
{
    public class FanShapeAttack : AbstractAttack
    {
        private const float angle = 180.0f;
        private const float radius = 2.0f;
        [SerializeField] private Transform target;

        private MonsterAttackData attackData;
        private AudioSource audioSource;
        private ParticleSystem particle;

        private void Awake()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            monsterController = transform.parent.GetComponent<AbstractMonster>();
        }

        private void Start()
        {
            audioSource.clip = audioController.GetAudio(MonsterAudioType.WeaponAttackHit);
            audioSource.spatialBlend = 1.0f;
            audioSource.maxDistance = 50.0f;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = Color.red;
            Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angle / 2, radius);
            Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angle / 2, radius);
        }
#endif

        public override void InitializeFanShape(MonsterAttackData data)
        {
            target = GameObject.Find("Player").transform;
            attackData = data;
            base.InitializeFanShape(data);
            if (audioController == null)
            {
                audioController = transform.parent.GetComponent<MonsterAudioController>();
            }

            if (particleController == null)
            {
                particleController = transform.parent.GetComponent<MonsterParticleController>();
            }
        }

        public bool CaculateDotProduct()
        {
            var interV = target.position - transform.position;

            var dot = Vector3.Dot(interV.normalized, transform.forward.normalized);
            var theta = Mathf.Acos(dot);
            var degree = Mathf.Rad2Deg * theta;

            if (degree <= angle / 2.0f)
            {
                interV.y = 0;
                if (interV.sqrMagnitude <= radius * radius)
                {
                    return true;
                }
            }

            return false;
        }

        public override void ActivateAttack()
        {
            StartCoroutine(AttackFanshape());
        }

        public IEnumerator AttackFanshape()
        {
            var accumTime = 0.0f;
            while (accumTime <= attackData.attackDuration)
            {
                if (CaculateDotProduct())
                {
                    if (target.CompareTag("Player"))
                    {
                        if (audioSource.clip == null)
                        {
                            audioSource.clip = audioController.GetAudio(MonsterAudioType.WeaponAttackHit);
                        }

                        if (particle == null)
                        {
                            particle = particleController.GetParticle(MonsterParticleType.WeaponHit);
                        }

                        audioSource.Play();
                        particle.transform.position = target.position;
                        particle.Play();
                        SetAndAttack(attackData, target);
                        break;
                    }
                }

                accumTime += Time.deltaTime;
                yield return null;
            }
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
            payload.statusEffectduration = 0.5f;
            payload.Damage = data.attackValue;
            Attack(payload);
        }
    }
}