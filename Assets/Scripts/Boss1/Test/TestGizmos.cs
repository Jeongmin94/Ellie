using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGizmos : MonoBehaviour
{
    public GameObject testClient;

    public float radius = 10f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue; // Gizmo 색상 설정
        Gizmos.DrawWireSphere(testClient.transform.position, radius);
    }
}
