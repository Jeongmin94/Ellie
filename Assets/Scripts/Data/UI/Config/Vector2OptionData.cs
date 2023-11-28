using UnityEngine;

namespace Data.UI.Config
{
    [CreateAssetMenu(fileName = "Vector2OptionData", menuName = "UI/Config/Vector2OptionData", order = 2)]
    public class Vector2OptionData : BaseConfigOptionData<Vector2>
    {
        public override string ValueString(Vector2 value)
        {
            return $"x: {value.x}, y: {value.y}";
        }
    }
}