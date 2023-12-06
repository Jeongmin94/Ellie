using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class PlayerStateDown : PlayerBaseState
    {
        private float duration;
        private float curTime;
        private bool isForceAdded;
        private float force;

        private bool isGrounded = true;

        private string[] ellieRigiditySound = new string[2];
        private string[] ellieGroundedSound = new string[2];
        public PlayerStateDown(PlayerController controller) : base(controller)
        {
            ellieRigiditySound[0] = "ellie_sound3";
            ellieRigiditySound[1] = "ellie_sound4";
            ellieGroundedSound[0] = "ellie_sound5";
            ellieGroundedSound[1] = "ellie_sound6";
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

            int soundIdx = UnityEngine.Random.Range(0, ellieRigiditySound.Length);
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, ellieRigiditySound[soundIdx], Controller.transform.position);

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

            //가해지는 힘이 존재한다면
            if(0 != force)
            {
                bool curIsGrounded = Physics.Raycast(Controller.transform.position,
                Vector3.down, Controller.PlayerHeight * 0.5f + 0.3f, Controller.GroundLayer);
                if(!isGrounded && isGrounded != curIsGrounded)
                {
                    int idx = Random.Range(0, ellieGroundedSound.Length);
                    SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, ellieGroundedSound[idx],
                        Controller.transform.position);
                }
                isGrounded = curIsGrounded;
            }
        }

        public override void OnUpdateState()
        {
            curTime += Time.deltaTime;
            if (curTime >= Controller.TimeToDodgeAfterDown && Input.GetKeyDown(KeyCode.LeftControl))
            {
                Controller.ChangeState(PlayerStateName.Dodge);
            }
            if (curTime >= duration)
            {
                Controller.ChangeState(PlayerStateName.GetUp);
            }
        }
    }
}