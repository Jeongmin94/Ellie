using System;

namespace Channels
{
    public enum BaseEventType
    {
        BaseEvent
    }

    public class BaseEventChannel<T> where T : IBaseEventPayload
    {
        private Action<T> onExecute;

        public virtual void Subscribe(Action<T> listener)
        {
            onExecute -= listener;
            onExecute += listener;
        }

        public virtual void Publish(T payload)
        {
            onExecute?.Invoke(payload);
        }
    }
}