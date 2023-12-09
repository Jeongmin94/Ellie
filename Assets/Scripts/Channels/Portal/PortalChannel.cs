using Assets.Scripts.Item.Stone;
using Assets.Scripts.Managers;
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

        private LinkedList<Transform> portals = new LinkedList<Transform>();
        
        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            PortalEventPayload portalPayload = payload as PortalEventPayload;

            switch (portalPayload.Type)
            {
                case PortalEventType.ActivatePortal:
                    ActivatePortal(portalPayload);
                    break;
                case PortalEventType.DeactivatePortal:
                    DeactivatePortal(portalPayload);
                    break;
                case PortalEventType.UsePortal:
                    UsePortal(portalPayload);
                    break;
            }
        }

        private void ActivatePortal(PortalEventPayload payload)
        {
            portals.AddLast(payload.Portal);

            if(portals.Count > 2)
            {
                DeactivatePortal(new PortalEventPayload
                {
                    Portal = portals.First.Value,
                });
            }
        }

        private void DeactivatePortal(PortalEventPayload payload)
        {
            PoolManager.Instance.Push(payload.Portal.GetComponent<Poolable>());

            portals.RemoveFirst();
        }

        private void UsePortal(PortalEventPayload payload)
        {
            if(portals.Count < 2)
            {
                return;
            }

            var player = payload.Player;
            if(payload.Portal == portals.First.Value)
            {
                player.position = portals.Last.Value.position + new Vector3(0.0f, 2.0f, 0.0f);
            }
            else
            {
                player.position = portals.First.Value.position + new Vector3(0.0f, 2.0f, 0.0f);
            }

            foreach (var portal in portals)
            {
                var portalStone = portal.GetComponent<PortalStone>();

                portalStone.ApplyCooldown();
            }
        }
    }
}
