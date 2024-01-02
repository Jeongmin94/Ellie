using System.Collections;
using Assets.Scripts.Managers;
using Channels.Boss;
using Channels.Components;
using Channels.Stone;
using Channels.Type;
using Managers.Event;
using Managers.Particle;
using Managers.Sound;
using UnityEngine;

namespace Boss1.BossRoomObjects
{
    public class ManaFountain : MonoBehaviour
    {
        [SerializeField] private GameObject hitEffect;
        [SerializeField] private Light lightComponent;
        [SerializeField] private string manaHitSound = "ManaFountainHit";
        [SerializeField] private string manaRegenerateSound = "ManaFountainRegen";

        public float coolDownValue = 3.0f;
        public float respawnValue = 3.0f;
        public float lightIntensity = 15.0f;
        public float changeLightTime = 1.0f;
        public TerrapupaAttackType banBossAttackType;

        private readonly int MAGICSTONE_INDEX = 4020;
        private readonly int NORMALSTONE_INDEX = 4000;

        private TicketMachine ticketMachine;

        public bool IsCooldown { get; set; }

        public bool IsBroken { get; set; }

        private void Awake()
        {
            lightComponent = GetComponentInChildren<Light>();
        }
        
        public void InitTicketMachine(TicketMachine ticketMachine)
        {
            this.ticketMachine = ticketMachine;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsBroken)
            {
                var target = other.transform;
                if (other.transform.CompareTag("Stone") && !IsCooldown)
                {
                    Debug.Log($"{other.name} 돌맹이 충돌");
                    SpawnMagicStone(target.position);
                }
                else if (other.transform.CompareTag("Boss"))
                {
                    Debug.Log($"{other.name} 보스 충돌");
                    DestroyManaFounatainByBoss(target.position, target);
                }
                else if (other.transform.CompareTag("BattleObject"))
                {
                    Debug.Log($"{other.name} 보스 바위 충돌");
                    DestroyManaFountainByBossStone(target.position, target);
                }
            }
        }

        public void DestroyManaFountain()
        {
            IsBroken = true;
            IsCooldown = false;
            lightComponent.intensity = lightIntensity;
            StopAllCoroutines();
        }

        public void RegenerateManaFountain()
        {
            IsBroken = false;
            IsCooldown = false;

            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, manaRegenerateSound, transform.position);
        }

        private void SpawnMagicStone(Vector3 position)
        {
            IsCooldown = true;

            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, manaHitSound, transform.position);

            EventBus.Instance.Publish(EventBusEvents.HitManaByPlayerStone,
                new BossEventPayload
                {
                    TransformValue1 = transform
                });
            
            for (int i = 0; i < 3; i++)
            {
                StoneChannel.DropStone(ticketMachine, position, MAGICSTONE_INDEX);
            }
        }

        private void DestroyManaFounatainByBoss(Vector3 position, Transform other)
        {
            IsBroken = true;

            EventBus.Instance.Publish(EventBusEvents.DestroyedManaByBoss1,
                new BossEventPayload
                {
                    TransformValue1 = transform,
                    AttackTypeValue = banBossAttackType,
                    Sender = other.root
                });
            
            ParticleManager.Instance.GetParticle(hitEffect, position, 0.7f);
            for (int i = 0; i < 3; i++)
            {
                StoneChannel.DropStone(ticketMachine, position, NORMALSTONE_INDEX);
            }
        }

        private void DestroyManaFountainByBossStone(Vector3 position, Transform other)
        {
            IsBroken = true;

            EventBus.Instance.Publish(EventBusEvents.DestroyedManaByBoss1,
                new BossEventPayload
                {
                    TransformValue1 = transform,
                    TransformValue2 = other,
                    AttackTypeValue = banBossAttackType,
                    Sender = other
                });

            ParticleManager.Instance.GetParticle(hitEffect, position, 0.7f);
            for (int i = 0; i < 3; i++)
            {
                StoneChannel.DropStone(ticketMachine, position, NORMALSTONE_INDEX);
            }
        }

        public void SetLightIntensity(float targetIntensity, float duration)
        {
            StartCoroutine(ChangeIntensity(targetIntensity, duration));
        }

        private IEnumerator ChangeIntensity(float targetIntensity, float duration)
        {
            var startIntensity = lightComponent.intensity;
            float time = 0;

            while (time < duration)
            {
                lightComponent.intensity = Mathf.Lerp(startIntensity, targetIntensity, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            lightComponent.intensity = targetIntensity;
        }
    }
}
