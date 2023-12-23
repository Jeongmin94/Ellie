using Assets.Scripts.InteractiveObjects;
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
        public bool isInteracting;
        private bool canInteract;
        internal GameObject interactiveObject;

        private TicketMachine ticketMachine;
        public OutlineController OutlineController { get; private set; }

        private void Start()
        {
            ticketMachine = GetComponent<PlayerController>().TicketMachine;

            var go = Instantiate(outlineControllerPrefab);
            OutlineController = go.GetComponent<OutlineController>();

            DeactivateInteractiveUI();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                Interact();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Interactive"))
            {
                return;
            }

            interactiveObject = other.gameObject;
            canInteract = true;
            var io = interactiveObject.GetComponent<InteractiveObject>();
            if (io != null)
            {
                OutlineController.AddOutline(io.GetRenderer(), OutlineType.InteractiveOutline);
            }

            ActivateInteractiveUI();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Interactive"))
            {
                return;
            }

            var io = other.gameObject.GetComponent<InteractiveObject>();
            if (io != null)
            {
                OutlineController.RemoveMaterial(io.GetRenderer(), OutlineType.InteractiveOutline);
            }

            interactiveObject = null;
            canInteract = false;
            DeactivateInteractiveUI();
        }

        public void ActivateInteractiveUI()
        {
            if (interactiveObject == null)
            {
                return;
            }

            var payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.PopupInteractive;

            ticketMachine.SendMessage(ChannelType.UI, payload);
        }

        public void DeactivateInteractiveUI()
        {
            var payload = new UIPayload();
            payload.uiType = UIType.Notify;
            payload.actionType = ActionType.CloseInteractive;

            ticketMachine.SendMessage(ChannelType.UI, payload);
        }

        private void Interact()
        {
            if (GetComponent<PlayerController>().GetCurState() != PlayerStateName.Idle)
            {
                return;
            }

            if (!canInteract || isInteracting || null == interactiveObject)
            {
                return;
            }

            interactiveObject.GetComponent<InteractiveObject>().Interact(gameObject);
        }

        public void SetCanInteract(bool b)
        {
            canInteract = b;
        }
    }
}