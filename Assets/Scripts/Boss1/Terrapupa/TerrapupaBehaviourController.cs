using System.Collections;
using System.Collections.Generic;
using Boss1.DataScript.Terrapupa;
using Channels.Boss;
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
        [SerializeField] [Required] private TerrapupaWeakPoint weakPoint;
        [SerializeField] [Required] private TerrapupaHealthBar healthBar;
        [SerializeField] [Required] private Transform stone;

        [ShowInInspector] [ReadOnly] private readonly Dictionary<TerrapupaAttackType, Coroutine> attackCooldown = new();

        private TerrapupaQuestController terrapupaQuest;
        private TerrapupaGolemCore golemCore;
        private MaterialHitComponent hitComponent;
        private ParticleController intakeEffect;
        private TicketMachine ticketMachine;
        
        public Dictionary<TerrapupaAttackType, Coroutine> AttackCooldown
        {
            get { return attackCooldown; }
        }
        
        public Transform Stone
        {
            get { return stone; }
            set { stone = value; }
        }
        
        public TerrapupaRootData TerrapupaData
        {
            get; private set;
        }

        public bool IsDead
        {
            get; private set;
        }

        protected override void Awake()
        {
            base.Awake();

            TerrapupaData = rootTreeData as TerrapupaRootData;
            healthBar = gameObject.GetOrAddComponent<TerrapupaHealthBar>();
            hitComponent = gameObject.GetComponent<MaterialHitComponent>();
            golemCore = gameObject.GetComponent<TerrapupaGolemCore>();
            terrapupaQuest = gameObject.GetComponent<TerrapupaQuestController>();

            SubscribeEvent();
        }

        private void Start()
        {
            InitStatus();
        }

        public void InitTicketMachine(TicketMachine ticketMachine)
        {
            this.ticketMachine = ticketMachine;
        }

        private void InitStatus()
        {
            healthBar.InitData(TerrapupaData);
            terrapupaQuest.InitData(TerrapupaData);
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
            if (TerrapupaData.isStuned.value)
            {
                if (payload is CombatPayload combatPayload)
                {
                    PoolManager.Instance.Push(combatPayload.Attacker.GetComponent<Poolable>());

                    var damage = combatPayload.Damage;
                    ApplyDamage(damage);
                }
            }
            else
            {
                terrapupaQuest.CheckWeakPoint();
            }
        }

        public void ApplyDamage(int damageValue)
        {
            if (!gameObject.activeSelf || IsDead)
            {
                return;
            }

            StartCoroutine(ShakeCoroutine(TerrapupaData.cameraShakeIntensity, TerrapupaData.cameraShakeDuration));
            healthBar.ShowBillboard();
            hitComponent.Hit();

            healthBar.RenewHealthBar(TerrapupaData.currentHP.value - damageValue);
            TerrapupaData.currentHP.Value -= damageValue;

            if (TerrapupaData.currentHP.value <= 0)
            {
                Dead();
            }
        }

        public void ApplyHeal(int healValue)
        {
            if (!gameObject.activeSelf || IsDead)
            {
                return;
            }

            healthBar.RenewHealthBar(TerrapupaData.currentHP.value + healValue);
            TerrapupaData.currentHP.Value += healValue;

            if (TerrapupaData.currentHP.value > TerrapupaData.hp)
            {
                healthBar.HideBillboard();
                TerrapupaData.currentHP.Value = TerrapupaData.hp;
                healthBar.RenewHealthBar(TerrapupaData.currentHP.value);
            }
        }

        public void StartBattle(Transform player)
        {
            TerrapupaData.isStart.Value = true;
            golemCore.PlayCoreEffect();
            TerrapupaData.player.Value = player;
            healthBar.HideBillboard();
        }

        public void Dead()
        {
            IsDead = true;
            StopIntakeEffect();
            TerrapupaData.currentHP.Value = 0;
            healthBar.RenewHealthBar(0);
            golemCore.DarkenCore();
            healthBar.BillboardObject.gameObject.SetActive(false);
        }

        public void Stun()
        {
            StopIntakeEffect();
            golemCore.StopCoreEffect();
            golemCore.StopBlinkCore();
            TerrapupaData.isStuned.Value = true;
            TerrapupaData.isTempted.Value = false;
            TerrapupaData.isIntake.Value = false;
        }

        public void AttractMagicStone(Transform magicStone)
        {
            TerrapupaData.isTempted.Value = true;
            TerrapupaData.isIntake.Value = false;
            TerrapupaData.magicStoneTransform.Value = magicStone;
        }

        public void UnattractMagicStone()
        {
            TerrapupaData.isTempted.Value = false;
            TerrapupaData.isIntake.Value = false;
            TerrapupaData.magicStoneTransform.Value = null;
        }

        public void StartIntakeMagicStone()
        {
            golemCore.StartBlinkCore();

            var effect = Data<TerrapupaIntakeData>("TerrapupaIntake");
            var payload = new ParticlePayload { Origin = transform, LoopCount = 5 };
            intakeEffect = ParticleManager.Instance.
                GetParticle(effect.effect1, payload).GetComponent<ParticleController>();
        }

        public void EndIntakeMagicStone(int healValue)
        {
            StopIntakeEffect();
            ApplyHeal(healValue);
            golemCore.StopBlinkCore();
            TerrapupaData.isTempted.Value = false;
            TerrapupaData.isIntake.Value = false;
            TerrapupaData.magicStoneTransform.Value = null;
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
                    if (TerrapupaData.stoneUsable)
                    {
                        TerrapupaData.canThrowStone.Value = isCooldownDone;
                    }

                    break;
                case TerrapupaAttackType.EarthQuake:
                    if (TerrapupaData.earthQuakeUsable)
                    {
                        TerrapupaData.canEarthQuake.Value = isCooldownDone;
                    }

                    break;
                case TerrapupaAttackType.Roll:
                    if (TerrapupaData.rollUsable)
                    {
                        TerrapupaData.canRoll.Value = isCooldownDone;
                    }

                    break;
                case TerrapupaAttackType.LowAttack:
                    if (TerrapupaData.lowAttackUsable)
                    {
                        TerrapupaData.canLowAttack.Value = isCooldownDone;
                    }

                    break;
            }
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

        private void StopIntakeEffect()
        {
            if (!intakeEffect)
            {
                return;
            }

            intakeEffect.Stop();
            intakeEffect = null;
        }
    }
}
