using Assets.Scripts.Channels.Item;
using Assets.Scripts.Managers;
using Channels.Boss;
using Channels.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Item.Stone
{
    public class MagicStone : BaseStoneEffect
    {
        public static bool isActivateRange = false;

        public float attractionRadiusRange = 10.0f;
        public float duration = 10.0f;
        public LayerMask bossLayer;

        private bool isTrigger = false;
        private Transform target;
        private Rigidbody rb;

        private GameObject range;
        private Material material;
        private MeshCollider meshCollider;
        private Coroutine durationCoroutine;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            meshCollider = GetComponent<MeshCollider>();
            material = Resources.Load<Material>("Materials/Sensor2");
            bossLayer = 1 << LayerMask.NameToLayer("Monster");
        }

        private void OnDisable()
        {
            isTrigger = false;
            isActivateRange = false;

            if (range != null)
            {
                Destroy(range.gameObject);
                range = null;
            }

            if(rb != null)
            {
                rb.isKinematic = false;
            }

            //보스의 타겟 초기화
            EventBus.Instance.Publish(EventBusEvents.BossUnattractedByMagicStone, new BossEventPayload
            {
                TransformValue1 = transform,
                TransformValue2 = target,
            });
        }

        private void Update()
        {
            if(isActivateRange && !isTrigger)
            {
                CheckForBossInRange();
            }
        }

        protected override void OnCollisionEnter(Collision collision)
        {
            base.OnCollisionEnter(collision);

            if (!isActivateRange && Type == StoneEventType.ShootStone
                && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                if (collision.gameObject.CompareTag("Ground"))
                {
                    EventBus.Instance.Publish(EventBusEvents.ActivateMagicStone, new BossEventPayload
                    {
                        Sender = transform,             // 마법돌맹이 객체
                    });
                }
            }
        }

        public void StopCheckDuration()
        {
            Debug.Log("StopCheckDuration() :: 지속시간 체크 정지");
            if (durationCoroutine != null)
            {
                StopCoroutine(durationCoroutine);
            }
        }

        public void ActivateRange()
        {
            // 마법돌맹이 콜라이더 제거 + 중력 제거
            isActivateRange = true;

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            meshCollider.enabled = false;

            // 지속시간 설정 + 적용 범위 표시
            durationCoroutine = StartCoroutine(StartDurationCheck(duration));
            range = RangeManager.Instance.CreateRange(new RangePayload
            {
                DetectionMaterial = material,
                Type = RangeType.Circle,
                Radius = attractionRadiusRange,
                StartPosition = transform.position,
            });
        }

        private void CheckForBossInRange()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attractionRadiusRange, bossLayer);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Boss"))
                {
                    isTrigger = true;
                    target = hitCollider.transform.root;
                    Debug.Log(target.name);

                    EventBus.Instance.Publish(EventBusEvents.BossAttractedByMagicStone, new BossEventPayload 
                    { 
                        TransformValue1 = transform, 
                        TransformValue2 = target
                    });

                    break;
                }
            }
        }

        private IEnumerator StartDurationCheck(float duration)
        {
            Debug.Log($"마법돌맹이 지속시간 : {duration}");

            yield return new WaitForSeconds(duration);

            Debug.Log($"마법돌맹이 지속시간 종료");
            PoolManager.Instance.Push(this.GetComponent<Poolable>());
        }
    }
}
