using Assets.Scripts.Managers;
using Assets.Scripts.StatusEffects;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    internal class PlayerStateSprint : PlayerBaseState
    {
        private float moveSpeed;
        private float expectedMoveSpeed;
        private float startMoveSpeed;
        private float interpolateTime;
        private float duration = 0.5f;
        private readonly Rigidbody rb;

        private float footPrintInterval = 0.25f;

        private string[] footprint = new string[2];
        private int footprintIdx = 0;
        private float accTime;

        float temp;
        public PlayerStateSprint(PlayerController controller) : base(controller)
        {
            rb = controller.Rb;
            footprint[0] = "ellie_move3";
            footprint[1] = "ellie_move4";
        }

        public override void OnEnterState()
        {
            Controller.canTurn = true;
            Controller.isSprinting = true;
            moveSpeed = startMoveSpeed = rb.velocity.magnitude;
            expectedMoveSpeed = Controller.SprintSpeed;
            interpolateTime = 0f;
            Controller.PlayerStatus.isRecoveringStamina = false;
        }

        public override void OnExitState()
        {
            Controller.isSprinting = false;
            Controller.PlayerStatus.isRecoveringStamina = true;

        }
        public override void OnUpdateState()
        {
            InterpolateMoveSpeed();
            ControlSpeed();
            if(Controller.MoveInput.magnitude == 0f)
            {
                Controller.ChangeState(PlayerStateName.Idle);
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                Controller.ChangeState(PlayerStateName.Walk);
            }
            if (Input.GetKeyDown(KeyCode.Space) && Controller.isGrounded && Controller.canJump)
            {
                Controller.ChangeState(PlayerStateName.Jump);
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
            ConsumeStamina();
            PlayFootPrintSound();
        }
        private void ConsumeStamina()
        {
            //temp += Time.deltaTime;
            //if (temp >= 0.1f)
            //{
            //    Controller.PlayerStatus.ConsumeStamina(Controller.PlayerStatus.SprintStaminaConsumptionPerSec / 10);
            //    temp = 0;
            //}
            //Controller.PlayerStatus.Stamina -= Time.deltaTime * Controller.PlayerStatus.SprintStaminaConsumptionPerSec;
            Controller.PlayerStatus.ConsumeStamina(Time.deltaTime * Controller.PlayerStatus.SprintStaminaConsumptionPerSec);
            if (Controller.PlayerStatus.Stamina <= 10.0f)
            {
                Controller.ChangeState(PlayerStateName.Exhaust);
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

        private void PlayFootPrintSound()
        {
            accTime += Time.deltaTime;

            if (accTime >= footPrintInterval)
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
