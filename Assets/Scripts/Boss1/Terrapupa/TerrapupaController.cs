using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using Channels.Boss;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using Codice.Client.Common.GameUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss.Terrapupa
{
    public class TerrapupaController : BehaviourTreeController
    {
        [HideInInspector] public TerrapupaRootData terrapupaData;
        [SerializeField] private Transform stone;
        [SerializeField] private TerrapupaWeakPoint weakPoint;
        [SerializeField] private TerrapupaHealthBar healthBar;

        private readonly Dictionary<TerrapupaAttackType, Coroutine> attackCooldown = new Dictionary<TerrapupaAttackType, Coroutine>();
        
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

        public Dictionary<TerrapupaAttackType, Coroutine> AttackCooldown
        {
            get { return attackCooldown; }
        }

        private void Awake()
        {
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
            GetHealed(1);
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

        public void Cooldown(float cooldown, TerrapupaAttackType type)
        {
            // 마나의 샘 공격 봉인 겹치는거 방지용 딕셔너리 설정
            attackCooldown[type] = StartCoroutine(ApplyCooldown(cooldown, type));
        }

        private IEnumerator ApplyCooldown(float cooldown, TerrapupaAttackType type)
        {
            Debug.Log($"{gameObject} {cooldown}초 Cooldown 시작 : {type} 공격 봉인 적용");
            ApplyAttackAvailable(type, false);

            yield return new WaitForSeconds(cooldown);
            Debug.Log($"{gameObject} {cooldown}초 Cooldown 해제 : {type} 공격 봉인 해제");

            // 마나의 샘 공격 봉인 겹치는거 방지용 딕셔너리 설정
            attackCooldown[type] = null;
            ApplyAttackAvailable(type, true);
        }

        public void ApplyAttackAvailable(TerrapupaAttackType type, bool isCooldownDone)
        {
            switch (type)
            {
                case TerrapupaAttackType.ThrowStone:
                    if (terrapupaData.stoneUsable) terrapupaData.canThrowStone.Value = isCooldownDone;
                    break;
                case TerrapupaAttackType.EarthQuake:
                    if (terrapupaData.earthQuakeUsable) terrapupaData.canEarthQuake.Value = isCooldownDone;
                    break;
                case TerrapupaAttackType.Roll:
                    if (terrapupaData.rollUsable) terrapupaData.canRoll.Value = isCooldownDone;
                    break;
                case TerrapupaAttackType.LowAttack:
                    if (terrapupaData.lowAttackUsable) terrapupaData.canLowAttack.Value = isCooldownDone;
                    break;
                default:
                    break;
            }
        }

        public void GetDamaged(int damageValue)
        {
            healthBar.RenewHealthBar(terrapupaData.currentHP.Value - damageValue);
            terrapupaData.currentHP.Value -= damageValue;
        }

        public void GetHealed(int healValue)
        {
            healthBar.RenewHealthBar(terrapupaData.currentHP.value + healValue);
            terrapupaData.currentHP.Value += healValue;

            if(terrapupaData.currentHP.value > terrapupaData.hp)
            {
                terrapupaData.currentHP.Value = terrapupaData.hp;
                healthBar.RenewHealthBar(terrapupaData.currentHP.value);
            }
        }
    }
}