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

public class TerrapupaHealthBar : MonoBehaviour
{
    [SerializeField] private TerrapupaRootData data;

    private UIMonsterBillboard billboard;
    private readonly MonsterDataContainer dataContainer = new MonsterDataContainer();

    private int a = 100;

    private void Awake()
    {
        InitUI();
    }

    private void InitUI()
    {
        Transform billboardPos = Functions.FindChildByName(gameObject, "Billboard").transform;

        billboard = UIManager.Instance.MakeStatic<UIMonsterBillboard>(billboardPos, UIManager.UIMonsterBillboard);
        billboard.scaleFactor = 0.003f;
        billboard.InitBillboard(billboardPos);
    }

    public void InitData(TerrapupaRootData data)
    {
        this.data = data;
        dataContainer.MaxHp = data.hp;
        dataContainer.CurrentHp.Value = (int)Mathf.Ceil(data.hp);
        dataContainer.Name = data.bossName;

        billboard.InitData(dataContainer);
    }

    public void RenewHealthBar(int currentHP)
    {
        if (currentHP >= 0)
        {
            dataContainer.CurrentHp.Value = currentHP;
        }
    }
}
