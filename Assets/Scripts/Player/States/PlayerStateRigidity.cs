using Assets.Scripts.Managers;
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

        private string[] ellieRigiditySound = new string[2];
        public PlayerStateRigidity(PlayerController controller) : base(controller)
        {
            ellieRigiditySound[0] = "ellie_sound3";
            ellieRigiditySound[1] = "ellie_sound4";
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
            int soundIdx = UnityEngine.Random.Range(0, ellieRigiditySound.Length);
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, ellieRigiditySound[soundIdx], Controller.transform.position);
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
