using Assets.Scripts.Combat;
using Assets.Scripts.Player;
using Assets.Scripts.StatusEffects;
using Assets.Scripts.Utils;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Centers.Test
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
        private void SetTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.Combat);

            //ticketMachine.AddTicket(ChannelType.UI, new Ticket<IBaseEventPayload>());
        }
        public void Attack(IBaseEventPayload payload)
        {
            ticketMachine.SendMessage(ChannelType.Combat, payload);
        }

        public void ReceiveDamage(IBaseEventPayload payload)
        {

        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                TestAttack();
            }
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