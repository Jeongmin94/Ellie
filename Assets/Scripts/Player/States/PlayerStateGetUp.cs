using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class PlayerStateGetUp : PlayerBaseState
    {
        private float curTime;
        private const float GETUP_DURATION = 1.2f;
        public PlayerStateGetUp(PlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState()
        {
            Controller.Anim.SetTrigger("GetUp");
            curTime = 0;
        }

        public override void OnExitState()
        {
            Controller.Anim.SetTrigger("Idle");
            Controller.canTurn = true;
            Controller.isRigid = false;
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnUpdateState()
        {
            curTime += Time.deltaTime;
            if(curTime >= GETUP_DURATION)
            {
                Controller.ChangeState(PlayerStateName.Idle);
            }
        }
    }
}
