using Channels;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Channels
{
    public enum PortalEventType
    {
        ActivatePortal,
        DeactivatePortal,
        UsePortal,
    }
    public class PortalEventPayload : IBaseEventPayload
    {
        public PortalEventType Type { get; set; }
        public Transform Portal { get; set; }
        public Transform Player { get; set; }
    }
    public class PortalChannel : BaseEventChannel
    {
        private int portalNumber = 0;

        private Queue<Transform> portals = new Queue<Transform>();
        
        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            PortalEventPayload portalPayload = payload as PortalEventPayload;

            switch (portalPayload.Type)
            {
                case PortalEventType.ActivatePortal:
                    break;
                case PortalEventType.DeactivatePortal:
                    break;
                case PortalEventType.UsePortal:
                    break;
            }

            //Hatchery의 StoneEvent 함수를 호출합니다
            Publish(portalPayload);
        }

        private void ActivatePortal()
        {
            if(portals.Count > 2)
            {

            }
        }

        private void DeactivatePortal()
        {

        }
    }
}
