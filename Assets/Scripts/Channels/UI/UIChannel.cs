namespace Channels.UI
{
    public enum UIType
    {
        BarImage
    }

    public class UIPayload : IBaseEventPayload
    {
        public UIType Type { get; set; }
    }

    public class UIChannel : BaseEventChannel
    {
    }
}