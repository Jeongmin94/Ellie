using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.StatusEffects
{
    public struct StatusEffectInfo
    {
        // !TODO : 상태이상에 정의된 수치들을 작성합니다
        //상태이상의 지속 시간
        public float effectDuration;
        //힘이 가해지는 상태이상의 힘의 크기
        public float effectForce;
    }
    public class PlayerStatusEffectController : MonoBehaviour
    {
        private List<IPlayerStatusEffect> effects;

        // !TODO : effects 리스트를 참조해서 UI 띄워주기
        private void Awake()
        {
            effects = new List<IPlayerStatusEffect>();
        }
        public void ApplyStatusEffect(IPlayerStatusEffect effect, StatusEffectInfo info)
        {
            effect?.ApplyStatusEffect(this, info);
        }
        public void AddStatusEffect(IPlayerStatusEffect effect)
        {
            effects?.Add(effect);
        }
        public void RemoveStatusEffect(IPlayerStatusEffect effect)
        {
            effects?.Remove(effect);
        }
    }
}