using System;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    internal class PlayerStateCharging : PlayerBaseState
    {
        public PlayerStateCharging(PlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState()
        {
            Controller.canTurn = false;

        }

        public override void OnExitState()
        {
            Controller.Anim.SetLayerWeight(1, 0);
            Controller.debugSphere.SetActive(false);
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnUpdateState()
        {
            Controller.Aim();
            Controller.LookAimTarget();

            if (Input.GetMouseButtonUp(0))
            {
                Controller.SetTimeScale(1f);
                Controller.TurnOffAimCam();
                Controller.Anim.SetLayerWeight(1, 0);

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
