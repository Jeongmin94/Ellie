using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class PlayerStateRigidity : PlayerBaseState
    {
        private float duration;
        private float curTime;
        public PlayerStateRigidity(PlayerController controller) : base(controller)
        {
        }
        public override void OnEnterState()
        {

        }

        public override void OnEnterState(StateInfo info)
        {
            Controller.Anim.SetTrigger("EnterRigidity");
            Controller.canTurn = false;
            Controller.SetTimeScale(1f);
            Controller.TurnOffAimCam();
            Controller.SetAnimLayerToDefault(PlayerController.AnimLayer.Aiming);
            Controller.ActivateShootPos(false);
            Controller.isRigid = true;
            duration = info.stateDuration;
            curTime = 0;
        }


        public override void OnExitState()
        {
            Controller.Anim.SetTrigger("ExitRigidity");
            Controller.canTurn = true;
            Controller.isRigid = false;
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnUpdateState()
        {
            curTime += Time.deltaTime;
            if(curTime >= duration)
            {
                Controller.ChangeState(PlayerStateName.Idle);
            }
        }
    }
}
