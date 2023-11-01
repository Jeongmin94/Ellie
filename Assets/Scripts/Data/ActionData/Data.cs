using System;

namespace Assets.Scripts.ActionData
{
    public class Data<T>
    {
        private T v;

        public T Value
        {
            get { return this.v; }
            set
            {
                this.v = value;
                this.valueChangeAction?.Invoke(value);
            }
        }

        private Action<T> valueChangeAction;

        public void ClearAction() => valueChangeAction = null;

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