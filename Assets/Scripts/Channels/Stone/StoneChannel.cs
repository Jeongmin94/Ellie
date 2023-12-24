using UnityEngine;

namespace Channels.Stone
{
    public enum StoneEventType
    {
        ShootStone,
        MineStone
    }

    public class StoneEventPayload : IBaseEventPayload
    {
        public StoneEventType Type { get; set; }

        public int StoneIdx { get; set; } = 4000;

        public Vector3 StoneSpawnPos { get; set; }
        public Vector3 StoneDirection { get; set; }
        public Vector3 StoneForce { get; set; }
        public float StoneStrength { get; set; }
    }

    public class StoneChannel : BaseEventChannel
    {
        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            var stonePayload = payload as StoneEventPayload;

            //Hatchery의 StoneEvent 함수를 호출합니다
            Publish(stonePayload);
        }
    }
}