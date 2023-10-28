using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrapupaStone : MonoBehaviour
{
	public float movementSpeed = 7.0f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Boss"))
		{
			Debug.Log("플레이어와 충돌");

			Destroy(gameObject);
		}
    }

    public void MoveToTarget(Vector3 target)
	{

	}

	private IEnumerator Move(Vector3 target)
	{
		while (true)
		{

			yield return null;
		}
	}
}
