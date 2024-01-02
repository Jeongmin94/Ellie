using System.Collections;
using System.Collections.Generic;
using Boss1.DataScript.Terrapupa;
using Channels.Boss;
using Channels.Camera;
using Channels.Combat;
using Channels.Components;
using Managers.Particle;
using Managers.Pool;
using Player.HitComponent;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Boss1.Terrapupa
{
    public class TerrapupaBehaviourController : BehaviourTreeController
    {
        [HideInInspector] public TerrapupaRootData terrapupaData;
        
        [SerializeField] [Required] private TerrapupaWeakPoint weakPoint;
        [SerializeField] [Required] private TerrapupaHealthBar healthBar;
        [SerializeField] [Required] private Transform stone;
        
        [ShowInInspector] [ReadOnly] 
        private readonly Dictionary<TerrapupaAttackType, Coroutine> attackCooldown = new();
        
        private TerrapupaQuestController terrapupaQuest;
        private TerrapupaCoreController coreController;
        private MaterialHitComponent hitComponent;
        private ParticleController intakeEffect;
        
        private TicketMachine ticketMachine;

        public Transform Stone
        {
            get => stone;
            set => stone = value;
        }
        
        public Dictionary<TerrapupaAttackType, Coroutine> AttackCooldown => attackCooldown;

        public bool IsDead { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            terrapupaData = rootTreeData as TerrapupaRootData;
            healthBar = gameObject.GetOrAddComponent<TerrapupaHealthBar>();
            hitComponent = gameObject.GetComponent<MaterialHitComponent>();
            coreController = gameObject.GetComponent<TerrapupaCoreController>();
            terrapupaQuest = gameObject.GetComponent<TerrapupaQuestController>();

            SubscribeEvent();
        }

        private void Start()
        {
            InitStatus();
            InitQuest();
        }
        
        public void InitTicketMachine(TicketMachine ticketMachine)
        {
            this.ticketMachine = ticketMachine;
        }

        private void InitStatus()
        {
            healthBar.InitData(terrapupaData);
        }

        private void InitQuest()
        {
            terrapupaQuest.InitData(terrapupaData);
            terrapupaQuest.InitTicketMachine(ticketMachine);
        }

        private void SubscribeEvent()
        {
            weakPoint.SubscribeCollisionAction(OnCollidedGolemCore);
        }

        private void OnCollidedGolemCore(IBaseEventPayload payload)
        {
            // 플레이어 총알 -> Combat Channel
            // -> TerrapupaWeakPoint :: ReceiveDamage() -> TerrapupaController
            if (terrapupaData.isStuned.value)
            {
                var combatPayload = payload as CombatPayload;
                PoolManager.Instance.Push(combatPayload.Attacker.GetComponent<Poolable>());
                
                var damage = combatPayload.Damage;
                GetDamaged(damage);
            }
            else
            {
                terrapupaQuest.CheckWeakPoint();
            }
        }

        public void GetDamaged(int damageValue)
        {
            if (!IsDead)
            {
                healthBar.ShowBillboard();
                CameraChannel.ShakeCamera(ticketMachine, damageValue);
                StartCoroutine(ShakeCoroutine());
                hitComponent.Hit();

                healthBar.RenewHealthBar(terrapupaData.currentHP.value - damageValue);
                terrapupaData.currentHP.Value -= damageValue;

                if (terrapupaData.currentHP.value <= 0)
                {
                    Dead();
                }
            }
        }

        public void GetHealed(int healValue)
        {
            if (!IsDead)
            {
                healthBar.RenewHealthBar(terrapupaData.currentHP.value + healValue);
                terrapupaData.currentHP.Value += healValue;

                if (terrapupaData.currentHP.value > terrapupaData.hp)
                {
                    healthBar.HideBillboard();
                    terrapupaData.currentHP.Value = terrapupaData.hp;
                    healthBar.RenewHealthBar(terrapupaData.currentHP.value);
                }
            }
        }

        public void StartBattle(Transform player)
        {
            terrapupaData.isStart.Value = true;
            coreController.PlayCoreEffect();
            terrapupaData.player.Value = player;
            healthBar.HideBillboard();
        }

        public void Dead()
        {
            IsDead = true;
            StopIntakeEffect();
            terrapupaData.currentHP.Value = 0;
            healthBar.RenewHealthBar(0);
            coreController.DarkenCore();
            healthBar.BillboardObject.gameObject.SetActive(false);
        }

        public void Stun()
        {
            StopIntakeEffect();
            coreController.StopCoreEffect();
            coreController.StopBlinkCore();
            terrapupaData.isStuned.Value = true;
            terrapupaData.isTempted.Value = false;
            terrapupaData.isIntake.Value = false;
        }

        public void AttractMagicStone(Transform magicStone)
        {
            terrapupaData.isTempted.Value = true;
            terrapupaData.isIntake.Value = false;
            terrapupaData.magicStoneTransform.Value = magicStone;
        }

        public void UnattractMagicStone()
        {
            terrapupaData.isTempted.Value = false;
            terrapupaData.isIntake.Value = false;
            terrapupaData.magicStoneTransform.Value = null;
        }

        public void StartIntakeMagicStone()
        {
            coreController.StartBlinkCore();

            var effect = Data<TerrapupaIntakeData>("TerrapupaIntake");
            var payload = new ParticlePayload { Origin = transform, LoopCount = 5 };
            intakeEffect = ParticleManager.Instance.GetParticle(effect.effect1, payload)
                .GetComponent<ParticleController>();
        }

        public void EndIntakeMagicStone(int healValue)
        {
            StopIntakeEffect();
            GetHealed(healValue);
            coreController.StopBlinkCore();
            terrapupaData.isTempted.Value = false;
            terrapupaData.isIntake.Value = false;
            terrapupaData.magicStoneTransform.Value = null;
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
                    if (terrapupaData.stoneUsable)
                    {
                        terrapupaData.canThrowStone.Value = isCooldownDone;
                    }

                    break;
                case TerrapupaAttackType.EarthQuake:
                    if (terrapupaData.earthQuakeUsable)
                    {
                        terrapupaData.canEarthQuake.Value = isCooldownDone;
                    }

                    break;
                case TerrapupaAttackType.Roll:
                    if (terrapupaData.rollUsable)
                    {
                        terrapupaData.canRoll.Value = isCooldownDone;
                    }

                    break;
                case TerrapupaAttackType.LowAttack:
                    if (terrapupaData.lowAttackUsable)
                    {
                        terrapupaData.canLowAttack.Value = isCooldownDone;
                    }

                    break;
            }
        }

        private IEnumerator ShakeCoroutine()
        {
            var elapsed = 0.0f;

            var originalPosition = transform.position;

            while (elapsed < terrapupaData.cameraShakeDuration)
            {
                transform.position = originalPosition + Random.insideUnitSphere * terrapupaData.cameraShakeIntensity;
                elapsed += Time.deltaTime;
                yield return null; // 다음 프레임까지 기다림
            }

            transform.position = originalPosition; // 원래 위치로 돌아감
        }

        private void StopIntakeEffect()
        {
            if (intakeEffect)
            {
                intakeEffect.Stop();
                intakeEffect = null;
            }
        }
    }
}
