using UnityEngine;

namespace Data.UI.Config
{
    [CreateAssetMenu(fileName = "StringOptionData", menuName = "UI/Config/StringOptionData", order = 4)]
    public class StringOptionData : BaseConfigOptionData<string>
    {
        protected override string ValueString(string value)
        {
            return value;
        }

        public override DataType GetDataType()
        {
            return DataType.String;
        }
    }
}