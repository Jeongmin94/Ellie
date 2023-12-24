using UnityEngine;

namespace Channels.Item
{
    public enum ItemEventType
    {
        PickupItem,
        UseItem
    }

    public class ItemEventPayload : IBaseEventPayload
    {
        public Vector3 itemDropPosition;
        public int itemIndex = 4100;
        public ItemEventType type;
    }

    public class ItemChannel : BaseEventChannel
    {
        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            var itemPayload = payload as ItemEventPayload;

            Publish(itemPayload);
        }
    }
}