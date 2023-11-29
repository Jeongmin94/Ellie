using UnityEngine;

namespace Data.UI.Config
{
    [CreateAssetMenu(fileName = "ResolutionData", menuName = "UI/Config/ResolutionData", order = 3)]
    public class ResolutionData : Vector2OptionData
    {
        protected override string ValueString(Vector2 value)
        {
            return $"{(int)value.x} X {(int)value.y}";
        }
    }
}