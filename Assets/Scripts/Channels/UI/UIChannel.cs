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
    }

    public class UIPayload : IBaseEventPayload
    {
        public UIType uiType;
        public ActionType actionType;
        public SlotAreaType slotAreaType;
        public ItemData itemData;

        public Transform onDragParent;
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