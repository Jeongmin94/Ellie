using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.Data.Quest;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects.NPCs
{
    public class QuestNPC : BaseNPC
    {
        private int curDialogIdx;

        public int QuestIdx 
        { 
            get { return data.questIdx; }
        }   
        public override void Interact(GameObject obj)
        {
            base.Interact(obj);
            //QuestStatus status = CheckQuestStatus(obj);
            //최초 상호작용 시 1회의 대화를 진행합니다
            curDialogIdx = 0;
            Talk(CheckQuestStatus(obj), obj);
        }

        public void Talk(QuestStatus status, GameObject obj)
        {
            //최초 실행시, 또는 Conversation 상태에서 PlayerStateConversation에서 호출됩니다
            Debug.Log("talk");
            switch (status)
            {
                case QuestStatus.CantAccept:
                    //퀘스트를 수락할 수 없을 때의 로직입니다
                    if (curDialogIdx < data.dialogIndexList[(int)status].Count)
                    {
                        PrintDialog(curDialogIdx, status);
                        curDialogIdx++;
                    }
                    else
                    {
                        obj.GetComponent<PlayerController>().EndConversation();
                        EndInteract();
                    }
                    break;
                case QuestStatus.Unaccepted:

                    //최초 퀘스트 수락시의 로직입니다
                    if (curDialogIdx < data.dialogIndexList[(int)status].Count)
                    {
                        PrintDialog(curDialogIdx, status);
                        curDialogIdx++;
                    }
                    else
                    {
                        obj.GetComponent<PlayerQuest>().RenewQuestStatus(data.questIdx, QuestStatus.Accepted);
                        obj.GetComponent<PlayerController>().EndConversation();
                        EndInteract();

                    }
                    //전부 출력했을 경우

                    break;
                case QuestStatus.Accepted:
                    //퀘스트는 수락했으나, 완료하지 못한 상황에서의 로직입니다
                    if (curDialogIdx < data.dialogIndexList[(int)status].Count)
                    {
                        PrintDialog(curDialogIdx, status);
                        curDialogIdx++;
                    }
                    else
                    {
                        obj.GetComponent<PlayerController>().EndConversation();
                        EndInteract();

                    }
                    break;
                case QuestStatus.Done:
                    //퀘스트 조건이 완료된 상태에서의 로직입니다.
                    if (curDialogIdx < data.dialogIndexList[(int)status].Count)
                    {
                        PrintDialog(curDialogIdx, status);
                        curDialogIdx++;
                    }
                    else
                    {
                        obj.GetComponent<PlayerQuest>().RenewQuestStatus(data.questIdx, QuestStatus.End);
                        //다음 퀘스트를 수락 가능한 상태로 변경합니다
                        // !TODO : 플레이어에게 보상을 줘야 합니다
                        GiveReward();
                        obj.GetComponent<PlayerQuest>().RenewQuestStatus(data.questIdx + 1, QuestStatus.Unaccepted);
                        obj.GetComponent<PlayerController>().EndConversation();
                        EndInteract();

                    }

                    break;
                case QuestStatus.End:
                    //퀘스트를 종료한 상태에서의 로직입니다.
                    if (curDialogIdx < data.dialogIndexList[(int)status].Count)
                    {
                        PrintDialog(curDialogIdx, status);
                        curDialogIdx++;
                    }
                    else
                    {
                        obj.GetComponent<PlayerController>().EndConversation();
                        EndInteract();

                    }
                    break;
            }
        }
        private void PrintDialog(int curDialogIdx, QuestStatus status)
        {
            if (!data.dialogIndexList[(int)status].Any()) return;
            //dialogIndexList에는 퀘스트 상태에 맞게 출력해야하는 대사들의 인덱스들이 포함됩니다
            //상호작용 입력이 들어왔을 때 순차적으로 탐색하여 출력합니다
            int idx = data.dialogIndexList[(int)status][curDialogIdx];
            //!TODO : 대사를 로그가 아닌 UI로 출력해야 합니다
            Debug.Log(DataManager.Instance.GetIndexData<DialogData, DialogDataParsingInfo>(idx).dialog);
        }

        protected virtual void GiveReward()
        {
            Debug.Log("퀘스트보상!");
        }
    }
}
