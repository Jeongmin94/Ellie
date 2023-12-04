using Assets.Scripts.Managers;
using Sirenix.OdinInspector;
using System.Collections;
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
    }

    public class ParticleManager : Singleton<ParticleManager>
    {
        [ShowInInspector][ReadOnly] private List<Poolable> particles = new List<Poolable>();

        public override void ClearAction()
        {
            foreach (var particle in particles)
            {
                PoolManager.Instance.Push(particle);
            }
        }

        public void RemoveParticleFromList(Poolable target)
        {
            Debug.Log(particles.Count);
            particles.Remove(target);
            Debug.Log(particles.Count);
        }

        public GameObject GetParticle(GameObject prefab, ParticlePayload payload)
        {
            if (prefab == null)
            {
                Debug.LogError("ParticleManager :: Null Prefab");   
                return null;
            }

            var particle = PoolManager.Instance.Pop(prefab);
            particles.Add(particle);

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

            return particle.gameObject;
        }

        public GameObject GetParticle(GameObject prefab, Transform target, float scale)
        {
            return GetParticle(prefab, new ParticlePayload
            {
                Position = target.position,
                Rotation = target.rotation,
                Scale = new Vector3(scale, scale, scale),
            });
        }

        public GameObject GetParticle(GameObject prefab, Vector3 position, float scale)
        {
            return GetParticle(prefab, new ParticlePayload
            {
                Position = position,
                Scale = new Vector3(scale, scale, scale),
            });
        }
    }
}