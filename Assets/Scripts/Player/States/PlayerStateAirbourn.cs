using UnityEngine;

namespace Assets.Scripts.Player.States
{
    internal class PlayerStateAirbourn : PlayerBaseState
    {
        Rigidbody rb;
        private float moveSpeed = 1;
        public PlayerStateAirbourn(PlayerController controller) : base(controller)
        {
            rb = Controller.Rb;
        }

        public override void OnEnterState()
        {
            Controller.canTurn = false;

            Controller.isFalling = true;
            Controller.Anim.SetBool("IsFalling", true);
        }

        public override void OnExitState()
        {
            Controller.isFalling = false;
            Controller.Anim.SetBool("IsFalling", false);
        }   

        public override void OnFixedUpdateState()
        {
            //rb.AddForce(-rb.transform.up * Controller.AdditionalGravityForce, ForceMode.Force);
            Controller.MovePlayer(moveSpeed);
        }

        public override void OnUpdateState()
        {
            if (Input.GetMouseButton(0))
            {
                Controller.ChangeState(PlayerStateName.Zoom);
            }
        }
    }
}
