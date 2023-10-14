using UnityEngine;

namespace Assets.Scripts.Player.States
{
    internal class PlayerStateLand : PlayerBaseState
    {
        private float interval = 0.2f;
        private float time = 0f;
        public PlayerStateLand(PlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState()
        {
            Debug.Log("Land");
            Controller.canTurn = false;
            Controller.Anim.SetBool("IsFalling", false);
            Controller.isJumping = false;
            Controller.isFalling = false;

            time = 0f;
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
