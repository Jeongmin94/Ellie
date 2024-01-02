using Channels.Components;
using Channels.Type;
using UnityEngine;

namespace Channels.Boss
{
    public enum TerrapupaSituationType
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
        
        HitManaByPlayerStone,
        DestroyedManaByBoss1,
        DropMagicStalactite,
        ActivateMagicStone,
    }

    public class TerrapupaBattlePayload : IBaseEventPayload
    {
        public TerrapupaSituationType SituationType { get; set; }
        public Transform Sender { get; set; }
    }

    public class TerrapupaBattleChannel : BaseEventChannel
    {
        public static void SendMessage(TerrapupaSituationType type, TicketMachine ticketMachine)
        {
            var bPayload = new TerrapupaBattlePayload { SituationType = type };
            ticketMachine.SendMessage(ChannelType.BossBattle, bPayload);
        }

        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            if (payload is not TerrapupaBattlePayload bossDialogPayload)
            {
                return;
            }

            Publish(payload);
        }
    }
}
