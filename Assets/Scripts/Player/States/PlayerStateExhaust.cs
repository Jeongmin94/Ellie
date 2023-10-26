using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Player.States
{
    internal class PlayerStateExhaust : PlayerBaseState
    {
        
        public PlayerStateExhaust(PlayerController controller) : base(controller)
        {

        }

        public override void OnEnterState()
        {
            Controller.canTurn = false;
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
