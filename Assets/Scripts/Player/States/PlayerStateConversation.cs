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
            Controller.canMove = false;
                
            Vector3 direction = Controller.GetComponent<PlayerInteraction>().interactiveObject.transform.position - Controller.PlayerObj.transform.position;
            direction.y = 0;
            Controller.PlayerObj.rotation = Quaternion.LookRotation(direction);

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
            //y를 누르면 대화에서 탈출
            if(Input.GetKeyDown(KeyCode.Y))
            {
                //Controller.GetComponent<PlayerInteraction>().interactiveObject?.GetComponent<BaseNPC>().EndInteract();
                Controller.EndConversation();
            }
            //대화 중에 g를 눌러 다음으로
            
            if (Input.GetKeyDown(KeyCode.G))
            {
                //Debug.Log("Press g in conversation state");
                //QuestNPC npc = null;
                //Controller.GetComponent<PlayerInteraction>().interactiveObject?.TryGetComponent(out npc);
                //if (npc)
                //{
                //    npc.Talk(Controller.GetComponent<PlayerQuest>().GetQuestStatus(npc.QuestIdx), Controller.gameObject);
                //}
            }
        }
    }
}
