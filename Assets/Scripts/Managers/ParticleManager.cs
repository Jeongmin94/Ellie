using Assets.Scripts.Managers;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Particle
{
    public class ParticlePayload : IBaseEventPayload
    {
        public Transform Origin { get; set; } = null;
        public bool IsLoop { get; set; } = false;
        public bool IsFollowOrigin { get; set; } = false;
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; } = Vector3.one;
        public Vector3 Offset { get; set; }
        public int LoopCount { get; set; } = 1;
    }

    public class ParticleManager : Singleton<ParticleManager>
    {
        [ShowInInspector] [ReadOnly] private List<ParticleController> particles = new List<ParticleController>();

        private Transform particleRoot;

        public override void Awake()
        {
            base.Awake();

            GameObject go = new GameObject();
            go.transform.parent = transform;
            go.name = "@Particle_Root";
            particleRoot = go.transform;
        }

        public override void ClearAction()
        {
            base.ClearAction();

            foreach (var particle in particles)
            {
                particle.Stop();
            }
        }

        public void ReturnToPool(ParticleController controller)
        {
            if (particles.Remove(controller))
            {
                PoolManager.Instance.Push(controller);
            }
            else
            {
                Debug.Log($"{controller.name} 파티클 회수 실패, Particles {particles.Count}");
            }
        }
        
        public GameObject GetParticle(GameObject prefab, ParticlePayload payload)
        {
            if (prefab == null)
            {
                Debug.LogError("ParticleManager :: Null Prefab");
                return null;
            }

            var particle = PoolManager.Instance.Pop(prefab, particleRoot);

            if (payload.Origin != null)
            {
                // Origin의 로컬 좌표를 기준으로 추가 오프셋 벡터 설정
                Vector3 transformedOffset = payload.Origin.TransformDirection(payload.Offset);
                particle.transform.position = payload.Origin.position + transformedOffset;
                particle.transform.rotation = payload.Origin.rotation;
            }
            else
            {
                particle.transform.position = payload.Position + payload.Offset;
                particle.transform.rotation = payload.Rotation;
            }

            particle.transform.localScale = payload.Scale;

            var controller = particle.transform.gameObject.AddComponent<ParticleController>();
            controller.Init(payload);

            particles.Add(controller);

            return particle.gameObject;
        }

        public GameObject GetParticle(GameObject prefab, Transform target, float scale = 1.0f, int loopCount = 1)
        {
            return GetParticle(prefab, new ParticlePayload
            {
                Position = target.position,
                Rotation = target.rotation,
                Scale = new Vector3(scale, scale, scale),
                LoopCount = loopCount,
            });
        }

        public GameObject GetParticle(GameObject prefab, Vector3 position, float scale = 1.0f, int loopCount = 1)
        {
            return GetParticle(prefab, new ParticlePayload
            {
                Position = position,
                Scale = new Vector3(scale, scale, scale),
                LoopCount = loopCount,
            });
        }
    }
}