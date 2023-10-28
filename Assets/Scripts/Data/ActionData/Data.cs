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
                this.ValueChangeAction?.Invoke(value);
            }
        }

        public Action<T> ValueChangeAction;

        public void Subscribe(Action<T> listener)
        {
            ValueChangeAction -= listener;
            ValueChangeAction += listener;
        }
    }
}