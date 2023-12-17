using UnityEngine;

namespace Channels.Boss
{
    public enum BossDialogTriggerType
    {
        // 트리거 타입
        // https://docs.google.com/spreadsheets/d/1DlHsnJDvkPX63VSBQC0I_Bg3tRGKPfGMpX1gIgggWEQ/edit#gid=98615154

        EnterBossRoom = 8100,
        DestroyManaFountainFirstTime,
        AttackBossWithNormalStone,
        GetMagicStoneFirstTime,
        DontAttackBossWeakPoint,
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
        public BossDialogTriggerType TriggerType { get; set; }
    }

    public class BossDialogChannel : BaseEventChannel
    {
        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            if (payload is not BossDialogPaylaod bossDialogPayload)
                return;

            Publish(payload);
        }
    }
}