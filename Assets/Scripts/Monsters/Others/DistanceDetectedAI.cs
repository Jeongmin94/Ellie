using UnityEngine;

namespace Monsters.Others
{
    public class DistanceDetectedAI : MonoBehaviour
    {
        private SphereCollider collider;
        public bool IsDetected { get; private set; }

        private void Awake()
        {
            collider = GetComponent<SphereCollider>();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                IsDetected = false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player")
            {
                IsDetected = true;
            }
        }

        public void SetDetectDistance(float playerDetectDistance)
        {
            collider.isTrigger = true;
            collider.radius = playerDetectDistance;
        }
    }
}