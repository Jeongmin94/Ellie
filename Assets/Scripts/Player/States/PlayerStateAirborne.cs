using UnityEngine;

namespace Assets.Scripts.Player.States
{
    internal class PlayerStateAirborne : PlayerBaseState
    {
        Rigidbody rb;
        private float moveSpeed = 1;
        private const float AIRBORNE_TIME_THRESHOLD = 3.0f;
        private float time;
        public PlayerStateAirborne(PlayerController controller) : base(controller)
        {
            rb = Controller.Rb;
        }

        public override void OnEnterState()
        {
            Controller.canTurn = false;

            Controller.isFalling = true;
            Controller.Anim.SetBool("IsFalling", true);
            Controller.SetColliderHeight(1f);
            time = 0f;
        }

        public override void OnExitState()
        {
            Controller.SetColliderHeight(1.5f);

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
            if (Input.GetMouseButton(0) && Controller.hasStone)
            {
                Controller.ChangeState(PlayerStateName.Zoom);
            }
            time += Time.deltaTime;
            if(time>=AIRBORNE_TIME_THRESHOLD)
            {
                Controller.ChangeState(PlayerStateName.Land);
            }
        }
    }
}
