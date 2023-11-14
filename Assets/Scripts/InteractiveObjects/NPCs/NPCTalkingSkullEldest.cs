using Assets.Scripts.Combat;
using Assets.Scripts.Player;
using Channels.Combat;
using UnityEngine;
namespace Assets.Scripts.InteractiveObjects.NPCs
{
    public class NPCTalkingSkullEldest : QuestNPC, ICombatant
    {
        private bool cleared = false;
        private int hitCount = 0;
        public void Attack(IBaseEventPayload payload)
        {
        }

        public void ReceiveDamage(IBaseEventPayload payload)
        {
            CombatPayload combatPayload = payload as CombatPayload;
            if (combatPayload.Attacker.CompareTag("Player") || combatPayload.Attacker.CompareTag("Stone"))
            {
                PlayerQuest playerQuest = combatPayload.Attacker.GetComponent<PlayerQuest>();
                if (playerQuest.GetQuestStatus(data.questIdx) != Data.Quest.QuestStatus.Accepted)
                    return;
                hitCount++;
                Debug.Log("Eldest hit");
                if (hitCount >= 3)
                {
                    playerQuest.RenewQuestStatus(data.questIdx, Data.Quest.QuestStatus.Done);
                    cleared = true;
                }
            }
        }
    }
}
