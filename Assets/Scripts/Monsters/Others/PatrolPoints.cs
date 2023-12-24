using UnityEngine;

namespace Monsters.Others
{
    public class PatrolPoints : MonoBehaviour
    {
        public Vector3[] GetPatrolPointst()
        {
            var patrolVectors = new Vector3[transform.childCount];
            var i = 0;
            foreach (Transform child in gameObject.transform)
            {
                patrolVectors[i] = child.transform.position;
                patrolVectors[i].y = transform.position.y;
                i++;
            }

            return patrolVectors;
        }
    }
}