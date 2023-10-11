using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Player.States
{
    internal class PlayerState_Move : PlayerBaseState
    {
        public PlayerState_Move(PlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState()
        {
        }

        public override void OnExitState()
        {
        }

        public override void OnFixedUpdateState()
        {
            //여기서 이동 로직 실행
            //Controller.moveinput
        }

        public override void OnUpdateState()
        {
        }
    }
}
