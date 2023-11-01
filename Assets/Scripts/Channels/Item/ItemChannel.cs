using Channels;
using UnityEngine;

namespace Assets.Scripts.Channels.Item
{
    public enum ItemType
    {
        RequestStone,
    }
    public class ItemPayload : IBaseEventPayload
    {
        public ItemType Type { get; set; }
        public Vector3 StoneSpawnPos { get; set; }
        public Vector3 StoneDirection { get; set; }
        public float StoneStrength { get; set; }
    }
    public class ItemChannel : BaseEventChannel
    {
        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            ItemPayload itemPayload = payload as ItemPayload;
            if (itemPayload.Type == ItemType.RequestStone)
            {
                Publish(itemPayload);
                return;
            }
        }
    }
}
