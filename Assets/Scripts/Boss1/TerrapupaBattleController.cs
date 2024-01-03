﻿using Assets.Scripts.Managers;
using Boss1.BossRoomObjects;
using Channels.Boss;
using Channels.Components;
using Channels.Type;
using Controller;
using Managers.Sound;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Boss1
{
    public class TerrapupaBattleController : BaseController
    {
        [Title("테라푸파 이벤트 관련 오브젝트 객체")] [SerializeField] [Required]
        private BossRoomDoor bossRoomDoor;

        [SerializeField] [Required] private BossRoomTrigger enterTrigger;
        [SerializeField] [Required] private BossRoomTrigger leftTrigger;
        [SerializeField] [Required] private Canvas bossRoomLeftCanvas;
        [SerializeField] [Required] private GameObject bossRoomEnterWall;

        [Title("보스전 설정")] [InfoBox("보스전 BGM")] public string bossBGM = "TerrapupaBGM";

        [InfoBox("보스전 종료 BGM")] public string endingBGM = "EndingBGM";

        private TicketMachine ticketMachine;

        private void Start()
        {
            bossRoomEnterWall.SetActive(false);
        }

        public override void InitController()
        {
            Debug.Log($"{name} InitController");

            InitTicketMachine();
        }

        private void InitTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.Dialog, ChannelType.BossBattle, ChannelType.BossDialog);
            ticketMachine.RegisterObserver(ChannelType.BossBattle, OnNotifyBossBattle);

            bossRoomDoor.InitTicketMachine(ticketMachine);
            enterTrigger.InitTicketMachine(ticketMachine);
            leftTrigger.InitTicketMachine(ticketMachine);
        }

        private void OnNotifyBossBattle(IBaseEventPayload payload)
        {
            if (payload is not TerrapupaBattlePayload bPayload)
            {
                return;
            }

            switch (bPayload.SituationType)
            {
                case TerrapupaSituationType.EnterBossRoom:
                    OnEnterBossRoom();
                    break;
                case TerrapupaSituationType.StartBattle:
                    OnStartBattle();
                    break;
                case TerrapupaSituationType.EndBattle:
                    OnEndBattle();
                    break;
                case TerrapupaSituationType.EndDialog:
                    OnEndDialog();
                    break;
                case TerrapupaSituationType.LeftBossRoom:
                    OnLeftBossRoom();
                    break;
                case TerrapupaSituationType.OpenLeftDoor:
                    OnOpenLeftDoor();
                    break;
            }
        }

        private void OnEnterBossRoom()
        {
            Debug.Log("OnEnterBossRoom()");
            // 인풋 설정 제거, 트리거 제거, 벽 추가, 로드상태 확인해서 넘어가기
            //InputManager.Instance.CanInput = false;

            enterTrigger.gameObject.SetActive(false);
            bossRoomEnterWall.SetActive(true);
        }

        private void OnStartBattle()
        {
            Debug.Log("OnStartBattle()");
            // 인풋 설정 제거 + 노래 전환
            //InputManager.Instance.CanInput = true;
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Bgm, bossBGM);
        }

        private void OnEndBattle()
        {
            Debug.Log("OnEndBattle()");
            // 전투 끝났을때
            //InputManager.Instance.CanInput = false;
        }

        private void OnEndDialog()
        {
            Debug.Log("OnEndDialog()");
            // 제한시간 코루틴 설정
            StartCoroutine(bossRoomDoor.OpenDoorTimeLimit());
            //InputManager.Instance.CanInput = true;
        }

        private void OnOpenLeftDoor()
        {
            Debug.Log("OnLeftBossRoom()");
            // 보스 문 열렸을 때
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Bgm, endingBGM);
        }

        private void OnLeftBossRoom()
        {
            Debug.Log("OnLeftBossRoom()");
            // 보스 방에서 나갔을 때 (트리거)
            bossRoomLeftCanvas.gameObject.SetActive(true);
        }

        [Button]
        private void GoToEnding()
        {
            OnLeftBossRoom();
        }
    }
}