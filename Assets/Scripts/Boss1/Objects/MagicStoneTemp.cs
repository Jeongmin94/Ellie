using Channels.Boss;
using UnityEngine;

namespace Boss.Objects
{
    public class MagicStoneTemp : MonoBehaviour
    {
        public float attractionRadiusRange;
        public LayerMask bossLayer;

        private bool isTrigger = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!isTrigger && ((1 << other.gameObject.layer) & bossLayer) != 0)
            {
                isTrigger = true;

                EventBus.Instance.Publish(EventBusEvents.BossAttractedByMagicStone,
                    new BossEventPayload { TransformValue1 = transform, TransformValue2 = other.transform.root });
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & bossLayer) != 0)
            {
                isTrigger = false;

                EventBus.Instance.Publish(EventBusEvents.BossUnattractedByMagicStone,
                    new BossEventPayload { TransformValue1 = transform, TransformValue2 = other.transform.root });
            }
        }
    }
}
