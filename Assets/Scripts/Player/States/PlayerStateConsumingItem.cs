using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class PlayerStateConsumingItem : PlayerBaseState
    {
        private const float ConsumingStateTime = 1.5f;
        private float accTime = 0;
        private float moveSpeed;
        public PlayerStateConsumingItem(PlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState()
        {
            moveSpeed = Controller.WalkSpeed / 2;
            accTime = 0;
            Controller.Anim.SetBool("IsConsuming", true);
        }

        public override void OnExitState()
        {
            Controller.SetAnimLayerToDefault(PlayerController.AnimLayer.Consuming);
            Controller.Anim.SetBool("IsConsuming", false);

        }

        public override void OnFixedUpdateState()
        {
            Controller.MovePlayer(moveSpeed);
        }

        public override void OnUpdateState()
        {
            Controller.IncreaseAnimLayerWeight(PlayerController.AnimLayer.Consuming, 1f);
            accTime += Time.deltaTime;
            if (accTime >= ConsumingStateTime)
            {
                Controller.ChangeState(PlayerStateName.Idle);
            }
        }
    }
}
