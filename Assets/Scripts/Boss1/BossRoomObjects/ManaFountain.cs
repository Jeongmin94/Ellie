using Assets.Scripts.Channels.Item;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using Channels.Boss;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using System.Collections;
using UnityEngine;

namespace Boss.Objects
{
    public class ManaFountain : MonoBehaviour
    {
        [SerializeField] private Transform spawnPosition;
        [SerializeField] private GameObject hitEffect;
        [SerializeField] private Light lightComponent;
        [SerializeField] private string manaHitSound = "ManaFountainHit";
        [SerializeField] private string manaRegenerateSound = "ManaFountainRegen";

        public float coolDownValue = 3.0f;
        public float respawnValue = 3.0f;
        public float lightIntensity = 15.0f;
        public float changeLightTime = 1.0f;
        public TerrapupaAttackType banBossAttackType;

        private bool isCooldown;
        private bool isBroken;

        public readonly int NORMALSTONE_INDEX = 4000;
        public readonly int MAGICSTONE_INDEX = 4020;
        private TicketMachine ticketMachine;

        public Vector3 SpawnPosition
        {
            get { return spawnPosition.position; }
        }

        public bool IsCooldown
        {
            get { return isCooldown; }
            set { isCooldown = value; }
        }

        public bool IsBroken
        {
            get { return isBroken; }
            set { isBroken = value; }
        }

        private void Awake()
        {
            lightComponent = GetComponentInChildren<Light>();
        }
        
        public void InitTicketMachine(TicketMachine ticketMachine)
        {
            this.ticketMachine = ticketMachine;
        }
        
        public void DestroyManaFountain()
        {
            IsBroken = true;
            isCooldown = false;
            lightComponent.intensity = lightIntensity;
            StopAllCoroutines();
        }

        public void RegenerateManaFountain()
        {
            IsBroken = false;
            IsCooldown = false;
            
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, manaRegenerateSound, transform.position);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isBroken)
            {
                if (other.transform.CompareTag("Stone") && !isCooldown)
                {
                    Debug.Log($"{other.name} 돌맹이 충돌");
                    SpawnMagicStone();
                }
                else if (other.transform.CompareTag("Boss"))
                {
                    Debug.Log($"{other.name} 보스 충돌");
                    DestroyManaFounatainByBoss(other.transform);
                }
                else if (other.transform.CompareTag("BattleObject"))
                {
                    Debug.Log($"{other.name} 보스 바위 충돌");
                    DestroyManaFountainByBossStone(other.transform);
                }
            }
        }
        
        private void SpawnMagicStone()
        {
            isCooldown = true;

            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, manaHitSound, transform.position);

            EventBus.Instance.Publish(EventBusEvents.HitManaByPlayerStone,
                new BossEventPayload
                {
                    TransformValue1 = transform,
                });
        }

        private void DestroyManaFounatainByBoss(Transform other)
        {
            isBroken = true;

            EventBus.Instance.Publish(EventBusEvents.DestroyedManaByBoss1,
                new BossEventPayload
                {
                    PrefabValue = hitEffect,
                    TransformValue1 = transform,
                    AttackTypeValue = banBossAttackType,
                    Sender = other.transform.root,
                });
        }

        private void DestroyManaFountainByBossStone(Transform other)
        {
            isBroken = true;

            EventBus.Instance.Publish(EventBusEvents.DestroyedManaByBoss1,
                new BossEventPayload
                {
                    PrefabValue = hitEffect,
                    TransformValue1 = transform,
                    TransformValue2 = other.transform,
                    AttackTypeValue = banBossAttackType,
                    Sender = other.transform,
                });

            Debug.Log("Mine Stone : " + MAGICSTONE_INDEX.ToString());
            for (int i = 0; i < 3; i++)
            {
                ticketMachine.SendMessage(ChannelType.Stone,
                    new StoneEventPayload
                    {
                        Type = StoneEventType.MineStone,
                        StoneSpawnPos = spawnPosition.position,
                        StoneForce = GetRandVector(),
                        StoneIdx = NORMALSTONE_INDEX,
                    });
            }
        }

        private Vector3 GetRandVector()
        {
            Vector3 vec = new(Random.Range(-1.0f, 1.0f), 0.5f, 0);
            return vec.normalized;
        }

        public void SetLightIntensity(float targetIntensity, float duration)
        {
            StartCoroutine(ChangeIntensity(targetIntensity, duration));
        }

        private IEnumerator ChangeIntensity(float targetIntensity, float duration)
        {
            float startIntensity = lightComponent.intensity;
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
