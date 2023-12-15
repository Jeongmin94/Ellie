using Assets.Scripts.Controller;
using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using Channels.Boss;
using Channels.Components;
using Channels.Dialog;
using Channels.Type;
using Codice.CM.Common.Encryption;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlasticPipe.Server.MonitorStats;

namespace Assets.Scripts.Boss1
{
    public class TerrapupaDialogController : BaseController
    {
        [ShowInInspector][ReadOnly] private Dictionary<int, bool> dialogSaveDic = new();
        [ShowInInspector][ReadOnly] private BossDialogParsingInfo parsingInfo;
        [ShowInInspector][ReadOnly] private BossDialogData currentData;
        [ShowInInspector] private List<BossDialog> dialogList = new();

        public float dialogDuration = 3.0f;
        public string[] speakers = new string[]{
            "엘리",
            "말하는 해골 머리 첫 째",
            "말하는 해골 머리 둘 째",
            "말하는 해골 머리 막내"};

        private TicketMachine ticketMachine;
        private int currentIndex;

        public TicketMachine TicketMachine
        {
            get { return ticketMachine; }
        }

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
            ticketMachine.AddTickets(ChannelType.Dialog, ChannelType.BossDialog);

            ticketMachine.RegisterObserver(ChannelType.Dialog, OnNotifyDialog);
            ticketMachine.RegisterObserver(ChannelType.BossDialog, OnNotifyBossDialog);
        }

        private IEnumerator InitDialogData()
        {
            yield return DataManager.Instance.CheckIsParseDone();

            parsingInfo = DataManager.Instance.GetData<BossDialogParsingInfo>();

            // 수행 여부 딕셔너리 초기화
            foreach (var data in parsingInfo.datas)
            {
                dialogSaveDic[data.index] = false;
            }
        }

        private void SaveBossDialog()
        {
            var payload = new BossSavePayload
            {
                bossDialogStatusDic = dialogSaveDic,
            };
            SaveLoadManager.Instance.AddPayloadTable(SaveLoadType.Boss, payload);
        }

        private void LoadBossDialog(IBaseEventPayload payload)
        {
            if (payload is not BossSavePayload savePayload) 
                return;

            dialogSaveDic = savePayload.bossDialogStatusDic;
        }

        [Button("테스트 페이로드 8100", ButtonSizes.Large)]
        public void Test1()
        {
            Debug.Log("테스트1");
            ticketMachine.SendMessage(ChannelType.BossDialog, new BossDialogPaylaod
            {
                TriggerType = BossDialogTriggerType.EnterBossRoom,
            });
        }
        [Button("다이얼로그 테스트1", ButtonSizes.Large)]
        public void Test2()
        {
            Debug.Log("테스트2");
            var dPayload1 = DialogPayload.Play("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            dPayload1.canvasType = DialogCanvasType.Simple;
            dPayload1.simpleDialogDuration = 3.0f;
            ticketMachine.SendMessage(ChannelType.Dialog, dPayload1);
        }
        [Button("다이얼로그 테스트2", ButtonSizes.Large)]
        public void Test2()
        {
            Debug.Log("테스트2");
            var dPayload1 = DialogPayload.Play("fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff");
            dPayload1.canvasType = DialogCanvasType.SimpleRemaining;
            dPayload1.simpleDialogDuration = 3.0f;
            ticketMachine.SendMessage(ChannelType.Dialog, dPayload1);
        }
        [Button("다이얼로그 테스트3", ButtonSizes.Large)]
        public void Test3()
        {
            Debug.Log("테스트2");
            var dPayload1 = DialogPayload.OnNext();
            dPayload1.canvasType = DialogCanvasType.SimpleRemaining;
            dPayload1.simpleDialogDuration = 3.0f;
            ticketMachine.SendMessage(ChannelType.Dialog, dPayload1);
        }
        [Button("다이얼로그 테스트4", ButtonSizes.Large)]
        public void Test4()
        {
            Debug.Log("테스트2");
            var dPayload1 = DialogPayload.Play("fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff");
            dPayload1.canvasType = DialogCanvasType.SimpleRemaining;
            dPayload1.simpleDialogDuration = 3.0f;
            ticketMachine.SendMessage(ChannelType.Dialog, dPayload1);
        }


        private void OnNotifyDialog(IBaseEventPayload payload)
        {
            if (payload is not DialogPayload dialogPayload)
                return;

            if (dialogPayload.dialogType != DialogType.NotifyToClient && currentData != null) 
                return;

            var isPlaying = dialogPayload.isPlaying;
            if (isPlaying)
            {
                Debug.Log("켜짐");
                currentIndex++;
            }
            else
            {
                Debug.Log("꺼짐");
            }
        }

        private void OnNotifyBossDialog(IBaseEventPayload payload)
        {
            Debug.Log("보스다이얼로그 요청받음");

            if (payload is not BossDialogPaylaod dialogPayload)
                return;

            currentData = parsingInfo.GetIndexData<BossDialogData>((int)dialogPayload.TriggerType);
            dialogList = currentData.dialogList;
            currentIndex = 0;

            SendDialogMessage(dialogList[currentIndex].dialog, dialogList[currentIndex].dialogCanvasType, dialogList[currentIndex].speaker);
        }

        private void SendDialogMessage(string text, DialogCanvasType canvasType, int speaker)
        {
            var dPayload = DialogPayload.Play(text);
            dPayload.canvasType = canvasType;
            dPayload.speaker = speaker > 0 ? speakers[speaker - 1] : "";
            dPayload.simpleDialogDuration = dialogDuration;
            ticketMachine.SendMessage(ChannelType.Dialog, dPayload);
        }

        private void CheckBossDialogType(BossDialogType type)
        {
            switch (type)
            {
                case BossDialogType.None:
                    break;
                case BossDialogType.EnterBossRoom:
                    break;
                case BossDialogType.ShowExitDoor:
                    break;
                case BossDialogType.StandUpTerrapupa:
                    break;
                case BossDialogType.StartBattle:
                    break;
                case BossDialogType.HighlightExitDoor:
                    break;
                default:
                    break;
            }
        }
    }
}