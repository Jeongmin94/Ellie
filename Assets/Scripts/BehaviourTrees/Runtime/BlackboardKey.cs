using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {

    [System.Serializable]
    public abstract class BlackboardKey : ISerializationCallbackReceiver{

        public string name;
        public System.Type underlyingType;
        public string typeName;

        public BlackboardKey(System.Type underlyingType) {
            this.underlyingType = underlyingType;
            typeName = this.underlyingType.FullName;
        }

        public void OnBeforeSerialize() {
            typeName = underlyingType.AssemblyQualifiedName;
        }

        public void OnAfterDeserialize() {
            underlyingType = System.Type.GetType(typeName);
        }

        public abstract void CopyFrom(BlackboardKey key);
        public abstract bool Equals(BlackboardKey key);
        public abstract void Subscribe(BlackboardKey key);

        public static BlackboardKey CreateKey(System.Type type) {
            return System.Activator.CreateInstance(type) as BlackboardKey;
        }

    }

    [System.Serializable]
    public abstract class BlackboardKey<T> : BlackboardKey {

        public T value;

        public Action<T> ValueChangeAction;

        public T Value
        {
            get 
            {
                return value;
            }
            set 
            { 
                this.value = value;
                this.ValueChangeAction?.Invoke(value);
            }
        }

        public BlackboardKey() : base(typeof(T)) {
        }

        public override string ToString() {
            return $"{name} : {value}";
        }

        public override void CopyFrom(BlackboardKey key) {
            if (key.underlyingType == underlyingType) {
                BlackboardKey<T> other = key as BlackboardKey<T>;
                this.Value = other.Value;
            }
        }

        public override bool Equals(BlackboardKey key) {
            if (key.underlyingType == underlyingType) {
                BlackboardKey<T> other = key as BlackboardKey<T>;
                return this.value.Equals(other.value);
            }
            return false;
        }

        public override void Subscribe(BlackboardKey key)
        {
            if (key.underlyingType == underlyingType)
            {
                BlackboardKey<T> other = key as BlackboardKey<T>;
                other.ValueChangeAction -= OnSetValue;
                other.ValueChangeAction += OnSetValue;
            }
        }
        public void OnSetValue(T newValue)
        {
            if (!EqualityComparer<T>.Default.Equals(value, newValue))
            {
                this.value = newValue;
                this.ValueChangeAction?.Invoke(newValue);
            }
        }
    }
}