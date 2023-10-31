using Assets.Scripts.Managers;
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
        private GameObject stone;
        public PlayerStateShoot(PlayerController controller) : base(controller)
        {
            recoilTime = controller.RecoilTime;
        }

        public override void OnEnterState()
        {
            stone = Controller.Stone;
            Controller.Anim.SetBool("IsShooting", true);
            Controller.SetTimeScale(1f);
            curTime = 0;
            // !TODO : 풀에서 돌맹이 가져와서 Shooter에 전달
            PoolManager.Instance.Pop(stone);
            Controller.shooter.GetComponent<Shooter>().Shoot(PoolManager.Instance.Pop(stone));
        }

        public override void OnExitState()
        {
            Controller.TurnOffAimCam();
            Controller.SetAimingAnimLayerToDefault();
            Controller.ActivateShootPos(false);
            Controller.Anim.SetBool("IsShooting", false);
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
