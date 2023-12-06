using Assets.Scripts.Managers;
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
            Controller.PlayerStatus.isRecoveringStamina = false;
            Controller.TurnOnSlingshot();
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "slingshot_sound1", Controller.transform.position);
        }

        public override void OnExitState()
        {
            Controller.PlayerStatus.isRecoveringStamina = true;
            Controller.TurnOffSlingshot();
        }

        public override void OnFixedUpdateState()
        {
            Controller.MovePlayer(moveSpeed);
        }

        public override void OnUpdateState()
        {
            Controller.Aim();
            Controller.LookAimTarget();
            Controller.PlayerStatus.ConsumeStamina(Controller.PlayerStatus.ChargeStaminaComsumptionPerSec * Time.deltaTime / Time.timeScale);

            if (Input.GetMouseButtonUp(0))
            {
                Controller.ChangeState(PlayerStateName.Shoot);
            }
            //Controller.GrabSlingshotLeather();
        }
    }
}
