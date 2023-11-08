using Assets.Scripts.Item;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using Channels.Boss;
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
		[SerializeField] private float attackValue = 5.0f;
		[SerializeField] private LayerMask layerMask;

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
			SetTicketMachine();
			sphereCollider = GetComponent<SphereCollider>();
			rb = GetComponent<Rigidbody>();
        }

        private void OnDisable()
        {
            transform.localScale = Vector3.one;

            sphereCollider.enabled = false;
			rb.isKinematic = true;
        }

        public void Init(Vector3 position, Vector3 scale, float speed, int attack, Transform sender)
		{
			transform.position = position;
			transform.localScale = scale;
			movementSpeed = speed;
			attackValue = attack;
			owner = sender;
        }

        private void SetTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.Terrapupa, ChannelType.BossInteraction, ChannelType.Combat);
        }

        private void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.CompareTag("Wall"))
			{
				PoolManager.Instance.Push(this);
			}
		}

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & layerMask) != 0 && other.transform.root != owner.transform.root)
            {
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