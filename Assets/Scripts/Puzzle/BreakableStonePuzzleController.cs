using System.Collections.Generic;
using Channels.Components;
using Channels.Type;
using Controller;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Assets.Scripts.Puzzle
{
    public class BreakableStonePuzzleController : BaseController
    {
        [SerializeField] private List<BreakableStone> stones = new();

        [Title("관련 정보")] [InfoBox("부서지는 이펙트")] public GameObject destroyEffect;

        [InfoBox("돌의 체력")] public int stoneHP = 10;

        [ShowInInspector] private TicketMachine ticketMachine;

        private void Start()
        {
            InitStones();
        }

        private void InitStones()
        {
            foreach (var stone in stones)
            {
                stone.destroyEffect = destroyEffect;
                stone.currentHP = stoneHP;
            }
        }

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