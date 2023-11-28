﻿using Assets.Scripts.Channels.Item;
using Assets.Scripts.Combat;
using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.Particle;
using System;
using UnityEngine;

namespace Assets.Scripts.Item.Stone
{
    public class BaseStoneEffect : MonoBehaviour
    {
        public StoneEventType Type { get; set; }

        private event Action<Transform> effectAction;
        protected StoneData data;
        private int collisionCount = 1;
        private LayerMask layerMask;

        private void Start()
        {
            int exceptGroundLayer = LayerMask.NameToLayer("ExceptGround");
            int monsterLayer = LayerMask.NameToLayer("Monster");

            layerMask = (1 << exceptGroundLayer) | (1 << monsterLayer);
        }
        private void OnDisable()
        {
            effectAction = null;
            collisionCount = 1;
        }

        public void InitData(StoneData data)
        {
            this.data = data;
        }

        public void SubscribeAction(Action<Transform> action)
        {
            effectAction -= action;
            effectAction += action;
        }

        public virtual void OccurEffect(Transform transform)
        {
            effectAction?.Invoke(transform);
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            if(collisionCount > 0)
            {
                ParticleManager.Instance.GetParticle(data.hitParticle, new ParticlePayload
                {
                    Position = transform.position,
                    Rotation = transform.rotation,
                });
                collisionCount--;
            }
            
            foreach (ContactPoint contact in collision.contacts)
            {
                GameObject hitObject = contact.otherCollider.gameObject;
                if ((layerMask.value & (1 << hitObject.layer)) == 0)
                {
                    continue;
                }

                ICombatant enemy = hitObject.GetComponentInChildren<ICombatant>();

                if (enemy != null && !hitObject.CompareTag("Player"))
                {
                    Debug.Log($"NormalStone OnCollisionEnter :: ICombatant OK {collision.gameObject.name}");
                    OccurEffect(hitObject.transform);

                    break;
                }
            }
        }

        
    }
}