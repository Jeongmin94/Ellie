using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.Managers;
using Channels.Components;
using Channels.Dialog;
using Channels.Type;
using Channels.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerQuest : MonoBehaviour
    {
        private PlayerController controller;
        private List<QuestData> questDataList;
        private const int FirstQuestDataIdx = 0;
        private Dictionary<int, QuestStatus> questStatusDic;


        private TicketMachine ticketMachine;
        public bool isPlaying;

        private void Awake()
        {
            controller = GetComponent<PlayerController>();
        }

        private void Start()
        {
            ticketMachine = controller.TicketMachine;
            questStatusDic = new();
            StartCoroutine(InitPlayerQuest());
            //6100번 퀘스트 시작
            StartCoroutine(FirstDialogCoroutine());
        }

        private IEnumerator InitPlayerQuest()
        {
            yield return DataManager.Instance.CheckIsParseDone();

            questDataList = DataManager.Instance.GetData<QuestDataParsingInfo>().questDatas;
            foreach (QuestData data in questDataList)
            {
                questStatusDic.Add(data.index, QuestStatus.CantAccept);
            }

            // !TODO : 플레이어의 퀘스트 데이터를 로딩합니다
        }


        private IEnumerator FirstDialogCoroutine()
        {
            yield return DataManager.Instance.CheckIsParseDone();
            int curDialogListIdx = 0;
            List<int> dialogList = questDataList[FirstQuestDataIdx].DialogListDic[QuestStatus.Unaccepted];

            if (dialogList == null)
            {
                Debug.Log("DialogList is Null");
                yield break;
            }

            SendPlayDialogPayload(dialogList[curDialogListIdx]);

            while (true)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    //다이얼로그 출력 로직
                    //isPlaying일 때 페이로드를 보내면 next가 호출되게 함
                    //isPlaying이 아닐 때 페이로드를 보내야 다음 대사가 출력
                    if (!isPlaying)
                        curDialogListIdx++;
                    if (curDialogListIdx < dialogList.Count)
                        SendPlayDialogPayload(dialogList[curDialogListIdx]);
                }
                if (curDialogListIdx == dialogList.Count)
                {
                    //player.EndConversation();
                    //yield return ResetRotation();

                    //대화 출력이 끝나면 퀘스트 수락 상태로 변경
                    if (Input.GetKeyDown(KeyCode.G))
                    {
                        questStatusDic[questDataList[FirstQuestDataIdx].index] = QuestStatus.Accepted;
                        SendStopDialogPayload(DialogCanvasType.Default);
                        SendStopDialogPayload(DialogCanvasType.SimpleRemaining);
                        yield break;
                    }
                }
                yield return null;
            }
        }

        public IEnumerator DialogCoroutine(int questIdx, QuestStatus status, string NPCName, bool isAdditionalDialog = false)
        {
            int curDialogListIdx = 0;
            List<int> dialogList;

            if (isAdditionalDialog)
                dialogList = questDataList[questIdx % 6100].additionalConditionDialogList;
            else
                dialogList = questDataList[questIdx % 6100].DialogListDic[status];
            if (dialogList == null)
            {
                Debug.Log("DialogList is Null");
                yield break;
            }

            //첫 대사 출력
            SendPlayDialogPayload(dialogList[curDialogListIdx], NPCName);

            while(true)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    //다이얼로그 출력 로직
                    //isPlaying일 때 페이로드를 보내면 next가 호출되게 함
                    //isPlaying이 아닐 때 페이로드를 보내야 다음 대사가 출력
                    if (!isPlaying)
                        curDialogListIdx++;
                    if (curDialogListIdx < dialogList.Count)
                        SendPlayDialogPayload(dialogList[curDialogListIdx], NPCName);
                }
                if (curDialogListIdx == dialogList.Count)
                {
                    //player.EndConversation();
                    //yield return ResetRotation();

                    //대화 출력이 끝나면 퀘스트 수락 상태로 변경
                    if (Input.GetKeyDown(KeyCode.G))
                    {
                        SendStopDialogPayload(DialogCanvasType.Default);
                        SendStopDialogPayload(DialogCanvasType.SimpleRemaining);
                        yield break;
                    }
                }
                yield return null;
            }
        }
        
        private void SendPlayDialogPayload(int dialogIdx, string npcName = "???")
        {
            DialogData dialogData = DataManager.Instance.GetIndexData<DialogData, DialogDataParsingInfo>(dialogIdx);
            string text = dialogData.dialog;
            string speaker = "";

            DialogPayload payload = DialogPayload.Play(text, 0.2f);
            switch (dialogData.speaker)
            {
                case DialogSpeaker.NPC:
                    SendStopDialogPayload(DialogCanvasType.SimpleRemaining);
                    speaker = npcName;
                    break;
                case DialogSpeaker.Player:
                    SendStopDialogPayload(DialogCanvasType.SimpleRemaining);
                    speaker = "엘리";
                    break;
                case DialogSpeaker.Narr:
                    SendStopDialogPayload(DialogCanvasType.Default);
                    payload.canvasType = DialogCanvasType.SimpleRemaining;
                    break;
            }

            payload.speaker = speaker;
            ticketMachine.SendMessage(ChannelType.Dialog, payload);
            Debug.Log(payload.canvasType);
        }

        private void SendStopDialogPayload(DialogCanvasType type)
        {
            DialogPayload payload = DialogPayload.Stop();
            payload.canvasType = type;
            ticketMachine.SendMessage(ChannelType.Dialog, payload);
        }
        public void StartConversation()
        {
            controller.StartConversation();
        }
        public void EndConversation()
        {
            controller.EndConversation();
        }

        public void SetIsPlaying(IBaseEventPayload payload)
        {
            if (payload is not DialogPayload) return;

            DialogPayload dialogPayload = payload as DialogPayload;

            if (dialogPayload.dialogType != DialogType.NotifyToClient) return;

            isPlaying = dialogPayload.isPlaying;
        }

        public QuestStatus GetQuestStatus(int questIdx)
        {
            if (questStatusDic.TryGetValue(questIdx, out QuestStatus status))
            {
                return status;
            }
            else
            {
                throw new KeyNotFoundException($"Quest status not found for quest index {questIdx}");
            }
        }
        public void SetQuestStatus(int questIdx, QuestStatus newStatus)
        {
            if (questStatusDic.ContainsKey(questIdx))
            {
                questStatusDic[questIdx] = newStatus;
            }
            else
            {
                Debug.Log($"Quest status not found for quest index {questIdx}");
            }
        }
        public void GetReward(int questIdx)
        {
            //해당 퀘스트의 데이터를 가져옴
            QuestData selectedQuest = questDataList.FirstOrDefault(quest => quest.index == questIdx);
            foreach (var itemTuple in selectedQuest.rewardList)
            {
                AcquireItem(itemTuple.Item1, itemTuple.Item2);
            }
        }

        private void AcquireItem(int itemIdx, int count)
        {
            UIPayload payload = new();
            payload.uiType = UIType.Notify;
            //아이템 종류 : 돌멩이(4000~), 소모품 및 기타(4100~)
            if (itemIdx >= 4100)
            {
                payload.slotAreaType = UI.Inventory.SlotAreaType.Item;
                payload.actionType = ActionType.AddSlotItem;
                payload.itemData = DataManager.Instance.GetIndexData<ItemData,ItemDataParsingInfo>(itemIdx);
                payload.groupType = payload.itemData.groupType;
            }
            else if(itemIdx >= 4000)
            {
                payload.slotAreaType = UI.Inventory.SlotAreaType.Item;
                payload.actionType = ActionType.AddSlotItem;
                payload.itemData = DataManager.Instance.GetIndexData<StoneData, StoneDataParsingInfo>(itemIdx);
                payload.groupType = payload.itemData.groupType;
            }
            for(int i  = 0; i < count; i++) 
                ticketMachine.SendMessage(ChannelType.UI, payload);
        }

        public void GetBackToNPC(Transform dest)
        {

            //방향 돌리고
            Vector3 direction = dest.position - controller.PlayerObj.position;
            direction.y = 0;
            controller.PlayerObj.rotation = Quaternion.LookRotation(direction);

            //앞쪽으로 1만큼만 이동(테스트)
            controller.transform.Translate(direction.normalized * 1f);
        }

        public void LockPlayerMovement()
        {
            controller.canMove = false;
            controller.canTurn = false;
        }

        public void UnlockPlayerMovement()
        {
            controller.canMove = true;
            controller.canTurn = true;
        }
    }
}