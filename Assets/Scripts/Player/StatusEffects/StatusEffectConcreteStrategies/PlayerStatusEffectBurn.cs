using Assets.Scripts.Player;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.StatusEffects.StatusEffectConcreteStrategies
{
    public class PlayerStatusEffectBurn : MonoBehaviour, IPlayerStatusEffect
    {
        private const float DURATION = 5.0f;
        PlayerStatus status;
        public void ApplyStatusEffect(PlayerStatusEffectController controller)
        {
            status = controller.GetComponent<PlayerStatus>();
            StartCoroutine(Burn(controller));
        }
        private IEnumerator Burn(PlayerStatusEffectController controller)
        {
            // !TODO : 플레이어 화상 이펙트 적용
            controller.effects.Add(this);
            float startTime = Time.time;
            while (Time.time - startTime < DURATION)
            {
                
                yield return null;
            }
            //화상 로직
        }
    }
}
