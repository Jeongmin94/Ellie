using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Boss.Objects
{
    public class MagicStalactite : MonoBehaviour
    {
        public float respawnValue = 10.0f;

        private bool isCooldown;

        public bool IsRespawn
        {
            get { return isCooldown; }
            set { isCooldown = value; }
        }


        private void OnCollisionEnter(Collision collision)
        {
            if(gameObject.CompareTag("Stone") && !isCooldown)
            {
                Debug.Log(collision.transform.name);

                isCooldown = true;

                EventBus.Instance.Publish(EventBusEvents.DropMagicStalactite,
                    new BossEventPayload { TransformValue1 = transform });
            }

            if(gameObject.CompareTag("Boss"))
            {
                Debug.Log(collision.transform.name);
            }
        }


    }
}