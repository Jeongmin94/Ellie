using Assets.Scripts.Monsters.AbstractClass;
using Channels.Combat;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Monsters.Attacks
{
    public class FanShapeAttack : AbstractAttack
    {
        private FanShapeAttackData attackData;
        [SerializeField] private Transform target;
        private AudioSource audioSource;

        private void Awake()
        {
            SetTicketMachine();
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        private void Start()
        {
            audioSource.clip = audioController.GetAudio(MonsterAudioType.WeaponAttackHit);
            audioSource.spatialBlend = 1.0f;
            audioSource.maxDistance = 50.0f;
        }

        public override void InitializeFanShape(FanShapeAttackData data)
        {
            target = GameObject.Find("Player").transform;
            attackData = data;
            base.InitializeFanShape(data);
            audioController = transform.parent.GetComponent<MonsterAudioController>();
        }

        public bool CaculateDotProduct()
        {
            Vector3 interV = target.position - transform.position;

            float dot = Vector3.Dot(interV.normalized, transform.forward.normalized);
            float theta = Mathf.Acos(dot);
            float degree = Mathf.Rad2Deg * theta;

            if (degree <= attackData.angleRange / 2.0f)
            {
                interV.y = 0;
                if (interV.sqrMagnitude <= attackData.radius * attackData.radius)
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
            float accumTime = 0.0f;
            while (accumTime <= attackData.attackDuration)
            {
                if (CaculateDotProduct())
                {
                    if (target.CompareTag("Player"))
                    {
                        if(audioSource.clip==null)
                        {
                            audioSource.clip = audioController.GetAudio(MonsterAudioType.WeaponAttackHit);
                        }
                        audioSource.Play();
                        SetAndAttack(attackData, target);
                        break;
                    }
                }
                accumTime += Time.deltaTime;
                yield return null;
            }
        }

        private void SetAndAttack(FanShapeAttackData data, Transform otherTransform)
        {
            Debug.Log("SetPayloadAttack");
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

        private void OnDrawGizmos()
        {
            Handles.color = Color.red;
            Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, attackData.angleRange / 2, attackData.radius);
            Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -attackData.angleRange / 2, attackData.radius);
        }
    }
}