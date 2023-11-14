using System;
using System.Collections.Generic;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.Utils;
using Channels.UI;
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
        private GroupType type = GroupType.Consumption;
        private ActivateButtonPanelHandler activateButtonPanelHandler;

        private readonly IDictionary<SlotAreaType, List<InventorySlotArea>> slotAreas = new Dictionary<SlotAreaType, List<InventorySlotArea>>();

        private Action<InventoryEventPayload> panelInventoryAction;

        public void InitCategoryButtonPanel()
        {
            Init();
        }

        public InventorySlotArea GetSlotArea(SlotAreaType slotAreaType, GroupType groupType)
        {
            return slotAreas[slotAreaType][(int)groupType];
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

            var slotTypes = Enum.GetValues(typeof(SlotAreaType));
            for (int i = 0; i < slotTypes.Length; i++)
            {
                slotAreas.TryAdd((SlotAreaType)slotTypes.GetValue(i), new List<InventorySlotArea>());
            }
        }

        private void ToggleChangeCallback(ToggleChangeInfo changeInfo)
        {
            if (changeInfo.IsOn)
            {
                type = changeInfo.Type; // 현재 활성화된 슬롯 타입
            }

            activateButtonPanelHandler?.Invoke(changeInfo);
        }

        public void MoveSlotArea(SlotAreaType areaType, GroupType groupType, Transform target, Transform parent, Rect size)
        {
            if (slotAreas.TryGetValue(areaType, out var slots))
            {
                slots[(int)groupType].MoveSlotArea(target, parent, size);
            }
        }

        public void AddSlotArea(SlotAreaType slotAreaType, InventorySlotArea area)
        {
            if (slotAreas.TryGetValue(slotAreaType, out var slots))
            {
                area.Subscribe(OnSlotAreaInventoryAction);
                slots.Add(area);
            }
        }

        public void Subscribe(ActivateButtonPanelHandler listener)
        {
            activateButtonPanelHandler -= listener;
            activateButtonPanelHandler += listener;
        }

        public void ActivateToggle(GroupType groupType, bool isOn)
        {
            toggles[(int)groupType].ActivateToggle(isOn);
        }

        public void Subscribe(Action<InventoryEventPayload> listener)
        {
            panelInventoryAction -= listener;
            panelInventoryAction += listener;
        }

        private void OnDestroy()
        {
            activateButtonPanelHandler = null;
            panelInventoryAction = null;
        }

        #region InventoryEvent

        private void OnSlotAreaInventoryAction(InventoryEventPayload payload)
        {
            if (payload.eventType != InventoryEventType.EquipItem &&
                payload.eventType != InventoryEventType.UnEquipItem &&
                payload.eventType != InventoryEventType.UpdateEquipItem)
                payload.groupType = type;

            if (payload.eventType == InventoryEventType.CopyItemWithShortCut)
            {
                var groupType = payload.baseSlotItem.SlotItemData.itemData.groupType;
                if (slotAreas.TryGetValue(SlotAreaType.Equipment, out var area))
                {
                    InventorySlot dup = area[(int)groupType].FindSlot(payload.baseSlotItem.SlotItemData.ItemIndex);
                    if (dup != null)
                    {
                        Debug.Log($"이미 등록되어 있는 아이템");
                        return;
                    }

                    InventorySlot emptySlot = area[(int)groupType].FindEmptySlot();
                    if (emptySlot == null)
                    {
                        Debug.Log($"비어있는 슬롯이 없음");
                        return;
                    }

                    payload.slot = emptySlot;
                }
            }
            else if (payload.eventType == InventoryEventType.SortSlotArea)
            {
                var types = Enum.GetValues(typeof(SlotAreaType));
                for (int i = 0; i < types.Length; i++)
                {
                    SlotAreaType t = (SlotAreaType)types.GetValue(i);

                    if (t == SlotAreaType.Description)
                    {
                        continue;
                    }

                    if (slotAreas.TryGetValue(t, out var areas))
                    {
                        areas[(int)payload.groupType].Sort(SortType.OnLeft);
                    }
                }

                return;
            }
            else if (payload.eventType == InventoryEventType.EquipItem)
            {
                if (slotAreas.TryGetValue(SlotAreaType.Equipment, out var area))
                {
                    InventorySlot emptySlot = area[(int)payload.groupType].FindEmptySlot();
                    if (emptySlot == null)
                        return;
                    payload.slot = emptySlot;
                }
            }
            else if (payload.eventType == InventoryEventType.UpdateEquipItem)
            {
                if (slotAreas.TryGetValue(SlotAreaType.Equipment, out var area))
                {
                    var slot = area[(int)payload.groupType].FindSlot(payload.baseItem.ItemIndex);
                    if (slot == null)
                        return;
                    payload.slot = slot;
                }
            }
            else if(payload.eventType == InventoryEventType.SendMessageToPlayer)
            {

            }

            panelInventoryAction?.Invoke(payload);
        }

        public void AddItem(SlotAreaType slotAreaType, GroupType groupType, UIPayload payload)
        {
            if (slotAreas.TryGetValue(slotAreaType, out var area))
            {
                area[(int)groupType].AddItem(payload);
            }
        }

        public void ConsumeItem(SlotAreaType slotAreaType, GroupType groupType, UIPayload payload)
        {
            if (slotAreas.TryGetValue(slotAreaType, out var area))
            {
                area[(int)groupType].ConsumeItem(payload);
            }
        }

        // !TODO 회전하는 슬롯에 대한 정의 필요
        // 현재는 인벤토리에서 오픈된 type에 대해서 이동함
        public void MoveItem(SlotAreaType slotAreaType, UIPayload payload)
        {
            if (type == GroupType.Etc)
                return;

            if (slotAreas.TryGetValue(slotAreaType, out var area))
            {
                area[(int)type].MoveItem(payload);
            }
        }

        #endregion
    }
}