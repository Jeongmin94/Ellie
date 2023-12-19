using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.VFX;
using VFXManager = Assets.Scripts.Managers.VFXManager;

namespace Assets.Scripts.Particle
{
    public class VFXController : Poolable
    {
        public VisualEffect VFX { get; private set; }

        private Transform origin;
        private bool isFollowOrigin;

        private void Awake()
        {
            VFX = GetComponent<VisualEffect>();
        }

        private void Update()
        {
            if (VFX && VFX.aliveParticleCount == 0)
            {
                VFXManager.Instance.ReturnToPool(this);
            }

            if (isFollowOrigin)
            {
                transform.position = origin.transform.position;
                transform.rotation = origin.transform.rotation;
            }
        }

        public void Init(VFXPayload payload)
        {
            origin = payload.Origin;
            isFollowOrigin = payload.IsFollowOrigin;

            VFX.Play();
        }

        public void Stop()
        {
            VFX.Stop();
        }

        public override void PoolableDestroy()
        {
            Destroy(this.gameObject);
        }
    }
}