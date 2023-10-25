namespace Channels.UI
{
    public enum UIType
    {
    }

    public class UIPayload : IBaseEventPayload
    {
        public UIType Type { get; set; }
    }

    public class UIChannel<T> : BaseEventChannel<T> where T : IBaseEventPayload
    {
    }
}