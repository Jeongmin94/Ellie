using Assets.Scripts.Particle;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class VFXPayload : IBaseEventPayload
    {
        public Transform Origin { get; set; } = null;
        public bool IsFollowOrigin { get; set; } = false;
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; } = Vector3.one;
        public Vector3 Offset { get; set; }
    }

    public class VFXManager : Singleton<VFXManager>
    {
        [ShowInInspector][ReadOnly] private List<VFXController> vfxControllers = new List<VFXController>();

        private Transform vfxRoot;

        public override void Awake()
        {
            base.Awake();

            GameObject go = new GameObject("@VFX_Root");
            go.transform.parent = transform;
            vfxRoot = go.transform;
        }

        public override void ClearAction()
        {
            base.ClearAction();

            foreach (var vfx in vfxControllers)
            {
                vfx.Stop();
            }
        }

        public void ReturnToPool(VFXController controller)
        {
            if (vfxControllers.Remove(controller))
            {
                PoolManager.Instance.Push(controller);
            }
            else
            {
                Debug.Log($"{controller.name} VFX 회수 실패, VFXControllers {vfxControllers.Count}");
            }
        }

        public GameObject GetVFX(GameObject prefab, VFXPayload payload)
        {
            if (prefab == null)
            {
                Debug.LogError("VFXManager :: Null Prefab");
                return null;
            }

            var vfx = PoolManager.Instance.Pop(prefab, vfxRoot);

            if (payload.Origin != null)
            {
                Vector3 transformedOffset = payload.Origin.TransformDirection(payload.Offset);
                vfx.transform.position = payload.Origin.position + transformedOffset;
                vfx.transform.rotation = payload.Origin.rotation;
            }
            else
            {
                vfx.transform.position = payload.Position + payload.Offset;
                vfx.transform.rotation = payload.Rotation;
            }

            vfx.transform.localScale = payload.Scale;

            var controller = vfx.transform.gameObject.AddComponent<VFXController>();
            controller.Init(payload);

            vfxControllers.Add(controller);

            return vfx.gameObject;
        }

        public GameObject GetVFX(GameObject prefab, Transform target, float scale)
        {
            return GetVFX(prefab, new VFXPayload
            {
                Position = target.position,
                Rotation = target.rotation,
                Scale = new Vector3(scale, scale, scale),
            });
        }

        public GameObject GetVFX(GameObject prefab, Vector3 position, float scale)
        {
            return GetVFX(prefab, new VFXPayload
            {
                Position = position,
                Scale = new Vector3(scale, scale, scale),
            });
        }
    }
}