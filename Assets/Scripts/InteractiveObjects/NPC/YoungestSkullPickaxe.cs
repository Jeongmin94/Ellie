using Assets.Scripts.Player;
using System;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects.NPC
{
    public class YoungestSkullPickaxe : MonoBehaviour, IInteractiveObject
    {
        public int PickaxeIndex = 9000;

        private Action getPickaxeAction;

        public void Interact(GameObject obj)
        {
            if (!obj.CompareTag("Player")) return;

            PlayerQuest player = obj.GetComponent<PlayerQuest>();
            player.GetPickaxe(PickaxeIndex);

            Publish();
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
