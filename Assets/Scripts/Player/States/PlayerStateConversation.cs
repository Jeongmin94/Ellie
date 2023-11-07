using Assets.Scripts.NPCs;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class PlayerStateConversation : PlayerBaseState
    {
        public PlayerStateConversation(PlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState()
        {
            Controller.canTurn = false;
            Vector3 direction = Controller.GetComponent<PlayerInteraction>().interactiveObject.transform.position - Controller.PlayerObj.transform.position;
            direction.y = 0;
            Controller.PlayerObj.rotation = Quaternion.LookRotation(direction);

        }

        public override void OnExitState()
        {
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnUpdateState()
        {
            if(Input.GetKeyDown(KeyCode.Y))
            {
                Controller.GetComponent<PlayerInteraction>().interactiveObject?.GetComponent<BaseNPC>().EndInteract();
                Controller.EndConversation();
                Controller.ChangeState(PlayerStateName.Idle);
            }
            
        }
    }
}
