using Channels.Components;
using Channels.Type;
using Controller;
using UnityEngine;
using Utils;

namespace Boss1
{
    public class TerrapupaCombatController : BaseController
    {
        private TicketMachine ticketMachine;

        private void Start()
        {
            
        }
        
        public override void InitController()
        {
            Debug.Log($"{name} InitController");

            SubscribeEvents();
            InitTicketMachine();
        }
        
        private void SubscribeEvents()
        {
            
        }

        private void InitTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets
                (ChannelType.Stone, ChannelType.Combat, ChannelType.Camera, 
                    ChannelType.BossBattle, ChannelType.BossDialog);
        }
    }
}
