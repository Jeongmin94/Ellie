using Assets.Scripts.Combat;
using Assets.Scripts.Player;
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
                Attack(GenerateStonePayload(collision.transform));
            }
        }

        private CombatPayload GenerateStonePayload(Transform defender)
        {
            var payload = new CombatPayload();
            // !TODO : 돌맹이 데이터 읽어와서 현재 돌맹이에 맞는 값으로 페이로드 초기화

            //=> test
            payload.Type = CombatType.Melee;
            payload.Attacker = transform;
            payload.Defender = defender;
            payload.AttackDirection = Vector3.zero;
            payload.AttackStartPosition = transform.position;
            payload.AttackPosition = defender.position;
            payload.Damage = 5;
            //<=
            return payload;
        }
    }
}
