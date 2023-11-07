using Assets.Scripts.Managers;
using Channels.Boss;
using UnityEngine;

namespace Boss.Objects
{
    public class MagicStoneTemp : Poolable
    {
        public float attractionRadiusRange;
        public LayerMask bossLayer;

        private bool isTrigger = false;
        private Transform target;

        private void OnTriggerEnter(Collider other)
        {
            if (!isTrigger && ((1 << other.gameObject.layer) & bossLayer) != 0 &&
                other.CompareTag("Boss"))
            {
                isTrigger = true;

                EventBus.Instance.Publish(EventBusEvents.BossAttractedByMagicStone,
                    new BossEventPayload { TransformValue1 = transform, TransformValue2 = other.transform.root });

                target = other.transform.root;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.root == target)
            {
                isTrigger = false;

                EventBus.Instance.Publish(EventBusEvents.BossUnattractedByMagicStone,
                    new BossEventPayload { TransformValue1 = transform, TransformValue2 = other.transform.root });

                target = null;
            }
        }
    }
}
