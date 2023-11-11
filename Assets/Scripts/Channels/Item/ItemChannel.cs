using System.Collections;
using System.Collections.Generic;
using Channels;
using UnityEngine;

namespace Assets.Scripts.Channels.Item
{
    public enum ItemEventType
    {
        PickupItem,
        UseItem,
    }

    public class ItemEventPayload : IBaseEventPayload
    {
        public ItemEventType type;
        public int itemIndex = 4100;
        public Vector3 itemDropPosition;
    }
    public class ItemChannel : BaseEventChannel
    {
        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            ItemEventPayload itemPayload = payload as ItemEventPayload;

            Publish(itemPayload);
        }
    }

}