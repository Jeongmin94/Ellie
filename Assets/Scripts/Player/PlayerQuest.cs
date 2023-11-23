using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.Managers;
using Channels.Components;
using Channels.Dialog;
using Channels.Type;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerQuest : MonoBehaviour
    {
        private PlayerController controller;
        private List<QuestData> questDataList;
        private const int QuestDataIdx = 0;
        private Dictionary<int, QuestStatus> questStatusDic;

        private TicketMachine ticketMachine;
        private bool isPlaying;

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
            //StartCoroutine(FirstDialogCoroutine());
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
            List<int> dialogList = questDataList[QuestDataIdx].unAcceptedDialogList;

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
                        Debug.Log("dialog end input");
                        questStatusDic[questDataList[QuestDataIdx].index] = QuestStatus.Accepted;
                        SendStopDialogPayload(DialogCanvasType.Default);
                        yield break;
                    }
                }
                yield return null;
            }
        }

        private void SendPlayDialogPayload(int dialogIdx, string npcName = "")
        {
            DialogData dialogData = DataManager.Instance.GetIndexData<DialogData, DialogDataParsingInfo>(dialogIdx);
            string text = dialogData.dialog;
            string speaker = "";

            DialogPayload payload = DialogPayload.Play(text, 0.2f);
            switch (dialogData.speaker)
            {
                case 0:
                    SendStopDialogPayload(DialogCanvasType.SimpleRemaining);
                    speaker = npcName;
                    break;
                case 1:
                    SendStopDialogPayload(DialogCanvasType.SimpleRemaining);
                    speaker = "엘리";
                    break;
                case 2:
                    SendStopDialogPayload(DialogCanvasType.Default);
                    payload.canvasType = DialogCanvasType.SimpleRemaining;
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
            if (payload is not DialogPayload) return;

            DialogPayload dialogPayload = payload as DialogPayload;

            if (dialogPayload.dialogType != DialogType.NotifyToClient) return;

            isPlaying = dialogPayload.isPlaying;
        }
    }
}