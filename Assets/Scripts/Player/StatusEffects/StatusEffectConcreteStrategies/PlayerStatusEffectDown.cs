using System.Collections;
using UnityEngine;

namespace Player.StatusEffects.StatusEffectConcreteStrategies
{
    public class PlayerStatusEffectDown : MonoBehaviour, IPlayerStatusEffect
    {
        private float duration;
        private PlayerController playerController;

        public void InitStatusEffect()
        {
        }

        public void ApplyStatusEffect(PlayerStatusEffectController controller, StatusEffectInfo info)
        {
            playerController = controller.gameObject.GetComponent<PlayerController>();
            duration = info.effectDuration;
            StartCoroutine(Down(controller));
        }

        private IEnumerator Down(PlayerStatusEffectController controller)
        {
            StateInfo info = new();
            info.stateDuration = duration;
            controller.AddStatusEffect(this);
            playerController.ChangeState(PlayerStateName.Down, info);
            yield return new WaitForSeconds(duration);
            controller.RemoveStatusEffect(this);
        }
    }
}