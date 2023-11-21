using Assets.Scripts.Item;
using Assets.Scripts.UI.Inventory;
using UnityEngine;

namespace Channels.UI
{
    public enum UIType
    {
        BarImage,
        SlotItem,
        Notify
    }

    public enum ActionType
    {
        AddSlotItem,
        ConsumeSlotItem,
        ToggleInventory,
        MoveClockwise,
        MoveCounterClockwise,
        SetPlayerProperty,
        ClickCloseButton
    }

    public class UIPayload : IBaseEventPayload
    {
        public UIType uiType;
        public ActionType actionType;
        public SlotAreaType slotAreaType;
        public GroupType groupType;
        //ItemMetaData는 UI에 출력할 데이터들만 포함합니다
        public ItemMetaData itemData;
        public Transform onDragParent;
        public bool isStoneNull;
    }

    public class UIChannel : BaseEventChannel
    {
        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            if (payload is not UIPayload uiPayload)
                return;

            if (uiPayload.uiType == UIType.Notify)
            {
                Publish(payload);
                return;
            }

            // do something
        }
    }
}