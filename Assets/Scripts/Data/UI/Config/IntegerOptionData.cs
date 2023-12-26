using UnityEngine;

namespace Data.UI.Config
{
    [CreateAssetMenu(fileName = "IntegerOptionData", menuName = "UI/Config/IntegerOptionData", order = 1)]
    public class IntegerOptionData : BaseConfigOptionData<int>
    {
        protected override string ValueString(int value)
        {
            return value.ToString();
        }

        public override DataType GetDataType()
        {
            return DataType.Int;
        }
    }
}