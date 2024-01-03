﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Channels.Boss;
using Channels.Components;
using Channels.Dialog;
using Channels.Type;
using Controller;
using Data.GoogleSheet._8100BossDialog;
using Managers.Data;
using Managers.Save;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Boss1
{
    public class TerrapupaDialogController : BaseController
    {
        public string[] speakers =
        {
            "엘리",
            "말하는 해골 머리 첫 째",
            "말하는 해골 머리 둘 째",
            "말하는 해골 머리 막내",
            "테라푸파"
        };

        public string[] images =
        {
            "Ellie",
            "talking skull head1",
            "talking skull head2",
            "talking skull head3",
            "Frame 8"
        };

        [ShowInInspector] [ReadOnly] private BossDialogData currentData;
        private int currentIndex;
        private DialogCanvasType currentType;
        [ShowInInspector] [ReadOnly] private Dictionary<int, bool> dialogAchievementDic = new();
        private Coroutine dialogCoroutine;

        private Dictionary<int, bool> dialogFirstAchievementDic = new(); // 임시 저장용
        [ShowInInspector] [ReadOnly] private List<BossDialog> dialogList = new();
        private bool isInit;

        private BossDialogParsingInfo parsingInfo;
        private TicketMachine ticketMachine;

        public override void InitController()
        {
            Debug.Log($"{name} InitController");

            SubscribeEvents();
            InitTicketMachine();
            StartCoroutine(InitDialogData());
        }

        private void SubscribeEvents()
        {
            // 세이브, 로드
            SaveLoadManager.Instance.SubscribeSaveEvent(SaveBossDialog);
            SaveLoadManager.Instance.SubscribeLoadEvent(SaveLoadType.Boss, LoadBossDialog);
        }

        private void InitTicketMachine()
        {
            // 티켓머신 초기화 + 옵저버 등록
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.Dialog, ChannelType.BossDialog, ChannelType.BossBattle);

            ticketMachine.RegisterObserver(ChannelType.Dialog, OnNotifyDialog);
            ticketMachine.RegisterObserver(ChannelType.BossDialog, OnNotifyBossDialog);
            ticketMachine.RegisterObserver(ChannelType.BossBattle, OnNotifyBossBattle);
        }

        private IEnumerator InitDialogData()
        {
            yield return DataManager.Instance.CheckIsParseDone();

            parsingInfo = DataManager.Instance.GetData<BossDialogParsingInfo>();

            // 수행 여부 딕셔너리 초기화
            foreach (var data in parsingInfo.datas)
            {
                dialogAchievementDic[data.index] = false;
            }

            dialogFirstAchievementDic = dialogAchievementDic;
        }

        private void SaveBossDialog()
        {
            var payload = new BossSavePayload { bossDialogStatusDic = dialogAchievementDic };
            SaveLoadManager.Instance.AddPayloadTable(SaveLoadType.Boss, payload);
        }

        private void LoadBossDialog(IBaseEventPayload payload)
        {
            if (payload is not BossSavePayload savePayload)
            {
                return;
            }

            dialogAchievementDic = savePayload.bossDialogStatusDic;
            dialogFirstAchievementDic = dialogAchievementDic;
        }

        private void OnNotifyDialog(IBaseEventPayload payload)
        {
            if (payload is not DialogPayload dialogPayload)
            {
                return;
            }

            if (dialogPayload.dialogType != DialogType.NotifyToClient || currentData == null)
            {
                return;
            }

            if (isInit)
            {
                if (dialogPayload.isEnd)
                {
                    if (dialogCoroutine != null)
                    {
                        StopCoroutine(dialogCoroutine);
                    }

                    currentIndex++;
                    if (currentIndex < dialogList.Count)
                    {
                        NextDialog();
                    }
                    else
                    {
                        EndDialog();
                    }

                    dialogCoroutine = null;
                }
                else if (currentType != DialogCanvasType.Simple && currentType != DialogCanvasType.GuideDialog)
                {
                    Debug.Log($"isPlaying == {dialogPayload.isPlaying}");
                    if (dialogPayload.isPlaying == false)
                    {
                        if (dialogCoroutine != null)
                        {
                            StopCoroutine(dialogCoroutine);
                        }

                        dialogCoroutine = StartCoroutine(DialogCoroutine());
                    }
                }
            }
        }

        private void OnNotifyBossDialog(IBaseEventPayload payload)
        {
            if (payload is not TerrapupaDialogPaylaod dialogPayload)
            {
                return;
            }

            if (dialogPayload.TriggerType == TerrapupaDialogTriggerType.None)
            {
                return;
            }

            var data = parsingInfo.GetIndexData<BossDialogData>((int)dialogPayload.TriggerType);
            Debug.Log($"OnNotifyBossDialog() :: {data.index}");

            // false여야 최초 상태로 돌입
            if (!IsCheckedAchievementStatus(data))
            {
                Debug.Log("다이얼로그 출력");
                dialogFirstAchievementDic[data.index] = true;
                currentData = data;
                dialogList = currentData.dialogList;
                currentIndex = 0;

                NextDialog();
            }
        }

        private void OnNotifyBossBattle(IBaseEventPayload payload)
        {
            if (payload is not TerrapupaBattlePayload battlePayload)
            {
                return;
            }

            Debug.Log(battlePayload.SituationType);

            if (battlePayload.SituationType == TerrapupaSituationType.EnterBossRoom)
            {
                var hasDialogAchievement = dialogAchievementDic.ContainsKey((int)TerrapupaDialogTriggerType.EnterBossRoom)
                                           && dialogAchievementDic[(int)TerrapupaDialogTriggerType.EnterBossRoom];

                if ((currentData == null && hasDialogAchievement) ||
                    (currentData != null && currentData.index == (int)TerrapupaSituationType.EnterBossRoom &&
                     hasDialogAchievement))
                {
                    TerrapupaBattleChannel.SendMessage(TerrapupaSituationType.StartBattle, ticketMachine);
                }
            }
        }

        private IEnumerator DialogCoroutine()
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    currentIndex++;
                    if (currentIndex < dialogList.Count)
                    {
                        NextDialog();
                    }
                    else
                    {
                        EndDialog();
                    }

                    dialogCoroutine = null;
                    yield break;
                }

                yield return null;
            }
        }

        private void NextDialog()
        {
            Debug.Log($"{currentIndex} 인덱스");
            InitDialog();

            currentType = dialogList[currentIndex].dialogCanvasType;
            if (dialogCoroutine != null && currentType == DialogCanvasType.Simple)
            {
                StopCoroutine(dialogCoroutine);
            }

            // 상황 별 이벤트 재생
            var bossDialogType = dialogList[currentIndex].terrapupaDialogType;
            if (bossDialogType != TerrapupaSituationType.None)
            {
                SendBossDialogMessage(bossDialogType);
            }

            SendDialogMessage(dialogList[currentIndex].dialog, dialogList[currentIndex].dialogCanvasType,
                dialogList[currentIndex].speaker, dialogList[currentIndex].remainTime);
        }

        private void EndDialog()
        {
            Debug.Log("대사 종료");
            InitDialog();
            dialogAchievementDic[currentData.index] = true;
            SaveLoadManager.Instance.SaveSpecificData(SaveBossDialog);

            currentIndex = 0;
            dialogList = null;
            currentData = null;
        }

        private void InitDialog()
        {
            isInit = false;
            SendStopDialogPayload(DialogCanvasType.Default);
            SendStopDialogPayload(DialogCanvasType.Simple);
            SendStopDialogPayload(DialogCanvasType.SimpleRemaining);
            isInit = true;
        }

        private void SendDialogMessage(string text, DialogCanvasType canvasType, int speaker, float dialogDuration)
        {
            var dPayload = DialogPayload.Play(text);
            dPayload.canvasType = canvasType;
            dPayload.speaker = speaker > 0 ? speakers[speaker - 1] : "";
            dPayload.dialogDuration = dialogDuration;
            dPayload.imageName = speaker > 0 ? images[speaker - 1] : "";
            ticketMachine.SendMessage(ChannelType.Dialog, dPayload);
        }

        private void SendStopDialogPayload(DialogCanvasType type)
        {
            var dPayload = DialogPayload.Stop();
            dPayload.canvasType = type;
            ticketMachine.SendMessage(ChannelType.Dialog, dPayload);
        }

        private void SendBossDialogMessage(TerrapupaSituationType type)
        {
            var payload = new TerrapupaBattlePayload { SituationType = type };
            ticketMachine.SendMessage(ChannelType.BossBattle, payload);
        }

        private bool IsCheckedAchievementStatus(BossDialogData data)
        {
            // 세이브하는 데이터인지 체크
            if (data.isSaveDialog)
            {
                return dialogFirstAchievementDic[data.index];
            }

            return false;
        }

        [Button]
        private void SendMessageBossDialog(TerrapupaDialogTriggerType type = TerrapupaDialogTriggerType.EnterBossRoom)
        {
            var dPayload = new TerrapupaDialogPaylaod { TriggerType = type };
            ticketMachine.SendMessage(ChannelType.BossDialog, dPayload);
        }

        [Button]
        private void TestStopBossDialog()
        {
            isInit = false;
            SendStopDialogPayload(DialogCanvasType.Default);
            SendStopDialogPayload(DialogCanvasType.Simple);
            SendStopDialogPayload(DialogCanvasType.SimpleRemaining);
            SendStopDialogPayload(DialogCanvasType.GuideDialog);
            isInit = true;
        }

        [Button]
        private void InitAchievement()
        {
            // 테스트용 저장상태 초기화
            foreach (var data in parsingInfo.datas)
            {
                dialogAchievementDic[data.index] = false;
            }

            SaveLoadManager.Instance.SaveSpecificData(SaveBossDialog);
        }
    }
}