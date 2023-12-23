using System;
using Assets.Scripts.Channels.Item;
using Assets.Scripts.Combat;
using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.Managers;
using Assets.Scripts.Particle;
using Channels.Components;
using UnityEngine;

namespace Assets.Scripts.Item.Stone
{
    public class BaseStoneEffect : MonoBehaviour
    {
        private int collisionCount = 1;

        protected StoneData data;

        private Action<Transform> effectAction;
        private bool isHitEnemy;
        private LayerMask layerMask;
        protected TicketMachine ticketMachine;
        public StoneEventType Type { get; set; }

        private void Start()
        {
            var exceptGroundLayer = LayerMask.NameToLayer("ExceptGround");
            var monsterLayer = LayerMask.NameToLayer("Monster");

            layerMask = (1 << exceptGroundLayer) | (1 << monsterLayer);
        }

        private void OnDisable()
        {
            effectAction = null;
            collisionCount = 1;
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            foreach (var contact in collision.contacts)
            {
                var hitObject = contact.otherCollider.gameObject;
                if ((layerMask.value & (1 << hitObject.layer)) == 0)
                {
                    continue;
                }

                var enemy = hitObject.GetComponentInChildren<ICombatant>();

                if (enemy != null && !hitObject.CompareTag("Player"))
                {
                    Debug.Log($"NormalStone OnCollisionEnter :: ICombatant OK {collision.gameObject.name}");
                    OccurEffect(hitObject.transform);

                    isHitEnemy = true;

                    break;
                }
            }

            if (collisionCount > 0)
            {
                ParticleManager.Instance.GetParticle(data.hitParticle, new ParticlePayload
                {
                    Position = transform.position,
                    Rotation = transform.rotation
                });
                collisionCount--;
                SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "slingshot_sound3", transform.position);
            }
            else
            {
                SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "slingshot_sound4", transform.position);
            }

            if (isHitEnemy && gameObject.activeSelf)
            {
                PoolManager.Instance.Push(GetComponent<Poolable>());
            }
        }

        public virtual void InitData(StoneData data, TicketMachine ticketMachine)
        {
            this.data = data;
            this.ticketMachine = ticketMachine;
        }

        public void SubscribeAction(Action<Transform> action)
        {
            effectAction -= action;
            effectAction += action;
        }

        public virtual void OccurEffect(Transform transform)
        {
            if (collisionCount > 0)
            {
                effectAction?.Invoke(transform);
            }
        }
    }
}