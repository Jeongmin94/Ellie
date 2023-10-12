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
            jumpInputTime = 0;
            Controller.isJumping = true;
            Controller.JumpPlayer();
        }

        public override void OnExitState()
        {
            Controller.isJumping = false;
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
                Controller.isFalling = true;
            }
            if (Controller.isFalling)
                rb.AddForce(-rb.transform.up * Controller.AdditionalGravityForce, ForceMode.Force);
        }

        public override void OnUpdateState()
        {

        }
    }
}
