using Assets.Scripts.Combat;
using Assets.Scripts.Utils;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using UnityEngine;
namespace Assets.Scripts.Item.Stone
{
    public class TestStoneLootable : BaseStone, ICombatant
    {
        private TicketMachine ticketMachine;
        private void Awake()
        {
            SetTicketMachine();
        }

        private void SetTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.Combat);
        }
        public void Attack(IBaseEventPayload payload)
        { 
            ticketMachine.SendMessage(ChannelType.Combat, payload);
        }

        public void ReceiveDamage(IBaseEventPayload payload)
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            ICombatant enemy = collision.gameObject.GetComponent<ICombatant>();
            if (enemy != null)
            {

            }
        }

        private CombatPayload GenerateStonePayload()
        {
            var payload = new CombatPayload();
            return payload;
        }
    }
}
