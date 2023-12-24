using System;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using Player.StatusEffects;
using UnityEngine;
using Utils;

namespace InteractiveObjects.NPC
{
    public class SkullSecondTrap : MonoBehaviour
    {
        private bool isTrapActivated;
        private TicketMachine ticketMachine;

        private Action trapHitAction;


        private void Awake()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();

            ticketMachine.AddTickets(ChannelType.Combat);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isTrapActivated && other.CompareTag("Player"))
            {
                //플레이어에 닿으면 상태이상 주고 데미지
                ticketMachine.SendMessage(ChannelType.Combat, new CombatPayload
                {
                    Type = CombatType.Melee,
                    Damage = 1,
                    Defender = other.transform,
                    StatusEffectName = StatusEffectName.KnockedAirborne,
                    statusEffectduration = 1.5f,
                    force = 15f
                });

                isTrapActivated = true;

                Publish();
            }
        }

        public void SubscribeTrapHitAction(Action listener)
        {
            trapHitAction -= listener;
            trapHitAction += listener;
        }

        private void Publish()
        {
            trapHitAction.Invoke();
        }
    }
}