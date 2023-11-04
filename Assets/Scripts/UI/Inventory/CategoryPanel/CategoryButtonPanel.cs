using System;
using System.Collections.Generic;
using System.Linq;
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

    public class CategoryButtonPanel : UIBase
    {
        private RectTransform rect;
        private ToggleGroup toggleGroup;
        private CategoryToggleController[] toggles;
        private readonly List<InventorySlotArea> itemSlots = new List<InventorySlotArea>();
        private readonly List<InventorySlotArea> equipSlots = new List<InventorySlotArea>();

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
            toggleGroup.SetAllTogglesOff();

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
                type = changeInfo.Type; // 현재 활성화된 슬롯 타입
                ActivateSlotArea(changeInfo.Type);
            }
        }

        private void DeactivateAllSlotAreas()
        {
            itemSlots.ForEach(area => area.gameObject.SetActive(false));
            equipSlots.ForEach(area => area.gameObject.SetActive(false));
        }

        private void ActivateSlotArea(GroupType groupType)
        {
            DeactivateAllSlotAreas();
            if (itemSlots.Any() && equipSlots.Any())
            {
                itemSlots[(int)groupType].gameObject.SetActive(true);
                equipSlots[(int)groupType].gameObject.SetActive(true);
            }
        }

        public void AddSlot(SlotAreaType slotAreaType, InventorySlotArea area)
        {
            switch (slotAreaType)
            {
                case SlotAreaType.Item:
                    itemSlots.Add(area);
                    area.gameObject.SetActive(false);
                    break;
                case SlotAreaType.Equipment:
                    equipSlots.Add(area);
                    area.gameObject.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(slotAreaType), slotAreaType, null);
            }
        }
    }
}