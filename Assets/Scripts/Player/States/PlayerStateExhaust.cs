using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Player.States
{
    internal class PlayerStateExhaust : PlayerBaseState
    {
        private float moveSpeed;
        public PlayerStateExhaust(PlayerController controller) : base(controller)
        {

        }

        public override void OnEnterState()
        {
            Controller.canTurn = true;

            moveSpeed = Controller.WalkSpeed * 0.5f;
            Controller.PlayerStatus.isRecoveringStamina = true;
            Controller.Anim.SetBool("IsExhausted", true);
        }

        public override void OnExitState()
        {
            Controller.canTurn = true;
            Controller.Anim.SetBool("IsExhausted", false);

        }

        public override void OnFixedUpdateState()
        {
            Controller.MovePlayer(moveSpeed);
        }

        public override void OnUpdateState()
        {
            if(Controller.PlayerStatus.Stamina >= 50)
            {
                Controller.ChangeState(PlayerStateName.Idle);
            }
        }
    }
}
