﻿using Channels.Components;
using Channels.Type;

namespace Channels.Boss
{
    public enum TerrapupaDialogTriggerType
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
        FailedToOpenDoor
    }

    public class TerrapupaDialogPaylaod : IBaseEventPayload
    {
        public TerrapupaDialogTriggerType TriggerType { get; set; } = TerrapupaDialogTriggerType.None;
    }

    public class TerrapupaDialogChannel : BaseEventChannel
    {
        public static void SendMessage(TerrapupaDialogTriggerType type, TicketMachine ticketMachine)
        {
            var dPayload = new TerrapupaDialogPaylaod { TriggerType = type };
            ticketMachine.SendMessage(ChannelType.BossDialog, dPayload);
        }

        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            if (payload is not TerrapupaDialogPaylaod bossDialogPayload)
            {
                return;
            }

            Publish(payload);
        }
    }
}