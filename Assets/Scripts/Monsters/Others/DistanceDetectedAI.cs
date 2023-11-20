using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Monsters.Others
{

    public class DistanceDetectedAI : MonoBehaviour
    {
        public bool IsDetected { get; private set; }
        private SphereCollider collider;

        private void Awake()
        {
            collider = GetComponent<SphereCollider>();
            if (collider == null)
                Debug.Log("[DistanceDetectedAI] : Collider is Null");

        }
        public void SetDetectDistance(float playerDetectDistance)
        {
            collider.isTrigger = true;
            collider.radius = playerDetectDistance;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player")
            {
                Debug.Log(gameObject.name + "IS DETECTED");
                IsDetected = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                IsDetected = false;
            }
        }

    }
}