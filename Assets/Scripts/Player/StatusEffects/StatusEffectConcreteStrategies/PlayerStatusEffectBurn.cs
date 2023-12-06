using Assets.Scripts.Player;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.StatusEffects.StatusEffectConcreteStrategies
{
    public class PlayerStatusEffectBurn : MonoBehaviour, IPlayerStatusEffect
    {
        private float duration;
        PlayerStatus status;
        public void ApplyStatusEffect(PlayerStatusEffectController controller, StatusEffectInfo info)
        {
            status = controller.GetComponent<PlayerStatus>();
            StartCoroutine(Burn(controller));
            duration = info.effectDuration;
        }
        private IEnumerator Burn(PlayerStatusEffectController controller)
        {
            // !TODO : 플레이어 화상 이펙트 적용
            controller.AddStatusEffect(this);
            float startTime = Time.time;
            //화상 로직
            while (Time.time - startTime < duration)
            {
                yield return new WaitForSeconds(1.0f);
                Debug.Log("Burn!");
                status.ReduceHP(1);
            }
            controller.RemoveStatusEffect(this);
        }
    }
}
