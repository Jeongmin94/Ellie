using System;

namespace Assets.Scripts.ActionData
{
    public class Data<T>
    {
        private T v;
        public T Value
        {
            get
            {
                return this.v;
            }
            set
            {
                this.v = value;
                this.OnChange?.Invoke(value);
            }
        }

        public Action<T> OnChange;
    }
}
