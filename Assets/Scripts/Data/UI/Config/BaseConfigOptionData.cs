using System.Collections.Generic;
using Assets.Scripts.ActionData;
using Assets.Scripts.UI.PopupMenu;
using UnityEngine;

namespace Data.UI.Config
{
    public abstract class BaseConfigOptionData<T> : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] public ConfigType configType;
        [SerializeField] public bool readOnly;
        [SerializeField] public string configName;
        [SerializeField] public List<T> values;
        [SerializeField] public int currentIdx;

        private readonly Data<T> actionValue = new Data<T>();
        public readonly Data<int> actionIndex = new Data<int>();

        public void OnIndexChanged(int value)
        {
            if (value < 0)
                value = 0;
            if (value >= values.Count)
                value = values.Count - 1;

            actionIndex.Value = value;
            actionValue.Value = values[actionIndex.Value];
        }

        public bool IsSameType(ConfigType type) => configType == type;
        public abstract string ValueString(T value);

        public virtual void OnBeforeSerialize()
        {
            currentIdx = actionIndex.Value;
        }

        public virtual void OnAfterDeserialize()
        {
            actionIndex.Value = currentIdx;
        }
    }
}