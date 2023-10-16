using UnityEngine;

namespace Assets.Scripts.Player.States
{
    internal class PlayerStateJump : PlayerBaseState
    {
        private float jumpInputTime;
        readonly Rigidbody rb;
        public PlayerStateJump(PlayerController controller) : base(controller)
        {
            rb = Controller.Rb;
        }

        public override void OnEnterState()
        {
            Debug.Log("Jump");
            Controller.canTurn = true;
            jumpInputTime = 0;
            Controller.isJumping = true;
            Controller.JumpPlayer();
            Controller.Anim.SetBool("IsJumping",true);
        }

        public override void OnExitState()
        {
            Controller.Anim.SetBool("IsJumping", false);
            //Controller.isJumping = false;
        }

        public override void OnFixedUpdateState()
        {
            if (Input.GetKey(KeyCode.Space) && jumpInputTime < Controller.MaximumJumpInputTime)
            {
                jumpInputTime += Time.fixedDeltaTime;
                rb.AddForce(rb.transform.up * Controller.AdditionalJumpForce, ForceMode.Force);
            }
            else
            {
                //점프 input이 끝나면 Airbourn 스테이트로 넘어가자
                
                Controller.ChangeState(PlayerStateName.Airbourn);
            }
            //if (Input.GetMouseButtonDown(0))
            //{
            //    Controller.ChangeState(PlayerStateName.Zoom);
            //}
            //if (Controller.isFalling)
            //    rb.AddForce(-rb.transform.up * Controller.AdditionalGravityForce, ForceMode.Force);
        }

        public override void OnUpdateState()
        {

        }
    }
}
