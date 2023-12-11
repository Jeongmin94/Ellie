using System.Security.Cryptography.X509Certificates;

namespace Channels.QuestCollider
{
    public enum QuestColliderNum
    {
        EldestCollider,
        SecondCollider,
        YoungestCollider
    }

    public class QuestColliderPayload : IBaseEventPayload
    {
        public QuestColliderNum Num;
    }
    public class QuestCollider : BaseEventChannel
    {
        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            if (payload is not QuestColliderPayload qPayload) return;
            
            Publish(qPayload);
        }
    }
}