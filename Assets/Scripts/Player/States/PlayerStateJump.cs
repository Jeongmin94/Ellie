using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
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
            jumpInputTime = 0;
            Controller.isJumping = true;
            Controller.JumpPlayer();
            Controller.Anim.SetTrigger("Jump");
        }

        public override void OnExitState()
        {
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
            //if (Controller.isFalling)
            //    rb.AddForce(-rb.transform.up * Controller.AdditionalGravityForce, ForceMode.Force);
        }

        public override void OnUpdateState()
        {

        }
    }
}
