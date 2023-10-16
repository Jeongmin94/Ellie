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
            Debug.Log("Land");
            Controller.canTurn = false;
            Controller.Anim.SetBool("IsFalling", false);
            Controller.isJumping = false;
            Controller.isFalling = false;

            time = 0f;
            Controller.SetTimeScale(1f);

            if (Controller.cinematicAimCam.activeSelf)
                Controller.TurnOffAimCam();
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
            if(time > interval)
            {
                Controller.Anim.SetTrigger("Idle");
                Controller.ChangeState(PlayerStateName.Idle);
            }
        }
        
    }
}
