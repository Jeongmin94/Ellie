using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Monsters.EffectStatus
{
    public class MonsterEffectStatusController : MonoBehaviour
    {
        private List<IMonsterStatusEffect> effects;

        private void Awake()
        {
            effects = new();
        }
        public void ApplyStatusEffect(IMonsterStatusEffect effect)
        {
            effect.ApplyStatusEffect(this);
        }
        public void AddStatusEffect(IMonsterStatusEffect effect)
        {
            effects.Add(effect);
        }
        public void RemoveStatusEffect(IMonsterStatusEffect effect)
        {
            effects.Remove(effect);
        }
    }
}