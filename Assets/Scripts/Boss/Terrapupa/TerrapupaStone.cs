using Assets.Scripts.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrapupaStone : MonoBehaviour
{
	public float movementSpeed = 7.0f;

	private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("InteractionObject") || other.gameObject.CompareTag("Ground"))
		{
			Debug.Log($"{other.name} Ãæµ¹");

			Destroy(gameObject);
		}
    }

    public void MoveToTarget(Transform target)
	{
		Vector3 direction = target.position - transform.position;
		direction.Normalize();

        Rigidbody rb = GetComponent<Rigidbody>();
        SphereCollider sphereCollider = GetComponent<SphereCollider>();

        if (rb != null && sphereCollider != null)
		{
			rb.isKinematic = false;
			sphereCollider.enabled = true;

            rb.velocity = direction * movementSpeed;
		}
		else
		{
			Debug.LogError("GetComponent Error");
		}
    }
}
