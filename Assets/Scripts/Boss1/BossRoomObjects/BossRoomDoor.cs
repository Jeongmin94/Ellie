﻿using System.Collections;
using Channels.Boss;
using Channels.Components;
using Channels.Type;
using Item.Stone;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Boss1.BossRoomObjects
{
    public class BossRoomDoor : MonoBehaviour
    {
        [SerializeField] [Required] private BossRoomDoorKnob leftDoor;
        [SerializeField] [Required] private BossRoomDoorKnob rightDoor;

        [SerializeField] private float doorTimeLimit = 10.0f;
        [SerializeField] private float openSpeedTime = 3.0f;
        [SerializeField] private float openAngle = 120.0f;
        [SerializeField] [ReadOnly] private int golemCoreCount;

        private TicketMachine ticketMachine;

        public void Start()
        {
            leftDoor.SubScribeAction(OnCheckGolemCore);
            rightDoor.SubScribeAction(OnCheckGolemCore);
        }

        public void InitTicketMachine(TicketMachine ticketMachine)
        {
            this.ticketMachine = ticketMachine;
        }

        public IEnumerator OpenDoorTimeLimit()
        {
            // 제한시간(60초)안에 못열면 다이얼로그 출력
            yield return new WaitForSeconds(doorTimeLimit);

            TerrapupaDialogChannel.SendMessage(TerrapupaDialogTriggerType.FailedToOpenDoor, ticketMachine);
            EmphasizedDoor();
        }

        private void OnCheckGolemCore(BossRoomDoorKnob knob, Transform stone)
        {
            var core = stone.GetComponent<GolemCoreStone>();
            if (core == null)
            {
                return;
            }

            golemCoreCount++;
            knob.Init(core.transform);

            if (golemCoreCount == 2)
            {
                OpenDoor();
            }
        }

        private void OpenDoor()
        {
            var payload = new TerrapupaBattlePayload { SituationType = TerrapupaSituationType.OpenLeftDoor };
            ticketMachine.SendMessage(ChannelType.BossBattle, payload);

            leftDoor.OpenDoor(-openAngle, openSpeedTime);
            rightDoor.OpenDoor(openAngle, openSpeedTime);
        }

        private void EmphasizedDoor()
        {
            leftDoor.EmphasizedDoor();
            rightDoor.EmphasizedDoor();
        }
    }
}