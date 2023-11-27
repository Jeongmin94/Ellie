using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Particle
{
    public class ParticleController : Poolable
    {
        public ParticleSystem ps { get; private set; }

        private Transform origin;
        private bool isFollowOrigin;

        private void Awake()
        {
            ps = GetComponent<ParticleSystem>();
            var main = ps.main;
            main.playOnAwake = true;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        private void LateUpdate()
        {
            if(isFollowOrigin)
            {
                transform.position = origin.transform.position;
                transform.rotation = origin.transform.rotation;
            }
        }

        public void Init(ParticlePayload payload)
        {
            var main = ps.main;

            origin = payload.Origin;
            main.loop = payload.IsLoop;
            isFollowOrigin = payload.IsFollowOrigin;

            ps.Play();
        }

        public void Stop()
        {
            var main = ps.main;
            main.loop = false;
            ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        private void OnParticleSystemStopped()
        {
            Debug.Log("종료 확인");
            PoolManager.Instance.Push(this);
        }
    }
}