using Assets.Scripts.Data.GoogleSheet;
using Channels.Components;
using Channels.Type;
using UnityEngine;

namespace Channels.Boss
{
    public enum BossDialogTriggerType
    {
        // 트리거 타입
        // https://docs.google.com/spreadsheets/d/1DlHsnJDvkPX63VSBQC0I_Bg3tRGKPfGMpX1gIgggWEQ/edit#gid=98615154
        None,

        EnterBossRoom = 8100,
        DestroyManaFountainFirstTime,
        DontAttackBossWeakPoint,
        GetMagicStoneFirstTime,
        AttackBossWithNormalStone,
        IntakeMagicStoneFirstTime,
        StartSecondPhase,
        DestroyAllManaFountains,
        StartThirdPhase,
        GetGolemCoreFirstTime,
        DieAllMinions,
        FailedToOpenDoor,
    }

    public class BossDialogPaylaod : IBaseEventPayload
    {
        public BossDialogTriggerType TriggerType { get; set; } = BossDialogTriggerType.None;
    }

    public class BossDialogChannel : BaseEventChannel
    {
        public static void SendMessageBossDialog(BossDialogTriggerType type, TicketMachine ticketMachine)
        {
            var dPayload = new BossDialogPaylaod { TriggerType = type };
            ticketMachine.SendMessage(ChannelType.BossDialog, dPayload);
        }

        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            if (payload is not BossDialogPaylaod bossDialogPayload)
                return;

            Publish(payload);
        }
    }
}