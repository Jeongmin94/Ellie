using System;

namespace Assets.Scripts.ActionData
{
    public class Data<T>
    {
        private T v;

        private Action<T> valueChangeAction;

        public T Value
        {
            get => v;
            set
            {
                v = value;
                valueChangeAction?.Invoke(value);
            }
        }

        public void ClearAction()
        {
            valueChangeAction = null;
        }

        public void Subscribe(Action<T> listener)
        {
            valueChangeAction -= listener;
            valueChangeAction += listener;
        }

        public void Unsubscribe(Action<T> listener)
        {
            valueChangeAction -= listener;
        }
    }
}