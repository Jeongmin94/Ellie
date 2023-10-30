using Assets.Scripts.StatusEffects;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player.StatusEffects.StatusEffectConcreteStrategies
{
    public class PlayerStatusEffectWeakRigidity : MonoBehaviour, IPlayerStatusEffect
    {
        private const float RIGIDITY_DURATION = 0.5f;
        private PlayerController playerController;
        public void ApplyStatusEffect(PlayerStatusEffectController controller)
        {
            playerController = controller.gameObject.GetComponent<PlayerController>();
            StartCoroutine(ImposeRigidity(controller));
        }

        private IEnumerator ImposeRigidity(PlayerStatusEffectController controller)
        {
            PlayerStateName stateName = playerController.GetCurState();
            if (!(stateName == PlayerStateName.Idle || stateName == PlayerStateName.Walk || stateName == PlayerStateName.Sprint))
                yield break;

            controller.AddStatusEffect(this);
            playerController.ChangeState(PlayerStateName.Rigidity);
            yield return new WaitForSeconds(RIGIDITY_DURATION);
            playerController.ChangeState(PlayerStateName.Idle);
            controller.RemoveStatusEffect(this);
        }
    }
}