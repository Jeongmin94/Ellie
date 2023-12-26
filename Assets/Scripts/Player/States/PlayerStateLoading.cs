using Assets.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Player.States
{
    public class PlayerStateLoading : PlayerBaseState
    {
        public PlayerStateLoading(PlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState()
        {
        }

        public override void OnExitState()
        {
            Controller.GetComponent<PlayerAim>().canAim = true;

        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnUpdateState()
        {
            
        }
    }
}
