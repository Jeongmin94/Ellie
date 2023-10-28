using Assets.Scripts.InteractiveObjects;
using Microsoft.Cci;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        private GameObject interactiveObject = null;
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
            if (!canInteract || null == interactiveObject) return;
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