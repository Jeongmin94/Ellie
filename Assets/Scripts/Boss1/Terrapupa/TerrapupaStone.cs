using Assets.Scripts.Item;
using Assets.Scripts.Managers;
using Assets.Scripts.Particle;
using Assets.Scripts.StatusEffects;
using Assets.Scripts.Utils;
using Channels.Boss;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss.Terrapupa
{
	public class TerrapupaStone : Poolable
	{
		[SerializeField] private float movementSpeed = 15.0f;
		[SerializeField] private int attackValue = 5;
		[SerializeField] private LayerMask layerMask;

		private GameObject effect;
		private Transform owner;
		private TicketMachine ticketMachine;
		private SphereCollider sphereCollider;
		private Rigidbody rb;

        public Transform Owner
		{
			get { return owner; }
		}

        private void Awake()
        {
			sphereCollider = GetComponent<SphereCollider>();
			rb = GetComponent<Rigidbody>();
        }

        private void OnDisable()
        {
            transform.localScale = Vector3.one;

            sphereCollider.enabled = false;
			rb.isKinematic = true;
        }

        public void Init(Vector3 position, Vector3 scale, float speed, int attack, GameObject hitEffect, Transform sender, TicketMachine senderTicketMacine)
		{
            effect = hitEffect;
			transform.position = position;
			transform.localScale = scale;
			movementSpeed = speed;
			attackValue = attack;
			owner = sender;
			ticketMachine = senderTicketMacine;
        }

        private void OnCollisionEnter(Collision collision)
		{
            if (collision.gameObject.CompareTag("Player"))
            {
                ParticleManager.Instance.GetParticle(effect, new ParticlePayload
                {
                    Position = transform.position,
                    Scale = new Vector3(0.7f, 0.7f, 0.7f),
                });

                ticketMachine.SendMessage(ChannelType.Combat, new CombatPayload
				{
					Attacker = owner,
					Defender = collision.transform.root,
					Damage = attackValue,
					statusEffectduration = 1.0f,
					PlayerStatusEffectName = StatusEffectName.Down,

                });
            }
            if (collision.gameObject.CompareTag("Wall"))
			{
                ParticleManager.Instance.GetParticle(effect, new ParticlePayload
                {
                    Position = transform.position,
                    Scale = new Vector3(1.0f, 1.0f, 1.0f),
                });

                PoolManager.Instance.Push(this);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & layerMask) != 0 && other.transform.root != owner.transform.root)
            {
                ParticleManager.Instance.GetParticle(effect, new ParticlePayload
                {
                    Position = transform.position,
                    Scale = new Vector3(1.0f, 1.0f, 1.0f),
                });

                EventBus.Instance.Publish(EventBusEvents.HitStone, new BossEventPayload
				{
					TransformValue1 = other.transform.root,
				});

                PoolManager.Instance.Push(this);
            }
        }

        public void MoveToTarget(Transform target)
		{
			Vector3 direction = (target.position + new Vector3(0.0f, 2.0f, 0.0f)) - transform.position;
			direction.Normalize();

			SphereCollider sphereCollider = GetComponent<SphereCollider>();

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