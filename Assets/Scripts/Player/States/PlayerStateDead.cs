using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Player.States
{
    public class PlayerStateDead : PlayerBaseState
    {
        public PlayerStateDead(PlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState()
        {
            Controller.Anim.SetTrigger("Dead");
        }

        public override void OnExitState()
        {
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnUpdateState()
        {
        }
    }
}
