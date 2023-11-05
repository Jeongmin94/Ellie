using Assets.Scripts.Item;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Type;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss.Terrapupa
{
	public class TerrapupaStone : Poolable
	{
		private TicketMachine ticketMachine;

		public float movementSpeed = 7.0f;

        private void Awake()
        {
			SetTicketMachine();
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
				Debug.Log($"{collision.collider.name} Ãæµ¹");

				PoolManager.Instance.Push(this);
			}
		}

		public void MoveToTarget(Transform target)
		{
			Vector3 direction = (target.position + new Vector3(0.0f, 2.0f, 0.0f)) - transform.position;
			direction.Normalize();

			Rigidbody rb = GetComponent<Rigidbody>();
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