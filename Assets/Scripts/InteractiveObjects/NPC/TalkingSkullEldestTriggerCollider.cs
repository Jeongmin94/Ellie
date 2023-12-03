using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.Player;
using System;
using UnityEngine;

namespace Assets.Scripts.InteractiveObjects.NPC
{
    public class TalkingSkullEldestTriggerCollider : MonoBehaviour
    {
        private PlayerQuest player;
        private Action<Collider> firstEncounterAction;
        private Action secondEncounterAction;
        private Action playerExitAction;

        private void Update()
        {
            if (player != null &&
                player.GetQuestStatus(6101) == QuestStatus.Unaccepted &&
                Vector3.Distance(player.transform.position, transform.position) < 7.0f)
            {
                secondEncounterAction?.Invoke();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                player = other.GetComponent<PlayerQuest>();
                firstEncounterAction?.Invoke(other);
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (!other.CompareTag("Player")) return;
                if (player.GetQuestStatus(6101) <= QuestStatus.Accepted)
                    playerExitAction?.Invoke();
            }
        }

        public void SubscribeFirstEncounterAction(Action<Collider> listener)
        {
            firstEncounterAction -= listener;
            firstEncounterAction += listener;
        }

        public void SubscribeSecondEncounterAction(Action listener)
        {
            secondEncounterAction -= listener;
            secondEncounterAction += listener;
        }

        public void SubscribePlayerExitAction(Action listener)
        {
            playerExitAction -= listener;
            playerExitAction += listener;
        }

        private void OnDisable()
        {
            firstEncounterAction = null;
            secondEncounterAction = null;
        }
    }
}
