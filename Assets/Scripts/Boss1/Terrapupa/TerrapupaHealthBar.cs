using Boss1.DataScript.Terrapupa;
using UnityEngine;

namespace Boss1.Terrapupa
{
    public class TerrapupaHealthBar : HelathBarController
    {
        private TerrapupaRootData terrapupaData;
        
        public override void InitData(BehaviourTreeData data)
        {
            terrapupaData = data as TerrapupaRootData;

            dataContainer.MaxHp = terrapupaData.hp;
            dataContainer.CurrentHp.Value = (int)Mathf.Ceil(terrapupaData.hp);
            dataContainer.Name = terrapupaData.bossName;

            RenewHealthBar(dataContainer.CurrentHp.Value - 1);
            RenewHealthBar(dataContainer.CurrentHp.Value + 1);
            billboard.InitData(dataContainer);
        }
    }
}
