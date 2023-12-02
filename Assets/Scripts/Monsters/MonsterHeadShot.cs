using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Combat;
using Assets.Scripts.Monsters;
using Assets.Scripts.Monsters.AbstractClass;
using Assets.Scripts.Utils;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using UnityEngine;

public class MonsterHeadShot : MonoBehaviour, ICombatant
{
    [SerializeField] private AbstractMonster controller;
    private TicketMachine ticketMachine;

    private void Awake()
    {
        SetTicketMachine();
    }

    private void SetTicketMachine()
    {
        ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
        ticketMachine.AddTicket(ChannelType.Combat);
    }

    public void Attack(IBaseEventPayload payload)
    {
        throw new System.NotImplementedException();
    }

    public void ReceiveDamage(IBaseEventPayload payload)
    {
        controller.RecieveHeadShot();
        CombatPayload combatPayload = payload as CombatPayload;
        controller.UpdateHP(combatPayload.Damage);
    }
}
