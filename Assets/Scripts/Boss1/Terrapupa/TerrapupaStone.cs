using Assets.Scripts.Managers;
using Channels.Boss;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using Managers.Event;
using Managers.Particle;
using Managers.Pool;
using Managers.Sound;
using UnityEngine;

namespace Boss1.Terrapupa
{
    public class TerrapupaStone : Poolable
    {
        [SerializeField] private float movementSpeed = 15.0f;
        [SerializeField] private int attackValue = 5;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private string hitSound = "TerrapupaAttackHit";

        public float remainTime = 10.0f;
        private CombatPayload combatPayload;
        private Vector3 direction;

        private GameObject effect;
        private Rigidbody rb;
        private SphereCollider sphereCollider;
        private TicketMachine ticketMachine;

        public Transform Owner { get; private set; }

        private void Awake()
        {
            sphereCollider = GetComponent<SphereCollider>();
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            rb.AddForce(direction * movementSpeed, ForceMode.Force);
        }

        private void OnDisable()
        {
            transform.localScale = Vector3.one;

            sphereCollider.enabled = false;
            rb.isKinematic = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                ParticleManager.Instance.GetParticle(effect, transform, 0.7f);

                combatPayload.Attacker = Owner;
                combatPayload.Defender = collision.transform.root;
                SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, hitSound, transform.position);
                ticketMachine.SendMessage(ChannelType.Combat, combatPayload);
            }

            if (collision.gameObject.CompareTag("Wall"))
            {
                ParticleManager.Instance.GetParticle(effect, transform);
                SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, hitSound, transform.position);
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var detection = other.GetComponent<TerrapupaDetection>();

            if (((1 << other.gameObject.layer) & layerMask) != 0 && detection.MyTerrapupa != Owner)
            {
                ParticleManager.Instance.GetParticle(effect, transform);
                SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, hitSound, transform.position);

                EventBus.Instance.Publish(EventBusEvents.HitStone, new BossEventPayload
                {
                    TransformValue1 = detection.MyTerrapupa
                });

                Destroy(gameObject);
            }
        }

        public void Init(Vector3 position, Vector3 scale, float speed, CombatPayload hitPayload, GameObject hitEffect,
            Transform sender, TicketMachine senderTicketMacine)
        {
            effect = hitEffect;
            transform.position = position;
            transform.localScale = scale;
            movementSpeed = speed;
            combatPayload = hitPayload;
            attackValue = hitPayload.Damage;
            Owner = sender;
            ticketMachine = senderTicketMacine;

            Destroy(gameObject, remainTime);
        }

        public void MoveToTarget(Transform target)
        {
            direction = target.position + new Vector3(0.0f, 2.0f, 0.0f) - transform.position;
            direction.Normalize();

            var sphereCollider = GetComponent<SphereCollider>();

            if (rb != null && sphereCollider != null)
            {
                sphereCollider.enabled = true;
                rb.useGravity = true;
                rb.isKinematic = false;

                rb.velocity = direction * movementSpeed;
            }
            else
            {
                Debug.LogError("GetComponent Error");
            }
        }
    }
}