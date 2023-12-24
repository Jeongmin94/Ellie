using System.Collections;
using UnityEngine;

namespace Player.StatusEffects.StatusEffectConcreteStrategies
{
    public class PlayerStatusEffectWeakRigidity : MonoBehaviour, IPlayerStatusEffect
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
            StartCoroutine(ImposeRigidity(controller));
        }

        private IEnumerator ImposeRigidity(PlayerStatusEffectController controller)
        {
            var stateName = playerController.GetCurState();
            if (!(stateName == PlayerStateName.Idle || stateName == PlayerStateName.Walk ||
                  stateName == PlayerStateName.Sprint || stateName == PlayerStateName.Rigidity))
            {
                yield break;
            }

            controller.AddStatusEffect(this);
            StateInfo info = new()
            {
                stateDuration = duration
            };
            playerController.ChangeState(PlayerStateName.Rigidity, info);
            yield return new WaitForSeconds(duration);
            controller.RemoveStatusEffect(this);
        }
    }
}