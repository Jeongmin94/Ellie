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
    }

    public class UIPayload : IBaseEventPayload
    {
        public UIType uiType;
        public ActionType actionType;
        public Sprite sprite;
        public string name;
        public string text;
        public int count;
    }

    public class UIChannel : BaseEventChannel
    {
        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            Debug.Log($"I'm UIChannel");

            var uiPayload = payload as UIPayload;
            if (uiPayload.uiType == UIType.Notify)
                Publish(payload);
        }
    }
}