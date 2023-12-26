using Assets.Scripts.Player;
using Channels.UI;
using System;
using System.Collections;
using Assets.Scripts.Data.GoogleSheet;
using Outline;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects.NPC
{
    public class SkullSecondBackPack : InteractiveObject
    {
        [SerializeField] private Renderer renderer;

        private Action getBackPackAction;

        InteractiveType interactiveType = InteractiveType.Acquisition;

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
            if (obj.GetComponent<PlayerQuest>().GetQuestStatus(6104) != QuestStatus.Accepted)
            {
                return;
            }

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