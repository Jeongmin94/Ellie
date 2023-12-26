using UnityEngine;

namespace Data.UI.Config
{
    [CreateAssetMenu(fileName = "BooleanOptionData", menuName = "UI/Config/BooleanOptionData", order = 0)]
    public class BooleanOptionData : BaseConfigOptionData<bool>
    {
        protected override string ValueString(bool value)
        {
            return value.ToString();
        }

        public override DataType GetDataType()
        {
            return DataType.Boolean;
        }
    }
}