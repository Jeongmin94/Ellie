using Channels.Components;
using Channels.Type;
using UnityEngine;

namespace Channels.Boss
{
    public enum BossSituationType
    {
        None,
        EnterBossRoom,
        StartBattle,
        StartSeconPhase,
        EndBattle,
        EndDialog,
        LeftBossRoom,
        OpenLeftDoor,
        StartThirdPhase,
        EmphasizedDoor,
    }

    public class BossBattlePayload : IBaseEventPayload
    {
        public BossSituationType SituationType { get; set; }
        public Transform Sender { get; set; }
    }

    public class BossBattleChannel : BaseEventChannel
    {
        public static void SendMessageBossBattle(BossSituationType type, TicketMachine ticketMachine)
        {
            var bPayload = new BossBattlePayload { SituationType = type };
            ticketMachine.SendMessage(ChannelType.BossBattle, bPayload);
        }

        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            if (payload is not BossBattlePayload bossDialogPayload)
                return;

            Publish(payload);
        }
    }
}