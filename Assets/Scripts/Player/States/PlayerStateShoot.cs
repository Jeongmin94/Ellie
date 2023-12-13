using Assets.Scripts.Managers;
using Channels.Combat;
using Channels.Components;
using System;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class PlayerStateShoot : PlayerBaseState
    {
        // !TODO : 쏘고나서 짧은 후딜레이, EnterState에서 돌맹이 쏴주고 잠깐 있다가 다음스테이트로 전이 
        // !TODO : 쏘고 난 후의 애니메이션 필요함
        private float recoilTime;
        private float curTime;

        public PlayerStateShoot(PlayerController controller) : base(controller)
        {
            recoilTime = controller.RecoilTime;
        }

        public override void OnEnterState()
        {
            Controller.Anim.SetBool("IsShooting", true);
            Controller.SetTimeScale(1f);
            curTime = 0;
            if (Controller.CurStoneIdx != 0)
            {
                Controller.shooter.Shoot(Controller.TicketMachine, Controller.CurStoneIdx);
            }

            Controller.TurnOnSlingshot();
            Controller.TurnSlingshotLineRenderer(false);
            SoundManager.Instance.StopSfx("slingshot_sound1");
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "ellie_sound2", Controller.PlayerObj.position);
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "slingshot_sound2", Controller.PlayerObj.position);
            Controller.ShakeCamera(1.0f, 0.2f);

        }


        public override void OnExitState()
        {
            Controller.TurnOffAimCam();
            Controller.SetAnimLayerToDefault(PlayerController.AnimLayer.Aiming);
            Controller.ActivateShootPos(false);
            Controller.Anim.SetBool("IsShooting", false);
            Controller.TurnOffSlingshot();
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnUpdateState()
        {
            curTime += Time.deltaTime;
            if (curTime >= recoilTime)
            {
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