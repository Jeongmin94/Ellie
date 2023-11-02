using Assets.Scripts.StatusEffects;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player.StatusEffects.StatusEffectConcreteStrategies
{
    public class PlayerStatusEffectStrongRigidity : MonoBehaviour, IPlayerStatusEffect
    {

        private const float RIGIDITY_DURATION = 2.0f;
        private PlayerController playerController;
        public void ApplyStatusEffect(PlayerStatusEffectController controller)
        {
            playerController = controller.gameObject.GetComponent<PlayerController>();
            StartCoroutine(ImposeRigidity(controller));
        }

        private IEnumerator ImposeRigidity(PlayerStatusEffectController controller)
        {
            PlayerStateName stateName = playerController.GetCurState();
            if (!(stateName == PlayerStateName.Idle || stateName == PlayerStateName.Walk || stateName == PlayerStateName.Sprint
                || stateName == PlayerStateName.Jump || stateName == PlayerStateName.Airborne || stateName == PlayerStateName.Zoom || stateName == PlayerStateName.Charging
                || stateName == PlayerStateName.Shoot || stateName == PlayerStateName.Rigidity))
                yield break;

            controller.AddStatusEffect(this);
            playerController.ChangeState(PlayerStateName.Rigidity);
            yield return new WaitForSeconds(RIGIDITY_DURATION);
            playerController.ChangeState(PlayerStateName.Idle);
            controller.RemoveStatusEffect(this);
        }

    }
}