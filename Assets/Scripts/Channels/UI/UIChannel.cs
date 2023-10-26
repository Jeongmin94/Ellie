using UnityEngine;

namespace Channels.UI
{
    public enum UIType
    {
        BarImage
    }

    public class UIPayload : IBaseEventPayload
    {
    }

    public class UIChannel : BaseEventChannel
    {
        public override void ReceiveMessage<T>(T payload)
        {
            Debug.Log($"I'm UIChannel");
        }
    }
}