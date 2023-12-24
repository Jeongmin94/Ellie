using Channels.Combat;
using Channels.Components;
using Channels.Type;
using Combat;
using Player;
using Player.StatusEffects;
using UnityEngine;
using Utils;

namespace Centers.Test
{
    public class TestAttacker : MonoBehaviour, ICombatant
    {
        public PlayerStatus playerStatus;
        public int testDamage;
        public float testEffectDuration;
        public float testForce;
        public StatusEffectName statusEffect;
        private TicketMachine ticketMachine;

        private void Awake()
        {
            SetTicketMachine();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                TestAttack();
            }
        }

        public void Attack(IBaseEventPayload payload)
        {
            ticketMachine.SendMessage(ChannelType.Combat, payload);
        }

        public void ReceiveDamage(IBaseEventPayload payload)
        {
        }

        private void SetTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.Combat);

            //ticketMachine.AddTicket(ChannelType.UI, new Ticket<IBaseEventPayload>());
        }

        private void TestAttack()
        {
            CombatPayload payload = new()
            {
                Type = CombatType.Melee,
                Attacker = transform,
                Defender = playerStatus.gameObject.transform,
                AttackDirection = Vector3.zero,
                AttackStartPosition = transform.position,
                AttackPosition = playerStatus.gameObject.transform.position,
                StatusEffectName = statusEffect,
                statusEffectduration = testEffectDuration,
                force = testForce,
                Damage = testDamage
            };
            Attack(payload);
        }
    }
}