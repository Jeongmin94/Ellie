using Assets.Scripts.Data.ActionData.Player;
using Assets.Scripts.StatusEffects;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerStatus : MonoBehaviour
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

        public bool isRecoveringStamina;

        private Dictionary<PlayerStatusEffectName, IPlayerStatusEffect> playerStatusEffects;
        private PlayerStatusEffectController playerStatusEffectController;

        float temp;


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

        public int Stamina
        {
            get { return staminaData.CurrentStamina.Value; }
            set
            {
                staminaData.CurrentStamina.Value = value;
            }
        }
        private void Awake()
        {
            playerStatusEffectController = GetComponent<PlayerStatusEffectController>();
            playerStatusEffects = new();
            healthData.InitHealth();
            

            InitStatusEffects();
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
        }

        private void ApplyStatusEffect(PlayerStatusEffectName name)
        {
            if (playerStatusEffects.TryGetValue(name, out IPlayerStatusEffect effect))
                playerStatusEffectController.ApplyStatusEffect(effect);
        }

        private void RecoverStamina()
        {
            if (!isRecoveringStamina || Stamina >= MaxStamina) return;
            temp += staminaRecoveryPerSec * Time.deltaTime;
            if (temp >= 1f)
            {
                Stamina++;
                temp = 0;
            }
        }

        public void ConsumeStamina(int consumedStamina)
        {
            if (Stamina - consumedStamina < 0)
                Stamina = 0;
            else
                Stamina -= consumedStamina;
        }
    }
}