using System;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.PopupMenu;
using UnityEngine;

namespace Data.UI.Config
{
    public enum DataType
    {
        Int,
        Boolean,
        String,
        Float,
        Vector2
    }
    
    public abstract class BaseConfigOptionData<T> : ScriptableObject
    {
        [SerializeField] public ConfigType configType;
        [SerializeField] public bool readOnly;
        [SerializeField] public string configName;
        [SerializeField] public List<T> values;
        [SerializeField] public int currentIdx;
        [SerializeField] public string optionChangeSoundName;

        protected Action<string> valueChangeAction;

        // !TODO: 씬이 전환될 때마다 ClearAction을 호출해야 함
        public void ClearAction() => valueChangeAction = null;

        public void InitData()
        {
            valueChangeAction?.Invoke(ValueString(values[currentIdx]));
        }

        // !TODO: 옵션 설정값이 변경되면 설정값 변경에 대한 로직을 처리하는 클래스에서 구독해서 사용해야 함
        public void SubscribeValueChangeAction(Action<string> listener)
        {
            valueChangeAction -= listener;
            valueChangeAction += listener;
        }

        public virtual void OnIndexChanged(int value)
        {
            int idx = currentIdx + value;
            if (idx < 0)
                idx = 0;
            if (idx >= values.Count)
                idx = values.Count - 1;

            currentIdx = idx;
            if (!readOnly)
            {
                SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, optionChangeSoundName, Vector3.zero);
            }

            valueChangeAction?.Invoke(ValueString(values[currentIdx]));
        }

        public bool IsSameType(ConfigType type) => configType == type;
        protected abstract string ValueString(T value);
        public abstract DataType GetDataType();
    }
}