namespace Channels.Combat
{
    public enum CombatType
    {
    }

    public class CombatPayload : IBaseEventPayload
    {
        public CombatType Type { get; set; }
    }

    public class CombatChannel : BaseEventChannel
    {
    }
}