using UnityEngine;

namespace Channels.Combat
{
    public enum CombatType
    {
        Test,
    }

    public class CombatPayload : IBaseEventPayload
    {
        public CombatType Type { get; set; }

        public float HP { get; set; }
    }

    public class CombatChannel : BaseEventChannel
    {
        public override void ReceiveMessage<T>(T payload)
        {
            CombatPayload load = payload as CombatPayload;
            Debug.Log($"I'm CombatChannel {load.Type}");
            Debug.Log($"HP {load.HP}");

        }
    }
}