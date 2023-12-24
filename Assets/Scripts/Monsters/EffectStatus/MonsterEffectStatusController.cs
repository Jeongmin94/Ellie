using System.Collections.Generic;
using UnityEngine;

namespace Monsters.EffectStatus
{
    public struct MonsterStatusEffectInfo
    {
        public float EffectDuration { get; set; }
        public float EffectForce { get; set; }
    }

    public class MonsterEffectStatusController : MonoBehaviour
    {
        private List<IMonsterStatusEffect> effects;

        private void Awake()
        {
            effects = new List<IMonsterStatusEffect>();
        }

        public void ApplyStatusEffect(IMonsterStatusEffect effect, MonsterStatusEffectInfo info)
        {
            effect.ApplyStatusEffect(this, info);
        }

        public void AddStatusEffect(IMonsterStatusEffect effect)
        {
            effects?.Add(effect);
        }

        public void RemoveStatusEffect(IMonsterStatusEffect effect)
        {
            effects?.Remove(effect);
        }
    }
}