using UnityEngine;

namespace Channels
{
    public class BaseEventChannel
    {
        public virtual void ReceiveMessage<T>(T payload) where T : IBaseEventPayload
        {
            Debug.Log($"BaseEventChannel ReceiveMessage() - {payload}");
        }
    }
}