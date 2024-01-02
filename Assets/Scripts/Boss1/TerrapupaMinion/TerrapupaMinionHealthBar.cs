using Boss1.DataScript.Minion;
using UnityEngine;

namespace Boss1.TerrapupaMinion
{
    public class TerrapupaMinionHealthBar : HelathBarController
    {
        private TerrapupaMinionRootData minionData;
        
        public override void InitData(BaseBTData data)
        {
            minionData = data as TerrapupaMinionRootData;

            dataContainer.MaxHp = minionData.hp;
            dataContainer.CurrentHp.Value = (int)Mathf.Ceil(minionData.hp);
            dataContainer.Name = minionData.bossName;

            RenewHealthBar(dataContainer.CurrentHp.Value - 1);
            RenewHealthBar(dataContainer.CurrentHp.Value + 1);
            billboard.InitData(dataContainer);
        }
    }
}
