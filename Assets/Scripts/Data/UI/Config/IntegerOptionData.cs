using UnityEngine;

namespace Data.UI.Config
{
    [CreateAssetMenu(fileName = "IntegerOptionData", menuName = "UI/Config/IntegerOptionData", order = 0)]
    public class IntegerOptionData : BaseConfigOptionData<int>
    {
        // public readonly <int> actionValue = new Data<int>();
        
        public override void OnBeforeSerialize()
        {
            
        }

        public override void OnAfterDeserialize()
        {
            
        }

        public override string ValueString(int value)
        {
            return value.ToString();
        }
    }
}