using Assets.Scripts.Controller;
using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using Channels.Boss;
using Channels.Components;
using Channels.Dialog;
using Channels.Type;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Boss1
{
    public class TerrapupaDialogController : BaseController
    {
        [ShowInInspector][ReadOnly] private Dictionary<int, bool> dialogSaveDic = new();
        [ShowInInspector][ReadOnly] private BossDialogParsingInfo parsingInfo;
        [ShowInInspector][ReadOnly] private BossDialogData currentData = null;
        [ShowInInspector] private List<BossDialog> dialogList = new();
        

        [OnValueChanged("OnChangeIndex")] public int currentIndex;

        public string[] speakers = new string[]
        {
            "엘리",
            "말하는 해골 머리 첫 째",
            "말하는 해골 머리 둘 째",
            "말하는 해골 머리 막내",
            "테라푸파"};
        public string[] images = new string[]
        {
            "Ellie",
            "talking skull head1",
            "talking skull head2",
            "talking skull head3",
            "Frame 8",
        };

        [ShowInInspector] private TicketMachine ticketMachine;
        [ShowInInspector] private Coroutine dialogCoroutine = null;
        [ShowInInspector] private DialogCanvasType currentType;
        [ShowInInspector] private bool isInit;

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

        [Button]
        public void TestTriggerBossDialog(BossDialogTriggerType type = BossDialogTriggerType.EnterBossRoom)
        {
            Debug.Log($"테스트 타입 :: {type}");
            ticketMachine.SendMessage(ChannelType.BossDialog, new BossDialogPaylaod
            {
                TriggerType = type,
            });
        }

        [Button]
        public void TestStopBossDialog()
        {
            isInit = false;
            SendStopDialogPayload(DialogCanvasType.Default);
            SendStopDialogPayload(DialogCanvasType.Simple);
            SendStopDialogPayload(DialogCanvasType.SimpleRemaining);
            SendStopDialogPayload(DialogCanvasType.GuideDialog);
            isInit = true;
        }

        private void OnNotifyDialog(IBaseEventPayload payload)
        {
            if (payload is not DialogPayload dialogPayload)
                return;

            if (dialogPayload.dialogType != DialogType.NotifyToClient || currentData == null) 
                return;

            if(isInit)
            {
                if(dialogPayload.isEnd)
                {
                    Debug.Log("End OK");
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
                else if (currentType != DialogCanvasType.Simple)
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
            Debug.Log("다음 대사");
            InitDialog();

            currentType = dialogList[currentIndex].dialogCanvasType;
            if (dialogCoroutine != null && currentType == DialogCanvasType.Simple)
            {
                StopCoroutine(dialogCoroutine);
            }
            SendDialogMessage(dialogList[currentIndex].dialog, dialogList[currentIndex].dialogCanvasType, dialogList[currentIndex].speaker, dialogList[currentIndex].remainTime);
        }

        private void EndDialog()
        {
            Debug.Log("대사 종료");
            InitDialog();
            currentIndex = 0;
            dialogList = null;
            currentData = null;
        }

        private void OnNotifyBossDialog(IBaseEventPayload payload)
        {
            if (payload is not BossDialogPaylaod dialogPayload)
                return;

            currentData = parsingInfo.GetIndexData<BossDialogData>((int)dialogPayload.TriggerType);
            dialogList = currentData.dialogList;
            currentIndex = 0;

            NextDialog();
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
            DialogPayload payload = DialogPayload.Stop();
            payload.canvasType = type;
            ticketMachine.SendMessage(ChannelType.Dialog, payload);
        }

        public void OnChangeIndex()
        {
            Debug.Log($"OnChangeIndex :: {currentIndex}, {currentType}");
        }
    }
}