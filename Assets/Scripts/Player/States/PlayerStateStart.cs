using UnityEngine;

namespace Player.States
{
    public class PlayerStateStart : PlayerBaseState
    {
        private const float duration = 3.0f;
        private float acctime;

        public PlayerStateStart(PlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState()
        {
            Controller.Anim.SetBool("IsLoadEnd", true);
        }

        public override void OnExitState()
        {
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnUpdateState()
        {
            acctime += Time.deltaTime;
            if (acctime >= duration)
            {
                Controller.ChangeState(PlayerStateName.Idle);
            }
        }
    }
}