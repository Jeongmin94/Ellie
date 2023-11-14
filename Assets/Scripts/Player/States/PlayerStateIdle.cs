using System;
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
            Controller.canTurn = true;
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
            if (Input.GetKeyDown(KeyCode.Space) && Controller.isGrounded && Controller.canJump
                && Controller.PlayerStatus.Stamina >= Controller.PlayerStatus.JumpStaminaConsumption)
            {
                //점프하면 점프 스테이트로 전이
                Controller.ChangeState(PlayerStateName.Jump);
            }
            if (Input.GetMouseButtonDown(0) && Controller.canAttack)
            {
                if (Controller.hasStone)
                    Controller.ChangeState(PlayerStateName.Zoom);
                else
                    Controller.ChangeState(PlayerStateName.MeleeAttack);
            }
            if (Input.GetKeyDown(KeyCode.G) && Controller.CurOre != null)
            {
                Controller.ChangeState(PlayerStateName.Mining);
            }
        }

        public override void OnFixedUpdateState()
        {
            //Idle이니까 아무것도 안함
        }
    }
}
