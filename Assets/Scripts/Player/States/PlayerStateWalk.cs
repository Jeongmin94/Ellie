using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    internal class PlayerStateWalk : PlayerBaseState
    {
        private float moveSpeed;
        private float expectedMoveSpeed;
        private float startMoveSpeed;
        private float interpolateTime;
        private float duration = 0.2f;
        private readonly Rigidbody rb;
        public PlayerStateWalk(PlayerController controller) : base(controller)
        {
            rb = Controller.Rb;
        }

        public override void OnEnterState()
        {
            Debug.Log("Walk");
            moveSpeed = startMoveSpeed = rb.velocity.magnitude;
            expectedMoveSpeed = Controller.WalkSpeed;
            interpolateTime = 0f;
        }

        public override void OnExitState()
        {

        }
        public override void OnUpdateState()
        {
            InterpolateMoveSpeed();
            ControlSpeed();
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Controller.ChangeState(PlayerStateName.Sprint);
            }
            if (Input.GetKeyDown(KeyCode.Space) && Controller.isGrounded && Controller.canJump)
            {
                Controller.Anim.SetTrigger("Jump");
                Controller.ChangeState(PlayerStateName.Jump);
            }
            if (Controller.MoveInput.magnitude == 0)
            {
                Controller.ChangeState(PlayerStateName.Idle);
            }
            if(Input.GetKeyDown(KeyCode.LeftControl))
            {
                Controller.ChangeState(PlayerStateName.Dodge);
            }
        }

        public override void OnFixedUpdateState()
        {
            Controller.MovePlayer(moveSpeed);
        }

        private void InterpolateMoveSpeed()
        {
            if (interpolateTime < duration)
            {
                interpolateTime += Time.deltaTime;
                moveSpeed = Mathf.Lerp(startMoveSpeed, expectedMoveSpeed, interpolateTime / duration);
            }
        }

        private void ControlSpeed()
        {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (flatVelocity.magnitude > moveSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }
    }
}
