using Assets.Scripts.Managers;
using Boss1.DataScript.Minion;
using Managers.UI;
using Monsters.Utility;
using UI.Monster;
using UnityEngine;

namespace Boss1.TerrapupaMinion
{
    public class TerrapupaMinionHealthBar : MonoBehaviour
    {
        public float scaleFactor = 0.003f;
        private readonly MonsterDataContainer dataContainer = new();

        private UIMonsterBillboard billboard;
        private TerrapupaMinionRootData data;

        private void Awake()
        {
            InitUI();
        }

        private void InitUI()
        {
            var billboardPos = Functions.FindChildByName(gameObject, "Billboard").transform;

            billboard = UIManager.Instance.MakeStatic<UIMonsterBillboard>(billboardPos, UIManager.UIMonsterBillboard);
            billboard.scaleFactor = scaleFactor;
            billboard.InitBillboard(billboardPos);
        }

        public void InitData(TerrapupaMinionRootData data)
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
}