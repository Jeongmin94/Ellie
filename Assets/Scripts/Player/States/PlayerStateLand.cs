using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    internal class PlayerStateLand : PlayerBaseState
    {
        private float interval;
        private float time;
        public PlayerStateLand(PlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState()
        {
            interval = Controller.LandStateDuration;
            Controller.canTurn = false;
            Controller.Anim.SetBool("IsFalling", false);
            Controller.isJumping = false;
            Controller.isFalling = false;
            Controller.SetAnimLayerToDefault(PlayerController.AnimLayer.Aiming);
            Controller.ActivateShootPos(false);

            time = 0f;
            Controller.SetTimeScale(1f);

            if (Controller.cinematicAimCam.gameObject.activeSelf)
                Controller.TurnOffAimCam();

            Controller.PlayerStatus.isRecoveringStamina = true;

            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "ellie_move5", Controller.transform.position);

        }

        public override void OnExitState()
        {
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnUpdateState()
        {
            time += Time.deltaTime;
            if (time > interval)
            {
                if (Controller.PlayerStatus.Stamina < 10f)
                    Controller.ChangeState(PlayerStateName.Exhaust);
                else
                {
                    Controller.Anim.SetTrigger("Idle");
                    Controller.ChangeState(PlayerStateName.Idle);
                }
            }
        }

    }
}
