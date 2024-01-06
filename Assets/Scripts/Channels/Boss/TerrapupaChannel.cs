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

    public class TerrapupaChannel : BaseEventChannel
    {
        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            var terrapupaPayload = payload as BossEventPayload;

            Publish(terrapupaPayload);
        }
    }
}
