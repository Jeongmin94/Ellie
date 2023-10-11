using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    internal class PlayerStateJump : PlayerBaseState
    {
        Rigidbody rb;
        public PlayerStateJump(PlayerController controller) : base(controller)
        {
            rb = Controller.rb;
        }

        public override void OnEnterState()
        {
            Controller.isJumping = true;
            Controller.JumpPlayer();
        }

        public override void OnExitState()
        {
            Controller.isJumping = false;
        }

        public override void OnFixedUpdateState()
        {
            if (Controller.isFalling)
                Controller.Fall();
        }

        public override void OnUpdateState()
        {
        }
        
        
    }
}
