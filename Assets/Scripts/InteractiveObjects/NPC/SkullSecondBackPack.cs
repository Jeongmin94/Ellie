using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects.NPC
{
    public class SkullSecondBackPack : MonoBehaviour, IInteractiveObject
    {
        private Action getBackPackAction;

        public void Interact(GameObject obj)
        {
            Publish();
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