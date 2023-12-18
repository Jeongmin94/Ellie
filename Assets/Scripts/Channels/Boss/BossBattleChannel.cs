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
    }

    public class BossBattlePayload : IBaseEventPayload
    {
        public BossSituationType SituationType { get; set; }
        public Transform Sender { get; set; }
    }

    public class BossBattleChannel : BaseEventChannel
    {
        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            if (payload is not BossBattlePayload bossDialogPayload)
                return;

            Publish(payload);
        }
    }
}