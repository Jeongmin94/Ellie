using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Channels.Camera;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using Combat;
using Data.ActionData.Player;
using Player.HitComponent;
using Player.StatusEffects;
using Player.StatusEffects.StatusEffectConcreteStrategies;
using UI.Framework.Images;
using UnityEngine;

namespace Player
{
    public class PlayerStatus : MonoBehaviour, ICombatant
    {
        [Header("Player Properties")] [SerializeField]
        private int maxHP;

        [SerializeField] private int maxStamina;

        [Header("Stamina Consumption")] [SerializeField]
        private int staminaRecoveryPerSec;

        [SerializeField] private int sprintStaminaConsumptionPerSec;
        [SerializeField] private int jumpStaminaConsumption;
        [SerializeField] private int dodgeStaminaConsumption;
        [SerializeField] private int hangStaminaConsumptionPerSec;
        [SerializeField] private float chargeStaminaConsumptionPerSec;

        [Header("Combat")] [SerializeField] private PlayerHealthData healthData;
        [SerializeField] private StaminaData staminaData;
        [SerializeField] private float invulnerableTimeAfterHit;


        public bool isDead;
        public bool isRecoveringStamina;

        // hit effect
        private MaterialHitComponent hitComponent;
        private Coroutine invulnerableCoroutine;
        private PlayerStatusEffectController playerStatusEffectController;

        private Dictionary<StatusEffectName, IPlayerStatusEffect> playerStatusEffects;

        private PlayerUI playerUI;

        private float tempStamina;
        public TicketMachine ControllerTicketMachine { get; set; }

        public int MaxHP => maxHP;

        public int MaxStamina => maxStamina;

        public int StaminaRecoveryPerSec => staminaRecoveryPerSec;

        public int SprintStaminaConsumptionPerSec => sprintStaminaConsumptionPerSec;

        public int JumpStaminaConsumption => jumpStaminaConsumption;

        public int DodgeStaminaConsumption => dodgeStaminaConsumption;

        public int HangStaminaConsumption => HangStaminaConsumption;

        public float ChargeStaminaComsumptionPerSec => chargeStaminaConsumptionPerSec;


        public int HP
        {
            get => healthData.CurrentHealth.Value;
            set => healthData.CurrentHealth.Value = value;
        }

        public float Stamina
        {
            get => staminaData.CurrentStamina.Value;
            set => staminaData.CurrentStamina.Value = value;
        }

        private void Awake()
        {
            //SetTicketMachine();
            playerStatusEffectController = GetComponent<PlayerStatusEffectController>();
            playerStatusEffects = new Dictionary<StatusEffectName, IPlayerStatusEffect>();
            playerUI = GetComponent<PlayerUI>();

            hitComponent = GetComponent<MaterialHitComponent>();

            InitStatusEffects();
        }

        private void Start()
        {
            isRecoveringStamina = true;
            isDead = false;
        }

        private void Update()
        {
            RecoverStamina();
        }

        public void Attack(IBaseEventPayload payload)
        {
        }

        public void ReceiveDamage(IBaseEventPayload payload)
        {
            if (payload is not CombatPayload combatPayload)
            {
                return;
            }

            //hp처리 로직
            ReduceHP(combatPayload.Damage);
            //상태이상 공격 처리 로직
            if (combatPayload.StatusEffectName != StatusEffectName.None && HP > 0)
            {
                playerStatusEffects.TryGetValue(combatPayload.StatusEffectName, out var effect);
                if (effect != null)
                {
                    playerStatusEffectController.ApplyStatusEffect(effect, GenerateStatusEffectInfo(combatPayload));
                }
            }

            //무적 처리 로직
            if (HP > 0)
            {
                SetPlayerInvulnerable(invulnerableTimeAfterHit);
            }
        }

        private void SetTicketMachine()
        {
            Debug.Log("Player SetTicketMachine()");
        }

        private void InitStatusEffects()
        {
            // !TODO : 상태이상들 객체 생성, 리스트에 담아두기
            playerStatusEffects.Add(StatusEffectName.Burn,
                playerStatusEffectController.gameObject.AddComponent<PlayerStatusEffectBurn>());
            playerStatusEffects.Add(StatusEffectName.WeakRigidity,
                playerStatusEffectController.gameObject.AddComponent<PlayerStatusEffectWeakRigidity>());
            playerStatusEffects.Add(StatusEffectName.StrongRigidity,
                playerStatusEffectController.gameObject.AddComponent<PlayerStatusEffectStrongRigidity>());
            playerStatusEffects.Add(StatusEffectName.Down,
                playerStatusEffectController.gameObject.AddComponent<PlayerStatusEffectDown>());
            playerStatusEffects.Add(StatusEffectName.KnockedAirborne,
                playerStatusEffectController.gameObject.AddComponent<PlayerStatusEffectKnockedAirborne>());

            foreach (var effect in playerStatusEffects.Values)
            {
                effect.InitStatusEffect();
            }
        }

        private void RecoverStamina()
        {
            if (!isRecoveringStamina || Stamina >= MaxStamina)
            {
                return;
            }

            tempStamina += staminaRecoveryPerSec * Time.deltaTime;
            if (tempStamina >= 1f)
            {
                Stamina++;
                tempStamina = 0;
            }
        }

        public void ConsumeStamina(float consumedStamina)
        {
            var startColor = playerUI.StaminaBarImage.MidgroundStartColor;
            if (Stamina - consumedStamina < 0)
            {
                Stamina = 0;
            }
            else
            {
                Stamina -= consumedStamina;
            }

            if (consumedStamina > 10.0f)
            {
                playerUI.StaminaBarImage.MidgroundColor = startColor;
                StartCoroutine(CheckMidAndForegroundSliderValue());
            }
        }

        public void ReduceHP(int damage)
        {
            // Hit Effect
            // 1. Model Material
            hitComponent.Hit();

            // 2. Screen Damage
            var payload = UIPayload.Notify();
            payload.actionType = ActionType.ShowBlurEffect;
            ControllerTicketMachine.SendMessage(ChannelType.UI, payload);

            // 3. Camera
            var cameraPayload = new CameraPayload();
            cameraPayload.type = CameraShakingEffectType.Start;
            cameraPayload.shakeIntensity = damage;
            cameraPayload.shakeTime = hitComponent.HitDuration();
            ControllerTicketMachine.SendMessage(ChannelType.Camera, cameraPayload);

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
            var color = Color.white;
            color.a = 0f;
            playerUI.StaminaBarImage.MidgroundColor = color;
        }

        private StatusEffectInfo GenerateStatusEffectInfo(CombatPayload payload)
        {
            var info = new StatusEffectInfo
            {
                effectDuration = payload.statusEffectduration,
                effectForce = payload.force
            };

            return info;
        }

        public void SetPlayerInvulnerable(float time)
        {
            if (invulnerableCoroutine != null)
            {
                StopCoroutine(invulnerableCoroutine);
            }

            invulnerableCoroutine = StartCoroutine(SetPlayerInvulnerableCoroutine(time));
        }

        private IEnumerator SetPlayerInvulnerableCoroutine(float time)
        {
            gameObject.tag = "Untagged";
            yield return new WaitForSeconds(time);
            gameObject.tag = "Player";
        }

        public void ApplyConsumableItemEffect(PlayerInventory.ConsumableItemData data)
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "ellie_sound8", transform.position);
            var HPRecoveryAmount = data.HPRecoveryAmount;
            if (HP + HPRecoveryAmount >= maxHP)
            {
                HP = maxHP;
            }
            else
            {
                HP += HPRecoveryAmount;
            }
        }
    }
}