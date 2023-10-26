using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrapupaStone : MonoBehaviour
{
	public float movementSpeed = 7.0f;

	private Rigidbody rb;
	private Transform target;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Ground"))
		{
			Debug.Log("벽과 충돌");

			Destroy(gameObject);
		}
    }

    public void MoveToTarget(Transform target)
	{
		Vector3 direction = target.position - transform.position;

		direction.Normalize();

		//rb.velocity = direction * movementSpeed;
	}
}
