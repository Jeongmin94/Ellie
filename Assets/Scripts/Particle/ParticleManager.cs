using Assets.Scripts.Managers;
using System.Collections;
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
        public GameObject GetParticle(GameObject prefab, ParticlePayload payload)
        {
            if (prefab == null)
            {
                Debug.LogError("ParticleManager :: Null Prefab");   
                return null;
            }

            var particle = PoolManager.Instance.Pop(prefab);

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

            Debug.Log(particle);

            return particle.gameObject;
        }
    }
}