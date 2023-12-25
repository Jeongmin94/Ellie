using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder
{
    [Serializable]
    public abstract class BlackboardKey : ISerializationCallbackReceiver
    {
        public string name;
        public Type underlyingType;
        public string typeName;

        public BlackboardKey(Type underlyingType)
        {
            this.underlyingType = underlyingType;
            typeName = this.underlyingType.FullName;
        }

        public void OnBeforeSerialize()
        {
            typeName = underlyingType.AssemblyQualifiedName;
        }

        public void OnAfterDeserialize()
        {
            underlyingType = Type.GetType(typeName);
        }

        public abstract void CopyFrom(BlackboardKey key);
        public abstract bool Equals(BlackboardKey key);
        public abstract void Subscribe(BlackboardKey key);

        public static BlackboardKey CreateKey(Type type)
        {
            return Activator.CreateInstance(type) as BlackboardKey;
        }
    }

    [Serializable]
    public abstract class BlackboardKey<T> : BlackboardKey
    {
        public T value;

        public Action<T> ValueChangeAction;

        public BlackboardKey() : base(typeof(T))
        {
        }

        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                ValueChangeAction?.Invoke(value);
            }
        }

        public override string ToString()
        {
            return $"{name} : {value}";
        }

        public override void CopyFrom(BlackboardKey key)
        {
            if (key.underlyingType == underlyingType)
            {
                var other = key as BlackboardKey<T>;
                Value = other.Value;
            }
        }

        public override bool Equals(BlackboardKey key)
        {
            if (key.underlyingType == underlyingType)
            {
                var other = key as BlackboardKey<T>;
                return value.Equals(other.value);
            }

            return false;
        }

        public override void Subscribe(BlackboardKey key)
        {
            if (key.underlyingType == underlyingType)
            {
                var other = key as BlackboardKey<T>;
                other.ValueChangeAction -= OnSetValue;
                other.ValueChangeAction += OnSetValue;
            }
        }

        public void OnSetValue(T newValue)
        {
            if (!EqualityComparer<T>.Default.Equals(value, newValue))
            {
                value = newValue;
                ValueChangeAction?.Invoke(newValue);
            }
        }
    }
}