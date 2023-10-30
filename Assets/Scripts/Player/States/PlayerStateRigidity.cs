using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Player.States
{
    public class PlayerStateRigidity : PlayerBaseState
    {
        public PlayerStateRigidity(PlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState()
        {
            Controller.Anim.SetBool("IsRigid", true);
        }

        public override void OnExitState()
        {
            Controller.Anim.SetBool("IsRigid", false);
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnUpdateState()
        {
        }
    }
}
