using System;
using UnityEngine;

namespace TheKiwiCoder
{
    [Serializable]
    public class NodeProperty
    {
        [SerializeReference] public BlackboardKey reference;
    }

    [Serializable]
    public class NodeProperty<T> : NodeProperty
    {
        public T defaultValue;
        private BlackboardKey<T> _typedKey;

        private BlackboardKey<T> typedKey
        {
            get
            {
                if (_typedKey == null && reference != null)
                {
                    _typedKey = reference as BlackboardKey<T>;
                }

                return _typedKey;
            }
        }

        public T Value
        {
            set
            {
                if (typedKey != null)
                {
                    typedKey.Value = value;
                }
                else
                {
                    defaultValue = value;
                }
            }
            get
            {
                if (typedKey != null)
                {
                    return typedKey.Value;
                }

                return defaultValue;
            }
        }
    }
}