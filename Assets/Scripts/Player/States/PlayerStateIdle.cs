using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    internal class PlayerStateIdle : PlayerBaseState
    {
        public PlayerStateIdle(PlayerController controller) : base(controller)
        {

        }

        public override void OnEnterState()
        {

        }

        public override void OnExitState()
        {

        }
        public override void OnUpdateState()
        {
            if (Controller.MoveInput.magnitude != 0f)
            {
                //입력이 들어온 것
                //스테이트 전이
                Controller.ChangeState(PlayerStateName.Walk);
            }
            if (Input.GetKeyDown(KeyCode.Space) && Controller.isGrounded && Controller.canJump)
            {
                //점프하면 점프 스테이트로 전이
                Controller.ChangeState(PlayerStateName.Jump);
            }
        }

        public override void OnFixedUpdateState()
        {
            //Idle이니까 아무것도 안함
        }
    }
}
