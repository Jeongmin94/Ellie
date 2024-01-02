using Channels.Combat;
using UnityEngine;

namespace Channels.Boss
{
    public enum TerrapupaAttackType
    {
        None,
        ThrowStone,
        EarthQuake,
        Roll,
        LowAttack
    }

    public enum TerrapupaEvent
    {
        None,

        GripStoneByBoss1,
        ThrowStoneByBoss1,
        HitManaByPlayerStone,
        DestroyedManaByBoss1,
        OccurEarthQuake,
        DropMagicStalactite,
        BossAttractedByMagicStone,
        BossUnattractedByMagicStone,
        IntakeMagicStoneByBoss1,
        BossDeath,
        HitStone,
        BossMeleeAttack,
        BossLowAttack,
        ApplyBossCooldown,
        BossMinionAttack,
        DestroyAllManaFountain,
        ApplySingleBossCooldown,
        StartIntakeMagicStone,
        ActivateMagicStone
    }

    public class TerrapupaPayload : IBaseEventPayload
    {
        // 이벤트 타입
        public TerrapupaEvent Type { get; set; }

        // 공격력
        public int AttackValue { get; set; }

        // 공격 쿨타임 적용 시
        public float Cooldown { get; set; }

        // 상호작용하는 대상 (객체 -> 객체)
        public Transform Sender { get; set; }

        // 상호작용 받는 대상 (객체 <- 객체)
        public Transform Receiver { get; set; }
    }

    public class BossEventPayload : IBaseEventPayload
    {
        public GameObject PrefabValue { get; set; }
        public int IntValue { get; set; }
        public bool BoolValue { get; set; }
        public float FloatValue { get; set; }
        public Vector3 Vector3Value { get; set; }
        public Transform TransformValue1 { get; set; }
        public Transform TransformValue2 { get; set; }
        public Transform TransformValue3 { get; set; }
        public TerrapupaAttackType AttackTypeValue { get; set; }
        public Transform Sender { get; set; }
        public CombatPayload CombatPayload { get; set; }
    }

    public class BossChannel : BaseEventChannel
    {
        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            var terrapupaPayload = payload as TerrapupaPayload;

            Publish(terrapupaPayload);
        }
    }
}
