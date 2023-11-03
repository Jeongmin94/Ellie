﻿using Cinemachine;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    internal class PlayerStateZoom : PlayerBaseState
    {
        private float zoomingTime;
        private float curTime;
        public PlayerStateZoom(PlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState()
        {
            Controller.canTurn = false;
            zoomingTime = Controller.mainCam.GetComponent<CinemachineBrain>().m_DefaultBlend.BlendTime;
            //zoomingTime = 0.4f;
            Controller.TurnOnAimCam();
            curTime = 0;
            if (!Controller.isGrounded)
                Controller.isFalling = true;
            Controller.SetTimeScale(Controller.zoomMultiplier);
            Controller.AimingAnimLayerWeight = 0f;

        }

        public override void OnExitState()
        {

        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnUpdateState()
        {
            Controller.Aim();
            Controller.LookAimTarget();
            Controller.IncreaseAnimLayerWeight(1, 1f);

            curTime += Time.deltaTime / Time.timeScale;
            if (curTime >= zoomingTime)
            {
                Controller.ChangeState(PlayerStateName.Charging);
            }

            if (Input.GetMouseButtonUp(0))
            {
                Controller.SetTimeScale(1f);
                Controller.TurnOffAimCam();
                Controller.SetAnimLayerToDefault(1);

                if (Controller.isGrounded)
                {
                    Controller.ChangeState(PlayerStateName.Idle);
                }
                else
                {
                    Controller.ChangeState(PlayerStateName.Airborne);
                }
            }
        }
    }
}
