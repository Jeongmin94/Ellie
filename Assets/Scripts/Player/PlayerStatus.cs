using Assets.Scripts.Combat;
using Assets.Scripts.Data.ActionData.Player;
using Assets.Scripts.Player.StatusEffects.StatusEffectConcreteStrategies;
using Assets.Scripts.StatusEffects;
using Assets.Scripts.StatusEffects.StatusEffectConcreteStrategies;
using Assets.Scripts.UI.Framework.Images;
using Assets.Scripts.Utils;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerStatus : MonoBehaviour, ICombatant
    {
        [Header("Player Properties")]
        [SerializeField] private int maxHP;
        [SerializeField] private int maxStamina;

        [Header("Stamina Consumption")]
        [SerializeField] private int staminaRecoveryPerSec;
        [SerializeField] private int sprintStaminaConsumptionPerSec;
        [SerializeField] private int jumpStaminaConsumption;
        [SerializeField] private int dodgeStaminaConsumption;
        [SerializeField] private int hangStaminaConsumptionPerSec;

        [SerializeField] private int sprintStanimaThreshold;

        [Header("Combat")]
        [SerializeField] private PlayerHealthData healthData;
        [SerializeField] private StaminaData staminaData;

        public int MaxHP { get { return maxHP; } }
        public int MaxStamina { get { return maxStamina; } }
        public int StaminaRecoveryPerSec { get { return staminaRecoveryPerSec; } }
        public int SprintStaminaConsumptionPerSec { get { return sprintStaminaConsumptionPerSec; } }
        public int JumpStaminaConsumption { get { return jumpStaminaConsumption; } }
        public int DodgeStaminaConsumption { get { return dodgeStaminaConsumption; } }
        public int HangStaminaConsumption { get { return HangStaminaConsumption; } }

        public int SprintStaminaThreshold { get { return sprintStanimaThreshold; } }

        public bool isDead;
        public bool isRecoveringStamina;

        private Dictionary<PlayerStatusEffectName, IPlayerStatusEffect> playerStatusEffects;
        private PlayerStatusEffectController playerStatusEffectController;

        float tempStamina;

        private PlayerUI playerUI;
        private TicketMachine ticketMachine;


        // !TODO : ICombatant를 붙이고, StatusEffectController를 참조하여 ApplyStatusEffect를 해주기
        // !TODO : Battle 채널에 구독될 수 있는 티켓 포함
        public int HP
        {
            get { return healthData.CurrentHealth.Value; }
            set
            {
                healthData.CurrentHealth.Value = value;
            }
        }

        public float Stamina
        {
            get { return staminaData.CurrentStamina.Value; }
            set
            {
                staminaData.CurrentStamina.Value = value;
            }
        }
        private void Awake()
        {
            SetTicketMachine();
            playerStatusEffectController = GetComponent<PlayerStatusEffectController>();
            playerStatusEffects = new();
            healthData.InitHealth();
            playerUI = GetComponent<PlayerUI>();

            InitStatusEffects();
        }
        private void SetTicketMachine()
        {
            Debug.Log("Player SetTicketMachine()");
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.Combat);
            isDead = false;
        }
        private void Start()
        {
            isRecoveringStamina = true;
        }
        private void Update()
        {
            RecoverStamina();
        }
        private void InitStatusEffects()
        {
            // !TODO : 상태이상들 객체 생성, 리스트에 담아두기
            playerStatusEffects.Add(PlayerStatusEffectName.Burn, playerStatusEffectController.gameObject.AddComponent<PlayerStatusEffectBurn>());
            playerStatusEffects.Add(PlayerStatusEffectName.WeakRigidity, playerStatusEffectController.gameObject.AddComponent<PlayerStatusEffectWeakRigidity>());
        }

        private void RecoverStamina()
        {
            if (!isRecoveringStamina || Stamina >= MaxStamina) return;
            tempStamina += staminaRecoveryPerSec * Time.deltaTime;
            if (tempStamina >= 1f)
            {
                Stamina++;
                tempStamina = 0;
            }
        }

        public void ConsumeStamina(float consumedStamina)
        {
            Color startColor = playerUI.StaminaBarImage.MidgroundStartColor;
            if (Stamina - consumedStamina < 0)
                Stamina = 0;
            else
                Stamina -= consumedStamina;


            if (consumedStamina > 10.0f)
            {
                playerUI.StaminaBarImage.MidgroundColor = startColor;
                StartCoroutine(CheckMidAndForegroundSliderValue());
            }
        }
        public void ReduceHP(int damage)
        {
            if (HP <= damage && !isDead)
            {
                HP = 0;
                isDead = true;
                //HP -= damage;
                gameObject.GetComponent<PlayerController>().ChangeState(PlayerStateName.Dead);
            }
            else
            {
                HP -= damage;
            }
        }
        private IEnumerator CheckMidAndForegroundSliderValue()
        {
            yield return playerUI.StaminaBarImage.CheckSliderValue(FillAmountType.Midground, FillAmountType.Foreground);
            Color color = Color.white;
            color.a = 0f;
            playerUI.StaminaBarImage.MidgroundColor = color;
        }

        public void Attack(IBaseEventPayload payload)
        {

        }

        public void ReceiveDamage(IBaseEventPayload payload)
        {
            Debug.Log("Player recieve Damage");
            CombatPayload combatPayload = payload as CombatPayload;
            if (combatPayload.PlayerStatusEffectName != PlayerStatusEffectName.None)
            {
                Debug.Log("Player : RecieveDamage");
                IPlayerStatusEffect effect;
                playerStatusEffects.TryGetValue(combatPayload.PlayerStatusEffectName, out effect);
                playerStatusEffectController.ApplyStatusEffect(effect);
            }
            //hp처리 로직
            ReduceHP(combatPayload.Damage);
        }
    }

}