using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class PlayerStateDown : PlayerBaseState
    {
        private float duration;
        private float curTime;
        private bool isForceAdded;
        private float force;
        public PlayerStateDown(PlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState()
        {
        }
        public override void OnEnterState(StateInfo info)
        {
            isForceAdded = false;
            duration = info.stateDuration;
            force = info.magnitude;
            curTime = 0;

            Controller.Anim.SetTrigger("Down");
            Controller.canTurn = false;
            Controller.SetTimeScale(1f);
            Controller.TurnOffAimCam();
            Controller.SetAnimLayerToDefault(PlayerController.AnimLayer.Aiming);
            Controller.ActivateShootPos(false);
            Controller.isRigid = true;

        }
        public override void OnExitState()
        {

        }

        public override void OnFixedUpdateState()
        {
            if (!isForceAdded && 0 != force)
            {
                Controller.Rb.velocity = Vector3.zero;
                Controller.Rb.velocity = Controller.PlayerObj.up * force;
                isForceAdded = true;
            }
        }

        public override void OnUpdateState()
        {
            curTime += Time.deltaTime;
            if (curTime >= duration)
            {
                Controller.ChangeState(PlayerStateName.GetUp);
            }
        }
    }
}