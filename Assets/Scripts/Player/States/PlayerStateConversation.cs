using UnityEngine;

namespace Player.States
{
    public class PlayerStateConversation : PlayerBaseState
    {
        public PlayerStateConversation(PlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState()
        {
            Controller.canTurn = false;
            Controller.canMove = false;

            if (Controller.GetComponent<PlayerInteraction>().interactiveObject != null)
            {
                var direction = Controller.GetComponent<PlayerInteraction>().interactiveObject.transform.position -
                                Controller.PlayerObj.transform.position;
                direction.y = 0;
                Controller.PlayerObj.rotation = Quaternion.LookRotation(direction);
            }

            Controller.SetAnimLayerToDefault(PlayerController.AnimLayer.Aiming);
        }

        public override void OnExitState()
        {
            Controller.canMove = true;
            Controller.canTurn = true;
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnUpdateState()
        {
        }
    }
}