using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Channels.Boss
{
    public enum BossInteractionEvent
    {
        None,
        // 마나의 샘의 마법돌맹이 드롭 쿨타임 적용
        ApplyManaFountainReloadCooldown,
        // 마나의 샘 파괴 시 쿨타임 적용
        ApplyManaFountainDestroyCooldown,
        // 종마석 타격시 쿨타임 적용
        ApplyMagicStalactiteRespawnCooldown,
        // 마법 돌맹이 발사
        ShootMagicStone,
        // 보스가 마법돌맹이 범위 내에 들어옴
        BossAttractedByMagicStone,
        // 보스가 마법돌맹이 범위 내에 벗어남
        BossUnattractedByMagicStone,
        // 테라푸파의 마법돌맹이 섭취
        IntakeMagicStoneByTerrapupa,
    }

    public class BossInteractionPayload : IBaseEventPayload
    {
        // 이벤트 타입
        public BossInteractionEvent Type { get; set; }
        // 적용 쿨타임
        public float Cooldown { get; set; }

        // 상호작용하는 대상 (객체 -> 객체)
        public Transform Sender { get; set; }

        // 상호작용 받는 대상 (객체 <- 객체)
        public Transform Receiver { get; set; }

        // 상호작용 받는 대상 (객체들 <- 객체)
        public List<Transform> Receivers { get; set; }

        // 테라푸파의 공격 제한 타입
        public TerrapupaAttackType TerrapupaBannedAttackType { get; set; }
    }

    public class BossInteractionChannel : BaseEventChannel
    {
        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            BossInteractionPayload bossPayload = payload as BossInteractionPayload;

            Publish(bossPayload);
        }
    }
}