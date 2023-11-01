using System;
using UnityEngine;

namespace Channels
{
    public class BaseEventChannel
    {
        protected Action<IBaseEventPayload> notifyAction;
        public virtual void ReceiveMessage<T>(T payload) where T : IBaseEventPayload
        {
            Debug.Log($"BaseEventChannel ReceiveMessage() - {payload}");
        }

        public void SubscribeNotifyAction(Action<IBaseEventPayload> observer)
        {
            notifyAction -= observer;
            notifyAction += observer;
        }
    }
}