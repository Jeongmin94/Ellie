using Assets.Scripts.Channels.Item;
using Assets.Scripts.Combat;
using Assets.Scripts.Particle;
using Channels.Boss;
using Codice.Client.BaseCommands;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Item.Stone
{
    public class MagicStone : BaseStoneEffect
    {
        public float attractionRadiusRange = 10.0f;
        public LayerMask bossLayer;

        private bool isCollideGround = false;
        private bool isTrigger = false;
        private Transform target;
        private Rigidbody rb;

        private GameObject range;
        private Material material;

        private void Start()
        {
            Debug.Log($"확인용 마석돌맹이, 타입은 {Type}");

            int bossLayerIndex = LayerMask.NameToLayer("Monster");
            bossLayer = 1 << bossLayerIndex;

            rb = GetComponent<Rigidbody>();
            material = Resources.Load<Material>("Materials/Sensor2");
        }
        private void OnDisable()
        {
            isTrigger = false;
            isCollideGround = false;

            if(range != null)
            {
                Destroy(range.gameObject);
                range = null;
            }

            if(rb != null)
            {
                rb.isKinematic = false;
            }
        }

        private void Update()
        {
            if(isCollideGround && !isTrigger)
            {
                CheckForBossInRange();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!isCollideGround && Type == StoneEventType.ShootStone
                && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                if (collision.gameObject.CompareTag("Ground"))
                {
                    isCollideGround = true;
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.isKinematic = true;

                    range = RangeManager.Instance.CreateRange(new RangePayload
                    {
                        DetectionMaterial = material,
                        Type = RangeType.Circle,
                        Radius = attractionRadiusRange,
                        StartPosition = transform.position,
                    });
                }
            }
        }

        private void CheckForBossInRange()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attractionRadiusRange, bossLayer);
            foreach (var hitCollider in hitColliders)
            {
                Debug.Log(hitCollider);
                if (hitCollider.CompareTag("Boss"))
                {
                    isTrigger = true;
                    target = hitCollider.transform.root;

                    EventBus.Instance.Publish(EventBusEvents.BossAttractedByMagicStone,
                        new BossEventPayload { TransformValue1 = transform, TransformValue2 = hitCollider.transform.root });

                    break; // 탐지된 Boss가 있으면 루프 종료
                }
            }
        }

    }
}