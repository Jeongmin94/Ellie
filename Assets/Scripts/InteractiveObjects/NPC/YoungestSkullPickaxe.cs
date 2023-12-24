using System;
using Channels.UI;
using Outline;
using Player;
using UnityEngine;

namespace InteractiveObjects.NPC
{
    public class YoungestSkullPickaxe : InteractiveObject
    {
        [SerializeField] private Renderer renderer;

        public int PickaxeIndex = 9000;

        public InteractiveType interactiveType = InteractiveType.Acquisition;

        private Action getPickaxeAction;

        public override InteractiveType GetInteractiveType()
        {
            return interactiveType;
        }

        public override OutlineType GetOutlineType()
        {
            return OutlineType.InteractiveOutline;
        }

        public override Renderer GetRenderer()
        {
            return renderer;
        }

        public override void Interact(GameObject obj)
        {
            if (!obj.CompareTag("Player"))
            {
                return;
            }

            var player = obj.GetComponent<PlayerQuest>();
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