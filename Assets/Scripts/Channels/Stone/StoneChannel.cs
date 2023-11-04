using Channels;
using UnityEngine;

namespace Assets.Scripts.Channels.Item
{
    public enum StoneEventType
    {
        RequestStone,
        MineStone,
    }
    public class StoneEventPayload : IBaseEventPayload
    {
        public StoneEventType Type { get; set; }
        public Vector3 StoneSpawnPos { get; set; }
        public Vector3 StoneDirection { get; set; }
        public Vector3 StoneForce { get; set; }
        public float StoneStrength { get; set; }
    }
    public class StoneChannel : BaseEventChannel
    {
        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            StoneEventPayload itemPayload = payload as StoneEventPayload;
            Publish(itemPayload);
        }
    }
}
