using Assets.Scripts.Controller;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Type;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Puzzle
{
    public class BreakableStonePuzzleController : BaseController
    {
        [SerializeField] private List<BreakableStone> stones = new List<BreakableStone>();

        [Title("관련 정보")]
        [InfoBox("돌의 체력")] public int stoneHP = 100;
        [InfoBox("돌 떨림 시간")] public float stoneShakeTime = 0.5f;

        private TicketMachine ticketMachine;

        public override void InitController()
        {
            InitTicketMachine();
        }

        public void InitTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.Stone, ChannelType.Combat);

            foreach (var stone in stones)
            {
                stone.InitTicketMachine(ticketMachine);
            }
        }


    }
}