using Assets.Scripts.Managers;
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
            Controller.isRigid = false;
            Controller.canTurn = false;
            beforeDrag = Controller.groundDrag;
            Controller.groundDrag = 0f;
            dodgeDir = Controller.MoveDirection.normalized;
            //Controller.cam.RotationSpeed = 100f;
            Controller.PlayerObj.forward = Controller.MoveDirection.normalized;
            dodgeTime = 0f;
            Controller.PlayerStatus.isRecoveringStamina = false;
            Controller.PlayerStatus.ConsumeStamina(Controller.PlayerStatus.DodgeStaminaConsumption);
            Controller.PlayerStatus.SetPlayerInvulnerable(Controller.DodgeInvulnerableTime);
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "ellie_sound11", Controller.transform.position);
        }

        public override void OnExitState()
        {
            Controller.groundDrag = beforeDrag;
            //Controller.cam.RotationSpeed = 10f;
            Controller.isDodging = false;
            Controller.Anim.SetBool("IsDodging", false);
            //Controller.gameObject.tag = "Player";
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
