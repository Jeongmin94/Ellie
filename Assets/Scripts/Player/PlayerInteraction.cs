using Assets.Scripts.InteractiveObjects;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using Outline;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private GameObject outlineControllerPrefab;

        private TicketMachine ticketMachine;
        internal GameObject interactiveObject = null;
        public bool isInteracting = false;
        private bool canInteract = false;

        private OutlineController outlineController;
        public OutlineController OutlineController => outlineController;

        private void Start()
        {
            ticketMachine = GetComponent<PlayerController>().TicketMachine;

            var go = Instantiate(outlineControllerPrefab);
            outlineController = go.GetComponent<OutlineController>();

            DeactivateInteractiveUI();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Interactive")) return;
            interactiveObject = other.gameObject;
            canInteract = true;
            var io = interactiveObject.GetComponent<InteractiveObject>();
            if (io != null)
            {
                outlineController.AddOutline(io.GetRenderer(), OutlineType.InteractiveOutline);
            }

            ActivateInteractiveUI();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Interactive")) return;
            var io = other.gameObject.GetComponent<InteractiveObject>();
            if (io != null)
            {
                outlineController.RemoveMaterial(io.GetRenderer(), OutlineType.InteractiveOutline);
            }

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
            interactiveObject.GetComponent<InteractiveObject>().Interact(this.gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                Interact();
            }
        }

        public void SetCanInteract(bool b)
        {
            canInteract = b;
        }
    }
}