using Assets.Scripts.Monsters.EffectStatus.StatusEffectConcreteStrategies;
using Assets.Scripts.Player.StatusEffects.StatusEffectConcreteStrategies;
using Assets.Scripts.StatusEffects;
using Assets.Scripts.Utils;
using Channels.Combat;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Assets.Scripts.Monsters.EffectStatus
{
    public class MonsterStatus : SerializedMonoBehaviour
    {
        [ShowInInspector]
        [ReadOnly]
        private Dictionary<StatusEffectName, IMonsterStatusEffect> monsterStatusEffects = new Dictionary<StatusEffectName, IMonsterStatusEffect>();
        private MonsterEffectStatusController monsterStatusEffectController;

        private void Awake()
        {
            InitStatusEffectController();
        }

        private void Start()
        {
            AddStatusEffect();
        }

        private void InitStatusEffectController()
        {
            monsterStatusEffectController = gameObject.GetOrAddComponent<MonsterEffectStatusController>();
        }
        private void AddStatusEffect()
        {
            // 상태이상 여기에 추가
            monsterStatusEffects.Add(StatusEffectName.Incarceration, gameObject.AddComponent<MonsterStatusEffectFreezing>());
        }

        public void ApplyStatusEffect(CombatPayload payload)
        {
            monsterStatusEffects.TryGetValue(payload.StatusEffectName, out IMonsterStatusEffect effect);
            if (effect != null)
            {
                monsterStatusEffectController.ApplyStatusEffect(effect, GenerateStatusEffectInfo(payload));
            }
        }

        [Button("몬스터 빙결 상태이상 체크", ButtonSizes.Large)]
        public void Test()
        {
            ApplyStatusEffect(new CombatPayload
            {
                StatusEffectName = StatusEffectName.Incarceration,
                statusEffectduration = 10.0f,
            });
        }

        private MonsterStatusEffectInfo GenerateStatusEffectInfo(CombatPayload payload)
        {
            MonsterStatusEffectInfo info = new MonsterStatusEffectInfo();

            info.EffectDuration = payload.statusEffectduration;
            info.EffectForce = payload.force;
            return info;
        }
    }
}