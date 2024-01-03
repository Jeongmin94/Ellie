using System.Collections;
using Boss1.DataScript.Minion;
using Channels.Combat;
using Channels.Components;
using Managers.Pool;
using Player.HitComponent;
using UnityEngine;
using Utils;

namespace Boss1.TerrapupaMinion
{
    public class TerrapupaMinionBehaviourController : BehaviourTreeController
    {
        [SerializeField] private TerrapupaMinionHealthBar healthBar;
        [SerializeField] private TerrapupaMinionWeakPoint[] weakPoints;

        private MaterialHitComponent hitComponent;
        private TicketMachine ticketMachine;
        
        private bool isDead;

        public TerrapupaMinionRootData MinionData
        {
            get; private set;
        }
        
        public TerrapupaMinionHealthBar HealthBar
        {
            get { return healthBar; }
        }

        protected override void Awake()
        {
            base.Awake();

            MinionData = rootTreeData as TerrapupaMinionRootData;
            healthBar = gameObject.GetOrAddComponent<TerrapupaMinionHealthBar>();
            weakPoints = GetComponentsInChildren<TerrapupaMinionWeakPoint>();
            hitComponent = gameObject.GetComponent<MaterialHitComponent>();

            SubscribeEvent();
        }

        private void Start()
        {
            InitStatus();
        }

        private void SubscribeEvent()
        {
            foreach (var weakPoint in weakPoints)
            {
                weakPoint.SubscribeCollisionAction(OnCollidedCoreByPlayerStone);
            }
        }

        public void InitTicketMachine(TicketMachine ticketMachine)
        {
            this.ticketMachine = ticketMachine;
        }

        private void InitStatus()
        {
            healthBar.InitData(MinionData);
        }

        private void OnCollidedCoreByPlayerStone(IBaseEventPayload payload)
        {
            Debug.Log($"ReceiveDamage :: {payload}");

            if (payload is CombatPayload combatPayload)
            {
                PoolManager.Instance.Push(combatPayload.Attacker.GetComponent<Poolable>());
                var damage = combatPayload.Damage;

                ApplyDamage(damage);
                Debug.Log($"{damage} 데미지 입음 : {MinionData.currentHP.Value}");
            }
        }

        public void ApplyDamage(int damageValue)
        {
            if (!isDead)
            {
                StartCoroutine(ShakeCoroutine(damageValue, MinionData.cameraShakeDuration));
                healthBar.ShowBillboard();
                hitComponent.Hit();

                healthBar.RenewHealthBar(MinionData.currentHP.value - damageValue);
                MinionData.currentHP.Value -= damageValue;

                if (MinionData.currentHP.value <= 0)
                {
                    Dead();
                }

                MinionData.isHit.Value = true;
            }
        }

        public void ApplyHeal(int healValue)
        {
            if (!isDead)
            {
                healthBar.RenewHealthBar(MinionData.currentHP.value + healValue);
                MinionData.currentHP.Value += healValue;

                if (MinionData.currentHP.value > MinionData.hp)
                {
                    healthBar.ShowBillboard();
                    MinionData.currentHP.Value = MinionData.hp;
                    healthBar.RenewHealthBar(MinionData.currentHP.value);
                }
            }
        }

        public void Dead()
        {
            isDead = true;
            MinionData.currentHP.Value = 0;
            healthBar.RenewHealthBar(0);
            healthBar.BillboardObject.gameObject.SetActive(false);
        }
        
        private IEnumerator ShakeCoroutine(float shakeIntensity, float shakeDuration)
        {
            var elapsed = 0.0f;

            var originalPosition = transform.position;

            while (elapsed < shakeDuration)
            {
                transform.position = originalPosition + Random.insideUnitSphere * shakeIntensity;
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = originalPosition;
        }
    }
}
