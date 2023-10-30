using Assets.Scripts.Combat;
using Assets.Scripts.Player;
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
        private TicketMachine ticketMachine;

        private void Awake()
        {
            SetTicketMachine();
        }
        private void SetTicketMachine()
        {
            Debug.Log("TestAttacker SetTicketMachine()");
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

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                TestAttack();
            }
        }
        private void TestAttack()
        {
            CombatPayload payload = new();
            payload.Type = CombatType.Melee;
            payload.Attacker = transform;
            payload.Defender = playerStatus.transform;
            payload.AttackDirection = Vector3.zero;
            payload.AttackStartPosition = transform.position;
            payload.AttackPosition = playerStatus.transform.position;
            payload.PlayerStatusEffectName = StatusEffects.PlayerStatusEffectName.WeakRigidity;
            payload.Damage = 5;
            Attack(payload);
        }
    }
}