using Assets.Scripts.Player;
using Channels.UI;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects.NPC
{
    public class SkullSecondBackPack : MonoBehaviour, IInteractiveObject
    {
        private Action getBackPackAction;

        InteractiveType interactiveType = InteractiveType.Acquisition;
        public InteractiveType GetInteractiveType()
        {
            return interactiveType;
        }

        public void Interact(GameObject obj)
        {
            Publish();
            obj.GetComponent<PlayerInteraction>().interactiveObject = null;
            obj.GetComponent<PlayerInteraction>().DeactivateInteractiveUI();
            gameObject.SetActive(false);
        }

        public void SubscribeGetBackPackAction(Action listener)
        {
            getBackPackAction -= listener;
            getBackPackAction += listener;
        }

        private void Publish()
        {
            getBackPackAction.Invoke();
        }
    }
}