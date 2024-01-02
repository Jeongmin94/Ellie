using Channels.Boss;
using Channels.Components;
using Channels.Type;
using UnityEngine;

namespace Boss1.BossRoomObjects
{
    public class BossRoomTrigger : MonoBehaviour
    {
        public TerrapupaSituationType situationType;
        public TerrapupaDialogTriggerType dialogType;

        private TicketMachine ticketMachine;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var bPayload = new TerrapupaBattlePayload { SituationType = situationType };
                ticketMachine.SendMessage(ChannelType.BossBattle, bPayload);

                var dPayload = new TerrapupaDialogPaylaod { TriggerType = dialogType };
                ticketMachine.SendMessage(ChannelType.BossDialog, dPayload);
            }
        }

        public void InitTicketMachine(TicketMachine ticketMachine)
        {
            this.ticketMachine = ticketMachine;
        }
    }
}