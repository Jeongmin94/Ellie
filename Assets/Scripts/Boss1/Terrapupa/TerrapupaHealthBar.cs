using Assets.Scripts.Managers;
using Assets.Scripts.Monsters.Utility;
using Assets.Scripts.UI.Monster;
using UnityEngine;

public class TerrapupaHealthBar : MonoBehaviour
{
    [SerializeField] private TerrapupaRootData data;

    private UIMonsterBillboard billboard;
    private readonly MonsterDataContainer dataContainer = new MonsterDataContainer();

    public float scaleFactor = 0.003f;

    private void Awake()
    {
        InitUI();
    }

    private void InitUI()
    {
        Transform billboardPos = Functions.FindChildByName(gameObject, "Billboard").transform;

        billboard = UIManager.Instance.MakeStatic<UIMonsterBillboard>(billboardPos, UIManager.UIMonsterBillboard);
        billboard.scaleFactor = scaleFactor;
        billboard.InitBillboard(billboardPos);
    }

    public void InitData(TerrapupaRootData data)
    {
        this.data = data;
        dataContainer.MaxHp = data.hp;
        dataContainer.CurrentHp.Value = (int)Mathf.Ceil(data.hp);
        dataContainer.Name = data.bossName;

        RenewHealthBar(dataContainer.CurrentHp.Value - 1);
        RenewHealthBar(dataContainer.CurrentHp.Value + 1);
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
