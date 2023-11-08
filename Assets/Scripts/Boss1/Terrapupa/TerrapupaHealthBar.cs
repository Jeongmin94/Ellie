using Assets.Scripts.Combat;
using Assets.Scripts.Managers;
using Assets.Scripts.Monsters.Utility;
using Assets.Scripts.UI.Framework.Billboard;
using Assets.Scripts.UI.Monster;
using Assets.Scripts.Utils;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrapupaHealthBar : MonoBehaviour, ICombatant
{
    [SerializeField] private TerrapupaRootData data;

    private UIMonsterBillboard billboard;
    private readonly MonsterDataContainer dataContainer = new MonsterDataContainer();

    private int a = 100;

    private void Start()
    {
        InitUI();
        InitData();
    }

    private void InitUI()
    {
        Transform billboardPos = Functions.FindChildByName(gameObject, "Billboard").transform;

        billboard = UIManager.Instance.MakeStatic<UIMonsterBillboard>(billboardPos, UIManager.UIMonsterBillboard);
        billboard.scaleFactor = 0.003f;
        billboard.InitBillboard(billboardPos);
    }

    public void InitData()
    {
        dataContainer.MaxHp = data.currentHP.value;
        dataContainer.CurrentHp.Value = (int)Mathf.Ceil(data.currentHP.value);
        dataContainer.Name = data.bossName;

        billboard.InitData(dataContainer);
    }

    public void Attack(IBaseEventPayload payload)
    {

    }

    public void ReceiveDamage(IBaseEventPayload payload)
    {
        CombatPayload combatPayload = payload as CombatPayload;

        dataContainer.CurrentHp.Value -= combatPayload.Damage;
    }
}
