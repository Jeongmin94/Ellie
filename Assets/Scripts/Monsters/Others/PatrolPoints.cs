using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoints : MonoBehaviour
{
    public Vector3[] GetPatrolPointst()
    {
        Vector3[] patrolVectors = new Vector3[transform.childCount];
        int i = 0; 
        foreach (Transform child in gameObject.transform)
        {
            patrolVectors[i] = child.transform.position;
            patrolVectors[i].y = transform.position.y;
            i++;
        }

        return patrolVectors;
    }

}
