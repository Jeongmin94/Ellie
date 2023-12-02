using Assets.Scripts.Boss1.TerrapupaMinion;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using Channels.Combat;
using Channels.Components;
using UnityEngine;

public class TerrapupaMinionBTController : BehaviourTreeController
{
    [HideInInspector] public TerrapupaMinionRootData minionData;
    [SerializeField] private TerrapupaMinionHealthBar healthBar;
    [SerializeField] private TerrapupaMinionWeakPoint[] weakPoints;

    private TicketMachine ticketMachine;

    public TerrapupaMinionHealthBar HealthBar
    {
        get { return healthBar; }
    }

    private void Awake()
    {
        minionData = rootTreeData as TerrapupaMinionRootData;
        healthBar = gameObject.GetOrAddComponent<TerrapupaMinionHealthBar>();
        weakPoints = GetComponentsInChildren<TerrapupaMinionWeakPoint>();

        SubscribeEvent();
    }

    private void Start()
    {
        InitStatus();
    }

    private void SubscribeEvent()
    {
        foreach (var weakPoint in weakPoints) 
        { 
            weakPoint.SubscribeCollisionAction(OnCollidedCoreByPlayerStone);
        }
    }

    public void InitTicketMachine(TicketMachine ticketMachine)
    {
        this.ticketMachine = ticketMachine;
    }

    private void InitStatus()
    {
        healthBar.InitData(minionData);
        GetHealed(1);
    }

    private void OnCollidedCoreByPlayerStone(IBaseEventPayload payload)
    {
        Debug.Log($"ReceiveDamage :: {payload}");

        CombatPayload combatPayload = payload as CombatPayload;
        PoolManager.Instance.Push(combatPayload.Attacker.GetComponent<Poolable>());
        int damage = combatPayload.Damage;

        GetDamaged(damage);
        Debug.Log($"{damage} ������ ���� : {minionData.currentHP.Value}");
    }

    public void GetDamaged(int damageValue)
    {
        healthBar.RenewHealthBar(minionData.currentHP.value - damageValue);
        minionData.currentHP.Value -= damageValue;
        if (minionData.currentHP.value <= 0)
        {
            minionData.currentHP.Value = 0;
            healthBar.RenewHealthBar(0);
        }
        minionData.isHit.Value = true;
    }

    public void GetHealed(int healValue)
    {
        healthBar.RenewHealthBar(minionData.currentHP.value + healValue);
        minionData.currentHP.Value += healValue;

        if (minionData.currentHP.value > minionData.hp)
        {
            minionData.currentHP.Value = minionData.hp;
            healthBar.RenewHealthBar(minionData.currentHP.value);
        }
    }
}
