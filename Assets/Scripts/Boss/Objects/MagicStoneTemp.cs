using Assets.Scripts.Boss;
using UnityEngine;

public class MagicStoneTemp : MonoBehaviour
{
    public float attractionRadiusRange;
    public LayerMask bossLayer;

    private bool isTempted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isTempted && ((1 << other.gameObject.layer) & bossLayer) != 0)
        {
            isTempted = true;

            EventBus.Instance.Publish(EventBusEvents.BossAttractedByMagicStone,
                new BossEventPayload { TransformValue1 = transform, TransformValue2 = other.transform.root });
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & bossLayer) != 0)
        {
            isTempted = false;

            EventBus.Instance.Publish(EventBusEvents.BossUnattractedByMagicStone,
                new BossEventPayload { TransformValue1 = transform, TransformValue2 = other.transform.root });
        }
    }
}