using UnityEngine;

namespace Data.UI.Config
{
    [CreateAssetMenu(fileName = "IntegerOptionData", menuName = "UI/Config/IntegerOptionData", order = 1)]
    public class IntegerOptionData : BaseConfigOptionData<int>
    {
        // public readonly <int> actionValue = new Data<int>();

        protected override string ValueString(int value)
        {
            return value.ToString();
        }
    }
}