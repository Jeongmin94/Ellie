using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class PlayerStateDown : PlayerBaseState
    {
        private float duration;
        private float curTime;
        private bool temp;
        private float force;
        public PlayerStateDown(PlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState()
        {
        }
        public override void OnEnterState(StateInfo info)
        {
            temp = false;
            duration = info.stateDuration;
            force = info.magnitude;
            curTime = 0;

            Controller.Anim.SetTrigger("Down");
            Controller.canTurn = false;
            Controller.SetTimeScale(1f);
            Controller.TurnOffAimCam();
            Controller.SetAnimLayerToDefault(1);
            Controller.ActivateShootPos(false);
            Controller.isRigid = true;

        }
        public override void OnExitState()
        {

        }

        public override void OnFixedUpdateState()
        {
            if (!temp && 0 != force)
            {
                Controller.Rb.velocity = Vector3.zero;
                Controller.Rb.AddForce(Controller.PlayerObj.up * force, ForceMode.Impulse);
                temp = true;
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