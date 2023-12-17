using System.Collections;
using Assets.Scripts.Boss1.TerrapupaMinion;
using Assets.Scripts.Managers;
using Assets.Scripts.Player.HitComponent;
using Assets.Scripts.Utils;
using Channels.Combat;
using Channels.Components;
using UnityEngine;

public class TerrapupaMinionBTController : BehaviourTreeController
{
    [HideInInspector] public TerrapupaMinionRootData minionData;
    [SerializeField] private TerrapupaMinionHealthBar healthBar;
    [SerializeField] private TerrapupaMinionWeakPoint[] weakPoints;


    private float shakeDuration = 0.05f;
    private float shakeMagnitude = 0.05f;

    private MaterialHitComponent hitComponent;
    private TicketMachine ticketMachine;
    private bool isDead = false;

    public TerrapupaMinionHealthBar HealthBar
    {
        get { return healthBar; }
    }

    protected override void Awake()
    {
        base.Awake();

        minionData = rootTreeData as TerrapupaMinionRootData;
        healthBar = gameObject.GetOrAddComponent<TerrapupaMinionHealthBar>();
        weakPoints = GetComponentsInChildren<TerrapupaMinionWeakPoint>();
        hitComponent = gameObject.GetComponent<MaterialHitComponent>();

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
    }

    private void OnCollidedCoreByPlayerStone(IBaseEventPayload payload)
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
        if(!isDead)
        {
            ShowBillboard();
            StartCoroutine(ShakeCoroutine());
            hitComponent.Hit();

            healthBar.RenewHealthBar(minionData.currentHP.value - damageValue);
            minionData.currentHP.Value -= damageValue;

            if (minionData.currentHP.value <= 0)
            {
                Dead();
            }

            minionData.isHit.Value = true;
        }
    }

    public void GetHealed(int healValue)
    {
        if (!isDead)
        {
            healthBar.RenewHealthBar(minionData.currentHP.value + healValue);
            minionData.currentHP.Value += healValue;

            if (minionData.currentHP.value > minionData.hp)
            {
                ShowBillboard();
                minionData.currentHP.Value = minionData.hp;
                healthBar.RenewHealthBar(minionData.currentHP.value);
            }
        }
    }
    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0.0f;

        Vector3 originalPosition = transform.position;

        while (elapsed < shakeDuration)
        {
            transform.position = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            elapsed += Time.deltaTime;
            yield return null; // 다음 프레임까지 기다림
        }

        transform.position = originalPosition; // 원래 위치로 돌아감
    }

    public void Dead()
    {
        isDead = true;
        minionData.currentHP.Value = 0;
        healthBar.RenewHealthBar(0);
        billboardObject.gameObject.SetActive(false);
    }
}