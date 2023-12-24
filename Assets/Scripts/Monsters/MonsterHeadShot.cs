using System;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using Combat;
using Monsters.AbstractClass;
using UnityEngine;
using Utils;

namespace Monsters
{
    public class MonsterHeadShot : MonoBehaviour, ICombatant
    {
        [SerializeField] private AbstractMonster controller;
        private TicketMachine ticketMachine;

        private void Awake()
        {
            SetTicketMachine();
        }

        public void Attack(IBaseEventPayload payload)
        {
            throw new NotImplementedException();
        }

        public void ReceiveDamage(IBaseEventPayload payload)
        {
            controller.RecieveHeadShot();
            var combatPayload = payload as CombatPayload;
            controller.UpdateHP(combatPayload.Damage);
        }

        private void SetTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTicket(ChannelType.Combat);
        }
    }
}