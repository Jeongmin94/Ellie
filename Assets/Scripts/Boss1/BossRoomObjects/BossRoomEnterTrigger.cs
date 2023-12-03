using Channels.Boss;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomEnterTrigger : MonoBehaviour
{
    [SerializeField] private Transform wall;

    public EventBusEvents eventType;

    private void Start()
    {
        wall.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (eventType)
            {
                case EventBusEvents.EnterBossRoom:
                    {
                        EventBus.Instance.Publish(EventBusEvents.EnterBossRoom, new BossEventPayload
                        {
                            TransformValue1 = transform,
                            TransformValue2 = wall,
                        });
                    }
                    break;
                case EventBusEvents.LeftBossRoom:
                    {
                        EventBus.Instance.Publish(EventBusEvents.LeftBossRoom, new BossEventPayload
                        {
                            TransformValue1 = transform,
                        });
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
