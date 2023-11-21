using Assets.Scripts.InteractiveObjects;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        internal GameObject interactiveObject = null;
        public bool isInteracting = false;
        private bool canInteract = false;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Interactive")) return;
            interactiveObject = other.gameObject;
            canInteract = true;
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Interactive")) return;
            interactiveObject = null;
            canInteract = false;
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
    }
}