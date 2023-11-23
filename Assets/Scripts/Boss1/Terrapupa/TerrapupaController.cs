using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using System.Collections;
using UnityEngine;

namespace Boss.Terrapupa
{
    public class TerrapupaController : BehaviourTreeController
    {
        [HideInInspector] public TerrapupaRootData terrapupaData;
        [SerializeField] private Transform stone;
        [SerializeField] private TerrapupaWeakPoint weakPoint;
        [SerializeField] private TerrapupaHealthBar healthBar;
        
        private TicketMachine ticketMachine;

        public Transform Stone
        {
            get { return stone; }
            set { stone = value; }
        }

        public TerrapupaWeakPoint WeakPoint
        {
            get { return weakPoint; }
            set { weakPoint = value; }
        }

        public TicketMachine TicketMachine
        {
            get { return ticketMachine; }
        }

        public TerrapupaHealthBar HealthBar
        { 
            get { return healthBar; }
        }

        protected override void Awake()
        {
            base.Awake();
            terrapupaData = rootTreeData as TerrapupaRootData;
            healthBar = gameObject.GetOrAddComponent<TerrapupaHealthBar>();

            InitTicketMachine();
            SubscribeEvent();
        }

        private void Start()
        {
            InitStatus();
        }

        private void InitTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.Combat, ChannelType.Terrapupa, ChannelType.BossInteraction);
        }

        private void InitStatus()
        {
            healthBar.InitData(terrapupaData);
        }

        private void SubscribeEvent()
        {
            weakPoint.SubscribeCollisionAction(OnCollidedCoreByPlayerStone);
        }

        private void OnCollidedCoreByPlayerStone(IBaseEventPayload payload)
        {
            // 플레이어 총알 -> Combat Channel -> TerrapupaWeakPoint :: ReceiveDamage() -> TerrapupaController
            Debug.Log($"OnCollidedCoreByPlayerStone :: {payload}");

            if(terrapupaData.isStuned.value)
            {
                CombatPayload combatPayload = payload as CombatPayload;
                PoolManager.Instance.Push(combatPayload.Attacker.GetComponent<Poolable>());
                int damage = combatPayload.Damage;

                GetDamaged(damage);
                Debug.Log($"기절 상태, {damage} 데미지 입음 : {terrapupaData.currentHP.Value}");
                
            }
            else 
            {
                Debug.Log("기절 상태가 아님");
            }
        }

        public void GetDamaged(int damageValue)
        {
            terrapupaData.currentHP.Value -= damageValue;
            healthBar.RenewHealthBar(terrapupaData.currentHP.value);
        }

        public void GetHealed(int healValue)
        {
            terrapupaData.currentHP.Value += healValue;
            if(terrapupaData.currentHP.value > terrapupaData.hp)
            {
                terrapupaData.currentHP.Value = terrapupaData.hp;
            }
            healthBar.RenewHealthBar(terrapupaData.currentHP.value);
        }
    }
}