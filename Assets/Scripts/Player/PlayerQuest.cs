using Assets.Scripts.Data.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerQuest : MonoBehaviour
    {
        //스크립터블 오브젝트
        [SerializeField] private QuestDataList QuestDataListObj;
        const int DEFAULT = 6100;


        private void Start()
        {
            InitQuestDataList();
        }

        private void InitQuestDataList()
        {
            // !TODO : 구글 스프레드 시트에서 데이터를 파싱하여 List를 갱신
            foreach (QuestData data in QuestDataListObj.questDataList)
            {
                data.status = QuestStatus.CantAccept;
            }
            //첫 퀘스트는 수락 가능 상태로 변경
            QuestDataListObj.questDataList[0].status = QuestStatus.Unaccepted;
        }

        public void RenewQuestStatus(int questIdx, QuestStatus status)
        {
            QuestDataListObj.questDataList[questIdx % DEFAULT].status = status;
        }

        public QuestStatus GetQuestStatus(int questIdx)
        {
            return QuestDataListObj.questDataList[questIdx % DEFAULT].status;
        }

        private void Update()
        {
            //test
            if(Input.GetKeyDown(KeyCode.M))
            {
                QuestDataListObj.questDataList[0].status = QuestStatus.Done;
            }
            if(Input.GetKeyDown(KeyCode.N))
            {
                QuestDataListObj.questDataList[0].status = QuestStatus.Accepted;
            }
        }
    }
}