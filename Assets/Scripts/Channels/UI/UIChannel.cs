using System;
using UnityEngine;

namespace Channels.UI
{
    public enum UIType
    {
        BarImage,
        SlotItem,
        ChannelAction
    }

    public class UIPayload : IBaseEventPayload
    {
        public UIType uiType;
    }

    public class UIChannel : BaseEventChannel
    {
        public Action uiChannelAction;

        public override void ReceiveMessage<T>(T payload)
        {
            Debug.Log($"I'm UIChannel");

            var uiPayload = payload as UIPayload;
            if (uiPayload.uiType == UIType.ChannelAction)
                uiChannelAction?.Invoke();
        }
    }
}