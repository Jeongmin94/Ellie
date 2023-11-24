using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects.NPC
{
    public class TalkingSkullEldestNPC : BaseNPC
    {
        private const int PLAYERFIRSTQUESTIDX = 6100;
        private enum EldestQuestList
        {
            quest6101,
            quest6102,
        }
        public override void Interact(GameObject obj)
        {
            base.Interact(obj);
            //첫 째가 상호작용할 때 로직
        }

        private void OnTriggerEnter(Collider other)
        {
            //처음 enter 했을 시 6100퀘스트를 클리어하고 보상을 줌
            if (other.CompareTag("Player"))
            {
                player = other.gameObject.GetComponent<PlayerQuest>();
                //6100퀘스트가 수락상태라면 Done으로 변경
                if (player.GetQuestStatus(PLAYERFIRSTQUESTIDX) == Data.GoogleSheet.QuestStatus.Accepted)
                {
                    player.SetQuestStatus(PLAYERFIRSTQUESTIDX, Data.GoogleSheet.QuestStatus.Done);
                    //보상 주기
                    player.GetReward(PLAYERFIRSTQUESTIDX);
                }
                
            }
        }

        private void OnTriggerExit(Collider other)
        {

        }
    }
}