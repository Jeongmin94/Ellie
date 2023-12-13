using Assets.Scripts.StatusEffects;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player.StatusEffects.StatusEffectConcreteStrategies
{
    public class PlayerStatusEffectKnockedAirborne : MonoBehaviour, IPlayerStatusEffect
    {
        PlayerController playerController;
        public void InitStatusEffect()
        {
            
        }

        public void ApplyStatusEffect(PlayerStatusEffectController controller, StatusEffectInfo info)
        {
            playerController = controller.gameObject.GetComponent<PlayerController>();
            StartCoroutine(KnockedAirborne(controller, info));
        }

        private IEnumerator KnockedAirborne(PlayerStatusEffectController controller, StatusEffectInfo effectInfo)
        {
            StateInfo info = new();
            info.stateDuration = effectInfo.effectDuration;
            info.magnitude = effectInfo.effectForce;
            controller.AddStatusEffect(this);
            playerController.ChangeState(PlayerStateName.Down, info);
            yield return new WaitForSeconds(effectInfo.effectDuration);
            controller.RemoveStatusEffect(this);
        }
    }
}