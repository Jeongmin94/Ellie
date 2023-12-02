﻿using Assets.Scripts.Data.GoogleSheet;
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

        Sprite QuestUISprite;

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

            //퀘스트 UI 스프라이트 로드
            QuestUISprite = Resources.Load<Sprite>("Images/UI/QuestUI");
            //퀘스트 세이브 로드
            StartCoroutine(FirstTimeSequenceCoroutine());
        }

        private IEnumerator FirstTimeSequenceCoroutine()
        {
            while(SaveLoadManager.Instance.IsLoadData)
            {
                yield return null;
            }

            SaveLoadManager.Instance.SubscribeSaveEvent(SaveQuestData);
            SaveLoadManager.Instance.SubscribeLoadEvent(SaveLoadType.Quest, LoadQuestData);
            if (questStatusDic[6100] == QuestStatus.CantAccept)
                StartCoroutine(FirstDialogCoroutine());
        }
        private void Update()
        {
            //퀘스트 상태 디버깅용
            if (Input.GetKeyDown(KeyCode.B))
            {
                SaveLoadManager.Instance.SaveData();
                DebugCurrentPlayerQuestDict();
            }
            if(Input.GetKeyDown(KeyCode.V))
            {
                SaveLoadManager.Instance.LoadData();
                DebugCurrentPlayerQuestDict();
            }
        }

        private void DebugCurrentPlayerQuestDict()
        {
            foreach(var item in questStatusDic)
            {
                Debug.Log($"{item.Key}번 째 퀘스트 상태 : {item.Value}");
            }
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

            SendClearQuestMessage();
            if (dialogList == null)
            {
                Debug.Log("DialogList is Null");
                yield break;
            }
            LockPlayerMovement();
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
                    {
                        SoundManager.Instance.PlaySound(SoundManager.SoundType.UISfx, "dialogue1");

                        SendPlayDialogPayload(dialogList[curDialogListIdx]);
                    }
                }
                if (curDialogListIdx == dialogList.Count)
                {
                    //player.EndConversation();
                    //yield return ResetRotation();

                    //대화 출력이 끝나면 퀘스트 수락 상태로 변경
                    if (Input.GetKeyDown(KeyCode.G))
                    {
                        SetQuestStatus(questDataList[FirstQuestDataIdx].index, QuestStatus.Accepted);
                        SendStopDialogPayload(DialogCanvasType.Default);
                        SendStopDialogPayload(DialogCanvasType.SimpleRemaining);
                        UnlockPlayerMovement();
                        yield break;
                    }
                }
                yield return null;
            }
            
        }

        public IEnumerator DialogCoroutine(int questIdx, QuestStatus status, bool isAdditionalDialog = false)
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
            DeactivateInteractiveUI();

            //첫 대사 출력
            SendPlayDialogPayload(dialogList[curDialogListIdx]);

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
                    {
                        SoundManager.Instance.PlaySound(SoundManager.SoundType.UISfx, "dialogue1");

                        SendPlayDialogPayload(dialogList[curDialogListIdx]);
                    }
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
        
        private void SendPlayDialogPayload(int dialogIdx, bool first = false)
        {
            DialogData dialogData = DataManager.Instance.GetIndexData<DialogData, DialogDataParsingInfo>(dialogIdx);
            string text = dialogData.dialog;
            string speaker = "";

            DialogPayload payload = DialogPayload.Play(text, 0.5f);

            switch (dialogData.speaker)
            {
                case 1:
                    SendStopDialogPayload(DialogCanvasType.SimpleRemaining);
                    speaker = "엘리";
                    break;
                case 2:
                    SendStopDialogPayload(DialogCanvasType.Default);
                    payload.canvasType = DialogCanvasType.SimpleRemaining;
                    break;
                default:
                    SendStopDialogPayload(DialogCanvasType.SimpleRemaining);
                    speaker = DataManager.Instance.GetIndexData<NPCData, NPCDataParsingInfo>(dialogData.speaker).name;
                    break;
            }

            payload.speaker = speaker;

            ticketMachine.SendMessage(ChannelType.Dialog, payload);
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
            if (payload is not DialogPayload dialogPayload) return;

            if (dialogPayload.dialogType != DialogType.NotifyToClient) return;

            isPlaying = dialogPayload.isPlaying;
            if(isPlaying)
            {
                SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "dialogue2", transform.position);
            }
            else
            {
                SoundManager.Instance.StopSfx("dialogue2");
            }
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
                //퀘스트 갱신시마다 세이브
                SaveLoadManager.Instance.SaveData();
                //ui채널에 보내기
                QuestData data = questDataList[questIdx % 6100];
                //newStatus 가 end 이면 clear하고 아무것도 안함
                //그 이외의 status면 clear하고 다시 띄워주기
                if (newStatus == QuestStatus.End)
                {
                    SendClearQuestMessage();
                }
                else
                {
                    SendClearQuestMessage();
                    SendDisplayQuestMessage(data);
                }

            }
            else
            {
                Debug.Log($"Quest status not found for quest index {questIdx}");
            }
        }

        private void SendClearQuestMessage()
        {
            controller.TicketMachine.SendMessage(ChannelType.UI, new UIPayload
            {
                uiType = UIType.Notify,
                actionType = ActionType.ClearQuest,
            });
        }

        private void SendDisplayQuestMessage(QuestData data)
        {
            QuestInfo info = new();

            // !TODO : 이미지를 추가해야 합니다
            info.questName = data.name;
            info.questDesc = data.playableText;
            info.questIcon = QuestUISprite;
            controller.TicketMachine.SendMessage(ChannelType.UI, new UIPayload
            {
                uiType = UIType.Notify,
                actionType = ActionType.SetQuestName,
                questInfo = info,
            }); ;
            controller.TicketMachine.SendMessage(ChannelType.UI, new UIPayload
            {
                uiType = UIType.Notify,
                actionType = ActionType.SetQuestDesc,
                questInfo = info,
            });
            controller.TicketMachine.SendMessage(ChannelType.UI, new UIPayload
            {
                uiType = UIType.Notify,
                actionType = ActionType.SetQuestIcon,
                questInfo = info,
            });
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
            Vector3 direction = dest.position - controller.PlayerObj.position;
            //방향 돌리고
            StartCoroutine(GetBackToNPCCoroutine(dest));
            //앞쪽으로 1만큼만 이동(테스트)
            controller.transform.Translate(direction.normalized * 1f);
        }

        private IEnumerator GetBackToNPCCoroutine(Transform dest)
        {
            if (dest == null) yield break;
            while(true)
            {
                Vector3 direction = dest.position - controller.PlayerObj.position;
                direction.y = 0;
                Quaternion rotation = Quaternion.LookRotation(direction);
                controller.PlayerObj.rotation = Quaternion.Slerp(controller.PlayerObj.rotation, rotation, Time.deltaTime * 10f);

                float angleDifference = Quaternion.Angle(controller.PlayerObj.rotation, rotation);

                if(angleDifference <=1.0f) 
                {
                    yield break;
                }
                yield return null;
            }
            
        }
        public void LockPlayerMovement()
        {
            controller.canMove = false;
            controller.canTurn = false;
            controller.ChangeState(PlayerStateName.Conversation);
        }

        public void UnlockPlayerMovement()
        {
            controller.canMove = true;
            controller.canTurn = true;
            controller.ChangeState(PlayerStateName.Idle);

        }

        public void GetPickaxe(int pickaxeIdx)
        {
            controller.GetPickaxe(pickaxeIdx);
        }

        private void SaveQuestData()
        {
            QuestSavePayload payload = new QuestSavePayload();
            payload.questStatusSaveInfo = questStatusDic;

            SaveLoadManager.Instance.AddPayloadTable(SaveLoadType.Quest, payload);
        }

        private void LoadQuestData(IBaseEventPayload payload)
        {
            if (payload is not QuestSavePayload questSavePayload) return;
            questStatusDic.Clear();

            questStatusDic = questSavePayload.questStatusSaveInfo;
        }

        public void ActivateInteractiveUI()
        {
            GetComponent<PlayerInteraction>().ActivateInteractiveUI();
        }
        public void DeactivateInteractiveUI()
        {
            GetComponent<PlayerInteraction>().DeactivateInteractiveUI();
        }

        public void SetInteractiveObjToNull()
        {
            GetComponent<PlayerInteraction>().interactiveObject = null;
            GetComponent<PlayerInteraction>().SetCanInteract(false);
        }
    }
}