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
        RemoveSlotItem,
        ToggleInventory,
    }

    public class UIPayload : IBaseEventPayload
    {
        public UIType uiType;
        public ActionType actionType;
        public ItemData itemData;
        
        public Sprite sprite;
        public string name;
        public string text;
        public int count;
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