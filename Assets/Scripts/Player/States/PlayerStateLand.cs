using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    internal class PlayerStateLand : PlayerBaseState
    {
        public PlayerStateLand(PlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState()
        {
            Debug.Log("Land");
            Controller.Anim.SetBool("IsFalling", false);
            Controller.isJumping = false;
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
