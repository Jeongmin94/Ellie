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
		[SerializeField] private string hitSound = "TerrapupaAttackHit";

		public float remainTime = 10.0f;

        private GameObject effect;
		private Transform owner;
		private TicketMachine ticketMachine;
		private SphereCollider sphereCollider;
		private Rigidbody rb;
		private CombatPayload combatPayload;
		private Vector3 direction;

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

        private void Update()
        {
			rb.AddForce(direction * movementSpeed, ForceMode.Force);
        }

        public void Init(Vector3 position, Vector3 scale, float speed, CombatPayload hitPayload, GameObject hitEffect, Transform sender, TicketMachine senderTicketMacine)
		{
            effect = hitEffect;
			transform.position = position;
			transform.localScale = scale;
			movementSpeed = speed;
			combatPayload = hitPayload;
            attackValue = hitPayload.Damage;
			owner = sender;
			ticketMachine = senderTicketMacine;

            Destroy(this.gameObject, remainTime);
        }

        private void OnCollisionEnter(Collision collision)
		{
            if (collision.gameObject.CompareTag("Player"))
            {
				ParticleManager.Instance.GetParticle(effect, transform, 0.7f);

				combatPayload.Attacker = owner;
				combatPayload.Defender = collision.transform.root;
				SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, hitSound, transform.position);
				ticketMachine.SendMessage(ChannelType.Combat, combatPayload);
            }
            if (collision.gameObject.CompareTag("Wall"))
			{
				ParticleManager.Instance.GetParticle(effect, transform, 1.0f);
                SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, hitSound, transform.position);
				Destroy(this.gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
			TerrapupaDetection detection = other.GetComponent<TerrapupaDetection>();

            if (((1 << other.gameObject.layer) & layerMask) != 0 && detection.MyTerrapupa != owner)
            {
                ParticleManager.Instance.GetParticle(effect, transform, 1.0f);
                SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, hitSound, transform.position);

                EventBus.Instance.Publish(EventBusEvents.HitStone, new BossEventPayload
				{
					TransformValue1 = detection.MyTerrapupa,
                });

                Destroy(this.gameObject);
            }
        }

        public void MoveToTarget(Transform target)
		{
			direction = (target.position + new Vector3(0.0f, 2.0f, 0.0f)) - transform.position;
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