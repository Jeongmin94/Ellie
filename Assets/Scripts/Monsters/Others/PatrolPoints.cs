using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoints : MonoBehaviour
{
    public List<GameObject> patrolPoints;

    public Vector3[] GetPatrolPointst()
    {
        Vector3[] patrolVectors = new Vector3[patrolPoints.Count];
        for(int i=0; i<patrolPoints.Count;i++)
        {
            patrolVectors[i] = patrolPoints[i].transform.position;
            patrolVectors[i].y = transform.position.y;
        }

        return patrolVectors;
    }
    public List<GameObject>GetPatrolPoints()
    {
        return patrolPoints;
    }

}
