using Assets.Scripts.Channels.Item;
using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.Managers;
using Assets.Scripts.Particle;
using Channels.Boss;
using Channels.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Item.Stone
{
    public class PortalStone : BaseStoneEffect
    {
        public static int portalNumber = 0;

        public GameObject particleEffect;
        public float portalRadius = 5.0f;
        public float duration = 5.0f;

        private int portalID = 0;

        private ParticleController portalParticle;
        private Rigidbody rb;

        private MeshCollider meshCollider;
        private Coroutine durationCoroutine;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            meshCollider = GetComponent<MeshCollider>();
        }

        public override void InitData(StoneData data)
        {
            base.InitData(data);

            particleEffect = data.skillEffectParticle;
        }

        private void OnDisable()
        {
            // 포탈 존재 O
            if (portalParticle != null)
            {
                portalParticle.Stop();
                portalParticle = null;
            }

            if(rb != null)
            {
                rb.isKinematic = false;
            }
        }

        protected override void OnCollisionEnter(Collision collision)
        {
            if (portalNumber < 2 && Type == StoneEventType.ShootStone
                && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                if (collision.gameObject.CompareTag("Ground"))
                {
                    ActivatePortal();
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

        public void ActivatePortal()
        {
            Debug.Log("포탈 활성화");

            // 콜라이더 제거 + 중력 제거
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            meshCollider.enabled = false;

            // 지속시간 설정 + 스킬 이펙트 추가
            durationCoroutine = StartCoroutine(StartDurationCheck(duration));
        }

        private IEnumerator StartDurationCheck(float duration)
        {
            Debug.Log($"포탈 지속시간 : {duration}");
            portalNumber++;
            portalID = portalNumber;
            Debug.Log($"{portalID} : {portalNumber}개 존재");

            portalParticle = ParticleManager.Instance.GetParticle(particleEffect, new ParticlePayload
            {
                IsLoop = true,
                Position = transform.position,
                Offset = new Vector3(0.0f, 2.0f, 0.0f),
            }).GetComponent<ParticleController>();
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "Stone_Sound_2", transform.position);

            yield return new WaitForSeconds(duration);

            Debug.Log($"포탈 지속시간 종료");
            portalNumber--;
            Debug.Log($"{portalID} : {portalNumber}개 존재");
            PoolManager.Instance.Push(this.GetComponent<Poolable>());

            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "Stone_Sound_2", transform.position);
        }
    }
}
