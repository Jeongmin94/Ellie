using Assets.Scripts.Managers;
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
        private float footPrintInterval = 0.6f;

        private string[] footprint = new string[2];
        private int footprintIdx = 0;
        private float accTime;
        private readonly Rigidbody rb;
        public PlayerStateWalk(PlayerController controller) : base(controller)
        {
            rb = Controller.Rb;

            footprint[0] = "ellie_move1";
            footprint[1] = "ellie_move2";
        }

        public override void OnEnterState()
        {
            Controller.canTurn = true;

            moveSpeed = startMoveSpeed = rb.velocity.magnitude;
            expectedMoveSpeed = Controller.WalkSpeed;
            interpolateTime = 0f;
            //Controller.Anim.lay
        }

        public override void OnExitState()
        {
            SoundManager.Instance.StopAmbient("ellie_move1");
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

                Controller.ChangeState(PlayerStateName.Jump);
            }
            if (Controller.MoveInput.magnitude == 0)
            {
                Controller.ChangeState(PlayerStateName.Idle);
            }
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Controller.ChangeState(PlayerStateName.Dodge);
            }
            if (Input.GetMouseButtonDown(0) && Controller.canAttack)
            {
                if (Controller.hasStone)
                    Controller.ChangeState(PlayerStateName.Zoom);
                else
                    Controller.ChangeState(PlayerStateName.MeleeAttack);
            }
            PlayFootPrintSound();
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

        private void PlayFootPrintSound()
        {
            accTime += Time.deltaTime;
            
            if(accTime>=footPrintInterval)
            {
                SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, footprint[footprintIdx], Controller.transform.position);
                if (footprintIdx == 1)
                    footprintIdx = 0;
                else
                    footprintIdx = 1;
                accTime = 0;
            }
        }
    }
}
