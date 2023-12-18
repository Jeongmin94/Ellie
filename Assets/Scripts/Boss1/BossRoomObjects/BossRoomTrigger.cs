using Channels.Boss;
using Channels.Components;
using Channels.Type;
using System.Collections;
using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    public BossSituationType situationType;
    public BossDialogTriggerType dialogType;

    private TicketMachine ticketMachine;

    public void InitTicketMachine(TicketMachine ticketMachine)
    {
        this.ticketMachine = ticketMachine;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var bPayload = new BossBattlePayload { SituationType = situationType };
            ticketMachine.SendMessage(ChannelType.BossBattle, bPayload);

            var dPayload = new BossDialogPaylaod { TriggerType = dialogType };
            ticketMachine.SendMessage(ChannelType.BossDialog, dPayload);
        }
    }
}
