using Assets.Scripts.Player;
using Channels.UI;
using System;
using Outline;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects.NPC
{
    public class YoungestSkullPickaxe : InteractiveObject
    {
        [SerializeField] private Renderer renderer;
        
        public int PickaxeIndex = 9000;

        private Action getPickaxeAction;

        public InteractiveType interactiveType = InteractiveType.Acquisition;

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