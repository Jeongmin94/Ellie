using System;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Inventory
{
    public enum GroupType
    {
        Consumption,
        Stone,
        Etc
    }

    public readonly struct ToggleChangeInfo
    {
        public GroupType Type { get; }
        public bool IsOn { get; }

        private ToggleChangeInfo(GroupType type, bool isOn)
        {
            Type = type;
            IsOn = isOn;
        }

        public static ToggleChangeInfo Of(GroupType type, bool isOn)
        {
            return new ToggleChangeInfo(type, isOn);
        }
    }

    public delegate void ToggleChangeHandler(ToggleChangeInfo changeInfo);

    // !TODO: 버튼 패널에 있는 버튼을 누를 때마다 카테고리 슬롯 항목들이 변경되어야 함
    public class CategoryButtonPanel : UIBase
    {
        private RectTransform rect;
        private ToggleGroup toggleGroup;
        private CategoryToggleController[] toggles;

        private GroupType type = GroupType.Consumption;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            Bind();
            InitObjects();
        }

        private void Bind()
        {
            rect = GetComponent<RectTransform>();
            toggleGroup = GetComponent<ToggleGroup>();
        }

        private void InitObjects()
        {
            var groupTypes = Enum.GetValues(typeof(GroupType));
            toggles = new CategoryToggleController[groupTypes.Length];
            for (int i = 0; i < groupTypes.Length; i++)
            {
                var child = rect.GetChild(i);
                if (child == null)
                    return;

                var toggle = rect.GetChild(i).gameObject.GetOrAddComponent<CategoryToggleController>();
                toggles[i] = toggle;
                toggles[i].Init((GroupType)groupTypes.GetValue(i));
                toggles[i].Subscribe(ToggleChangeCallback);
            }

            type = GroupType.Consumption;
            toggles[(int)type].IsOn = true;
        }

        private void ToggleChangeCallback(ToggleChangeInfo changeInfo)
        {
            if (changeInfo.IsOn)
            {
                Debug.Log($"{changeInfo.Type} 슬롯 활성화됨");
            }
        }
    }
}