using UnityEngine;

namespace Assets.Scripts.Player.States
{
    internal class PlayerStateDodge : PlayerBaseState
    {
        private readonly Rigidbody rb;
        private Vector3 dodgeDir;
        private float dodgeTime;
        private float beforeDrag;
        public PlayerStateDodge(PlayerController controller) : base(controller)
        {
            rb = Controller.Rb;
        }

        public override void OnEnterState()
        {
            Controller.canTurn = false;
            beforeDrag = Controller.groundDrag;
            Controller.groundDrag = 0f;
            dodgeDir = Controller.MoveDirection.normalized;
            //Controller.cam.RotationSpeed = 100f;
            Controller.playerObj.forward = Controller.MoveDirection.normalized;
            dodgeTime = 0f;
        }

        public override void OnExitState()
        {
            Controller.groundDrag = beforeDrag;
            //Controller.cam.RotationSpeed = 10f;
            Controller.isDodging = false;
            Controller.Anim.SetBool("IsDodging", false);

        }

        public override void OnFixedUpdateState()
        {
            if (!Controller.isDodging)
            {
                Controller.isDodging = true;
                Controller.Anim.SetBool("IsDodging", true);
                rb.velocity = Vector3.zero;
                rb.AddForce(dodgeDir * Controller.DodgeSpeed, ForceMode.VelocityChange);
            }
            //rb.velocity = dodgeDir * Controller.DodgeSpeed;
            dodgeTime += Time.fixedDeltaTime;
            if (dodgeTime > Controller.DodgeInvulnerableTime)
            {
                Controller.ChangeState(PlayerStateName.Land);
            }
        }

        public override void OnUpdateState()
        {
        }
    }
}
