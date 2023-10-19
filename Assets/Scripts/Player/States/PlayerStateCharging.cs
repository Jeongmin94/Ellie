using System;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    internal class PlayerStateCharging : PlayerBaseState
    {
        private float moveSpeed;
        public PlayerStateCharging(PlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState()
        {
            Controller.ActivateShootPos(true);
            moveSpeed = Controller.WalkSpeed;
            Controller.canTurn = false;

        }

        public override void OnExitState()
        {
            //Controller.debugSphere.SetActive(false);
        }

        public override void OnFixedUpdateState()
        {
            Controller.MovePlayer(moveSpeed);
        }

        public override void OnUpdateState()
        {
            Controller.Aim();
            Controller.LookAimTarget();

            if (Input.GetMouseButtonUp(0))
            {
                Controller.SetTimeScale(1f);
                Controller.TurnOffAimCam();
                Controller.SetAimingAnimLayerToDefault();
                Controller.ActivateShootPos(false);


                if (Controller.isGrounded)
                {
                    Controller.ChangeState(PlayerStateName.Idle);
                }
                else
                {
                    Controller.ChangeState(PlayerStateName.Airbourn);
                }
            }
        }
    }
}
