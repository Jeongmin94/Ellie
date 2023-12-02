using Assets.Scripts.Player;
using Channels.UI;
using System;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects.NPC
{
    public class YoungestSkullPickaxe : MonoBehaviour, IInteractiveObject
    {
        public int PickaxeIndex = 9000;

        private Action getPickaxeAction;

        public InteractiveType interactiveType = InteractiveType.Acquisition;

        public InteractiveType GetInteractiveType()
        {
            return interactiveType;
        }
        public void Interact(GameObject obj)
        {
            if (!obj.CompareTag("Player")) return;

            PlayerQuest player = obj.GetComponent<PlayerQuest>();
            player.GetPickaxe(PickaxeIndex);

            Publish();
            obj.GetComponent<PlayerInteraction>().interactiveObject = null;
            obj.GetComponent<PlayerInteraction>().DeactivateInteractiveUI();
            gameObject.SetActive(false);
        }

        public void SubscribeGetPickaxeAction(Action listener)
        {
            getPickaxeAction -= listener;
            getPickaxeAction += listener;
        }

        private void Publish()
        {
            getPickaxeAction.Invoke();
        }
    }
}
