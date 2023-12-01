using Assets.Scripts.InteractiveObjects;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using TheKiwiCoder;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        private TicketMachine ticketMachine;
        internal GameObject interactiveObject = null;
        public bool isInteracting = false;
        private bool canInteract = false;


        private void Start()
        {
            ticketMachine = GetComponent<PlayerController>().TicketMachine;
            DeactivateInteractiveUI();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Interactive")) return;
            interactiveObject = other.gameObject;
            canInteract = true;
            ActivateInteractiveUI();
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Interactive")) return;
            interactiveObject = null;
            canInteract = false;
            DeactivateInteractiveUI();
        }

        public void ActivateInteractiveUI()
        {
            if (interactiveObject == null) return;
            UIPayload payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.PopupInteractive;
            payload.interactiveType = interactiveObject.GetComponent<IInteractiveObject>().GetInteractiveType();

            ticketMachine.SendMessage(ChannelType.UI, payload);
        }

        public void DeactivateInteractiveUI()
        {
            UIPayload payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.CloseInteractive;

            ticketMachine.SendMessage(ChannelType.UI, payload);
        }
        private void Interact()
        {
            if (GetComponent<PlayerController>().GetCurState() != PlayerStateName.Idle) return;
            if (!canInteract || isInteracting || null == interactiveObject) return;
            interactiveObject.GetComponent<IInteractiveObject>().Interact(this.gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                Interact();
            }
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 50, 200, 20), "Can Interact : " + canInteract);
            string name = "";
            if (null != interactiveObject)
                name = interactiveObject.name;
            else
                name = "null";
            GUI.Label(new Rect(10, 60, 200, 20), "Current Interactive Obj : " + name);
        }

        public void SetCanInteract(bool b)
        {
            canInteract = b;
        }
    }
}