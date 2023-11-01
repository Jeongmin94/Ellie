using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.StatusEffects
{
    public class PlayerStatusEffectController : MonoBehaviour
    {
        private List<IPlayerStatusEffect> effects;

        // !TODO : effects 리스트를 참조해서 UI 띄워주기
        private void Awake()
        {
            effects = new List<IPlayerStatusEffect>();
        }
        public void ApplyStatusEffect(IPlayerStatusEffect effect)
        {
            effect.ApplyStatusEffect(this);
        }
        public void AddStatusEffect(IPlayerStatusEffect effect)
        {
            effects.Add(effect);
        }
        public void RemoveStatusEffect(IPlayerStatusEffect effect)
        {
            effects.Remove(effect);
        }
    }
}