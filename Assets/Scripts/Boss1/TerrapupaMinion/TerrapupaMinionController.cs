using Assets.Scripts.Combat;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using Boss.Terrapupa;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrapupaMinionController : BehaviourTreeController, ICombatant
{
    [SerializeField] private TerrapupaMinionHealthBar healthBar;
    [HideInInspector] public TerrapupaMinionRootData minionData;

    private TicketMachine ticketMachine;

    public TicketMachine TicketMachine
    {
        get { return ticketMachine; }
    }

    public TerrapupaMinionHealthBar HealthBar
    {
        get { return healthBar; }
    }

    protected override void Awake()
    {
        base.Awake();

        InitTicketMachine();
        minionData = rootTreeData as TerrapupaMinionRootData;
        healthBar = gameObject.GetOrAddComponent<TerrapupaMinionHealthBar>();
    }

    private void Start()
    {
        InitStatus();
    }

    private void InitTicketMachine()
    {
        ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
        ticketMachine.AddTickets(ChannelType.Combat, ChannelType.Terrapupa, ChannelType.BossInteraction);
    }

    private void InitStatus()
    {
        healthBar.InitData(minionData);
    }

    public void Attack(IBaseEventPayload payload)
    {

    }

    public void ReceiveDamage(IBaseEventPayload payload)
    {
        Debug.Log($"ReceiveDamage :: {payload}");

        CombatPayload combatPayload = payload as CombatPayload;
        PoolManager.Instance.Push(combatPayload.Attacker.GetComponent<Poolable>());
        int damage = combatPayload.Damage;

        GetDamaged(damage);
        Debug.Log($"{damage} 데미지 입음 : {minionData.currentHP.Value}");
    }

    public void GetDamaged(int damageValue)
    {
        minionData.currentHP.Value -= damageValue;
        healthBar.RenewHealthBar(minionData.currentHP.value);
    }
}
