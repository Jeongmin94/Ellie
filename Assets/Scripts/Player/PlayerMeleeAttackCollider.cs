using Assets.Scripts.Combat;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerMeleeAttackCollider : MonoBehaviour
    {
        private TicketMachine ticketMachine;

        private void Start()
        {
            ticketMachine = FindRootParent(gameObject).GetComponent<PlayerController>().TicketMachine;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (FindRootParent(other.gameObject).CompareTag("Player"))
            {
                return;
            }

            var enemy = other.GetComponent<ICombatant>();
            if (enemy != null)
            {
                ticketMachine.SendMessage(ChannelType.Combat, GenerateMeleeAttackPayload(other.transform));
            }
        }

        private GameObject FindRootParent(GameObject obj)
        {
            var parentTransform = obj.transform;

            while (parentTransform.parent != null)
            {
                parentTransform = parentTransform.parent;
            }

            return parentTransform.gameObject;
        }

        private CombatPayload GenerateMeleeAttackPayload(Transform defender)
        {
            var payload = new CombatPayload();

            payload.Type = CombatType.Melee;
            payload.Attacker = FindRootParent(gameObject).transform;
            payload.Defender = defender;
            payload.Damage = 1;

            return payload;
        }
    }
}