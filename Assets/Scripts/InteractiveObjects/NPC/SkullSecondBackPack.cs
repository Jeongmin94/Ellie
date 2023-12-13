using Assets.Scripts.Player;
using Channels.UI;
using System;
using System.Collections;
using Assets.Scripts.Data.GoogleSheet;
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