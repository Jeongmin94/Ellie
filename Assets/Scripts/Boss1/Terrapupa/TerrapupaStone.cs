using Assets.Scripts.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss.Terrapupa
{
	public class TerrapupaStone : MonoBehaviour
	{
		public float movementSpeed = 7.0f;

		private void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.CompareTag("Wall"))
			{
				Debug.Log($"{collision.collider.name} Ãæµ¹");

				Destroy(gameObject);
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